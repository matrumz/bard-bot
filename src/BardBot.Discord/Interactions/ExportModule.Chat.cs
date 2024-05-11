using System.Text;

using BardBot.Discord.Common;
using BardBot.Discord.Exporting;
using BardBot.Discord.Exporting.PathTokens;

using Discord;
using Discord.Interactions;

using Microsoft.Extensions.Logging;

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

        public string Title => "Export Chat";

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
        [ModalTextInput("bulk_export", style: TextInputStyle.Paragraph, placeholder: "CSV or JSON Array of after/before dates")]
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
                CheckFormats = [Token.DefaultDateTimeFormat],
                Last = LastChatExport,
            };
            var ranges = new List<AfterBeforeDate>();

            // Add single After/Before fields (if at least one specified)
            if (modal.AfterDate is not null || modal.BeforeDate is not null)
                ranges.Add(new(
                    converter.TryParse(modal.AfterDate, out var after) ? after.Value : DateTime.MinValue,
                    converter.TryParse(modal.BeforeDate, out var before) ? before.Value : DateTime.MaxValue
                ));

            // Add bulk export ranges (if specified)

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
