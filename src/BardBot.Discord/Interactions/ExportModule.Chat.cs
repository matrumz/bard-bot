using Discord;
using Discord.Interactions;

namespace BardBot.Discord.Interactions;

public sealed partial class ExportModule
{
    [SlashCommand("chat", "Configured channels & threads.")]
    public async Task ExportChatAsync() =>
        await Context.Interaction.RespondWithModalAsync<ChatExportModal>(ChatExportModal.CustomId);

    public sealed class ChatExportModal : IModal
    {
        public const string CustomId = "chat_export_modal";

        public string Title => "Export Chat";

        [InputLabel("After Date")]
        [RequiredInput(false)]
        [ModalTextInput("after_date", style: TextInputStyle.Short, placeholder: @"""last"" or a date with time")]
        public string AfterDate { get; set; }

        [InputLabel("Before Date")]
        [RequiredInput(false)]
        [ModalTextInput("before_date", style: TextInputStyle.Short, placeholder: @"""now"" or a date with time")]
        public string BeforeDate { get; set; }

        [InputLabel("Bulk Export")]
        [RequiredInput(false)]
        [ModalTextInput("bulk_export", style: TextInputStyle.Paragraph, placeholder: "CSV of after/before dates")]
        public string BulkExport { get; set; }

    }

    [ModalInteraction(ChatExportModal.CustomId)]
    public async Task ModalResponseAsync(ChatExportModal modal)
    {
        await RespondAsync(modal.BulkExport);
    }

}
