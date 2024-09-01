using Scriban.Runtime;

namespace BardBot.Discord.Exporting;

public partial class ExportFilePathModel
{

    public DateTime? After { get; set; }
    public DateTime? Before { get; set; }
    public string? Character { get; set; }
    public string? Extension { get; set; }

}

public partial class ExportFilePathModel : ScriptObject;
