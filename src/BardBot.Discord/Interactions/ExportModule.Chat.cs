using System.Text;

using BardBot.Discord.Common;
using BardBot.Discord.Exporting;

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

    }

    [ModalInteraction(ChatExportModal.CustomId)]
    public async Task ModalResponseAsync(ChatExportModal modal)
    {
        try
        {
            await Context.Interaction.RespondAsync("Exporting chat...");

            // Prep datetime converter & aggregate list of ranges
            var converter = new DateTimeConverter
            {
                CheckFormats = modal.DateFormat is not null ? [modal.DateFormat] : [],
                Last = LastChatExport,
            };
            var ranges = new List<AfterBeforeDate>();

            // Add single After/Before fields (if at least one specified)
            if (modal.AfterDate is not null || modal.BeforeDate is not null)
                ranges.Add(new(
                    converter.TryParse(modal.AfterDate, out var after) ? after.Value : DateTime.MinValue,
                    converter.TryParse(modal.BeforeDate, out var before) ? before.Value : DateTime.MaxValue
                ));

            // Load bulk ranges
            // TODO: record exceptions at each level in case all fail -> then report at end
            // Start by attempting to parse YAML
            try
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .WithTypeConverter(converter)
                    .Build();
                var yamlRanges = deserializer.Deserialize<List<AfterBeforeDate>>(modal.BulkExport ?? "[]") ?? [];
            }
            catch (YamlException)
            {
                // Fall back to parsing CSV
                try
                {
                    var csvRanges = modal.BulkExport?.Split('\n')
                        .Select(line => line.Split(','))
                        .Select(parts => new AfterBeforeDate(
                            converter.TryParse(parts.ElementAtOrDefault(0), out var after) ? after.Value : throw new FormatException("Invalid CSV After Date."),
                            converter.TryParse(parts.ElementAtOrDefault(1), out var before) ? before.Value : throw new FormatException("Invalid CSV Before Date.")
                        ))
                        ?? [];
                    ranges.AddRange(csvRanges);
                }
                catch (Exception)
                {
                    throw new FormatException("Invalid Bulk Export format.");
                }
            }

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
