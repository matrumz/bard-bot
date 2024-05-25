using System.Text;

using BardBot.Common;
using BardBot.Common.Extensions;
using BardBot.Discord.Database.Models;
using BardBot.Discord.Exporting;
using BardBot.Discord.Exporting.Chat;
using BardBot.Discord.Exporting.PathTokens;

using Discord;
using Discord.Interactions;

using Microsoft.Extensions.Logging;

using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BardBot.Discord.Interactions;

public sealed partial class ExportModule
{
    private DateTime? _lastChatExport;
    private DateTime? LastChatExport => _lastChatExport ??= chatExportHistoryRepository.LastChatExport(Context.Guild.Id);

    private Campaign? Campaign => campaignRepository.Get(Context.Guild.Id);

    [SlashCommand("chat", "Configured channels & threads.")]
    public async Task ExportChatAsync() =>
        await Context.Interaction.RespondWithModalAsync<ChatExportModal>($"{GroupName}:{ChatExportModal.CustomId}", modifyModal: (modal) =>
        {
            modal.UpdateTextInput("after_date", LastChatExport?.ToString());
            modal.UpdateTextInput("before_date", DateTime.Now.ToString());
        });

    public sealed class ChatExportModal : IModal
    {
        public const string CustomId = "chat_export_modal";
        public const string DefaultDateTimeFormat = "yyyy-MM-dd-HHmm";

        public string Title => "Export Chat";

        [InputLabel("Date format")]
        [RequiredInput(false)]
        [ModalTextInput("date_format", style: TextInputStyle.Short, initValue: DefaultDateTimeFormat, placeholder: "Format of dates in this form. e.g. yyyy-MM-dd HH:mm:ss")]
        public string? DateFormat { get; set; }

        [InputLabel("After Date")]
        [RequiredInput(false)]
        [ModalTextInput("after_date", style: TextInputStyle.Short, placeholder: @"""$last"" or a date with time")]
        public string? AfterDate { get; set; }

        [InputLabel("Before Date")]
        [RequiredInput(false)]
        [ModalTextInput("before_date", style: TextInputStyle.Short, placeholder: @"""$now"" or a date with time")]
        public string? BeforeDate { get; set; }

        [InputLabel("Bulk Export")]
        [RequiredInput(false)]
        [ModalTextInput("bulk_export", style: TextInputStyle.Paragraph, placeholder: "CSV or YAML Array of after/before dates")]
        public string? BulkExport { get; set; }

        public IEnumerable<AfterBeforeDate> GetDateRanges(DateTimeFactory dateTimeFactory, DateTime? lastChatExport)
        {
            // Prep customizations for DateTimeFactory
            var dateTimeFactoryAddons = (
                parsers: new DateTimeFactory.TryParser[] {
                    bool (string? input, out DateTime result) =>
                    {
                        switch (input)
                        {
                            case "$now":
                                result = DateTime.Now;
                                return true;
                            case "$last":
                                result = lastChatExport ?? DateTime.MinValue;
                                return true;
                            default:
                                result = DateTime.MinValue;
                                return false;
                        }
                    }
                },
                formats: DateFormat is not null ? new[] { DateFormat } : []
            );

            // DateTimeFactory shortcut
            DateTime ParseDateTime(string input) =>
                dateTimeFactory.Parse(
                    input,
                    parsers: dateTimeFactoryAddons.parsers,
                    formats: dateTimeFactoryAddons.formats
                );

            // date ranges for export
            var ranges = new List<AfterBeforeDate>();

            // Add single After/Before fields (if at least one specified)
            if (!string.IsNullOrWhiteSpace(AfterDate) || !string.IsNullOrWhiteSpace(BeforeDate))
                ranges.Add(new(
                    !string.IsNullOrWhiteSpace(AfterDate) ? ParseDateTime(AfterDate) : DateTime.MinValue,
                    !string.IsNullOrWhiteSpace(BeforeDate) ? ParseDateTime(BeforeDate) : DateTime.MaxValue
                ));

            // Load bulk ranges
            // TODO: record exceptions at each level in case all fail -> then report at end
            // Start by attempting to parse YAML
            try
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .WithTypeConverter(dateTimeFactory.CreateYamlTypeConverter(
                        parsers: dateTimeFactoryAddons.parsers,
                        formats: dateTimeFactoryAddons.formats
                    ))
                    .Build();
                var yamlRanges = deserializer.Deserialize<List<AfterBeforeDate>>(BulkExport ?? "[]") ?? [];
            }
            catch (YamlException)
            {
                // Fall back to parsing CSV
                try
                {
                    var csvRanges = BulkExport?.Split('\n')
                        // Skip empty lines
                        .Where(line => !string.IsNullOrWhiteSpace(line))
                        .Select(line => line.Split(','))
                        .Select(parts => new AfterBeforeDate(
                            ParseDateTime(parts.ElementAt(0)),
                            ParseDateTime(parts.ElementAt(1))
                        ))
                        ?? [];
                    ranges.AddRange(csvRanges);
                }
                catch (Exception)
                {
                    throw new FormatException("Invalid Bulk Export format.");
                }
            }

            return ranges;
        }
    }

    [ModalInteraction(ChatExportModal.CustomId)]
    public async Task ModalResponseAsync(ChatExportModal modal)
    {
        try
        {
            await Context.Interaction.RespondAsync("Exporting chat...");

            // validate requirements for an export
            if (Campaign is null)
                throw new InvalidOperationException("No campaign found for this guild.");

            // get list of export time ranges
            var ranges = modal.GetDateRanges(dateTimeFactory, LastChatExport);

            // cross-join the date ranges with the channels to export to determine work needed
            var contexts = Campaign
                .Channels.Values
                .Where(channel => channel.ExportableGameChat ?? false)
                .CrossJoin(ranges)
                .Select(((Channel channel, AfterBeforeDate range) tuple) =>
                {
                    // build export path
                    var tokens = new List<Token>();
                    tuple.channel.Character.IfNotNull(character => tokens.Add(Token.Character(character)));
                    tokens.Add(Token.After(tuple.range.After));
                    tokens.Add(Token.Before(tuple.range.Before));
                    var path = Campaign.ExportPathTemplates?.Chats?.ApplyTokens(tokens) ?? throw new InvalidOperationException("No chat export path template found.");

                    // assemble export context
                    return new ChatExportContext(
                        Guild: Context.Guild,
                        Channel: (IMessageChannel)Context.Client.GetChannel(tuple.channel.Id),
                        OutputPath: path,
                        Format: ChatExportFormat.Markdown, // TODO: support other formats
                        After: tuple.range.After,
                        Before: tuple.range.Before
                    );
                });

            // Run the export job
            var job = exportJobFactory.CreateChatExportJob(Context.Guild, ranges);
            var files = await job.ToFiles();

            // Report job results
            var message = new StringBuilder()
                .AppendLine($"Exported {files.Count()} files{(files.Any() ? ":" : ".")}")
                .AppendJoin('\n', files.Select(file => Format.Sanitize(file.FullName)))
                .ToString();
            await Context.Interaction.ModifyOriginalResponseAsync(orig => orig.Content = message);

        }
        catch (Exception ex)
        {
            const string message = "Failed to export chat.";
            logger.LogError(ex, message);
            await Context.Interaction.ModifyOriginalResponseAsync(orig => orig.Content = message);
        }
    }

}
