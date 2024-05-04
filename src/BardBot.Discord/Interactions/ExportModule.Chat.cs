using System.Text;

using BardBot.Discord.Exporting;

using Discord;
using Discord.Interactions;

using Microsoft.Extensions.Logging;

namespace BardBot.Discord.Interactions;

public sealed partial class ExportModule
{
    [SlashCommand("chat", "Configured channels & threads.")]
    public async Task ExportChatAsync() =>
        await Context.Interaction.RespondWithModalAsync<ChatExportModal>($"{GroupName}:{ChatExportModal.CustomId}");

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

            var ranges = new List<AfterBeforeDate>{
                new(DateTime.Now, DateTime.Now)
            };

            var job = exportJobFactory.CreateChatExportJob(Context.Guild, ranges);
            var files = await job.ToFiles();

            var message = new StringBuilder()
                .AppendLine($"Exported {files.Count()} files{(files.Any() ? ":" : ".")}")
                .AppendJoin('\n', files.Select(file => Format.Sanitize(file.FullName)))
                .ToString();

            await Context.Interaction.ModifyOriginalResponseAsync(orig => orig.Content = message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to export chat.");
            await Context.Interaction.ModifyOriginalResponseAsync(orig => orig.Content = "Failed to export chat.");
        }
    }


}
