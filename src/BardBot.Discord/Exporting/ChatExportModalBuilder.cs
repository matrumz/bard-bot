using Discord;

namespace BardBot.Discord.Exporting;

internal sealed class ChatExportModalBuilder : ModalBuilder
{
    private const string AfterDateId = "after_date";
    private const string BeforeDateId = "before_date";
    private const string BulkExportId = "bulk_export";

    public ChatExportModalBuilder() : base()
    {
        WithCustomId("chat_export_modal");
        WithTitle("Export Chat");
        AddTextInput("After Date", AfterDateId, TextInputStyle.Short, placeholder: @"""last"" or a date with time", required: false);
        AddTextInput("Before Date", BeforeDateId, TextInputStyle.Short, placeholder: @"""now"" or a date with time", required: false);
        AddTextInput("Bulk Export", BulkExportId, TextInputStyle.Paragraph, placeholder: "CSV of after/before dates", required: false);
    }

    public ChatExportModalBuilder WithAfterDate(string afterDate)
    {
        UpdateTextInput(AfterDateId, afterDate);
        return this;
    }

    public ChatExportModalBuilder WithBeforeDate(string beforeDate)
    {
        UpdateTextInput(BeforeDateId, beforeDate);
        return this;
    }

    public ChatExportModalBuilder WithBulkExport(string bulkExport)
    {
        UpdateTextInput(BulkExportId, bulkExport);
        return this;
    }

}
