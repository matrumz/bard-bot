using Discord;

namespace BardBot.Discord.Exporting.Chat;

public record ChatExportContext(
    IGuild Guild,
    IEnumerable<AfterBeforeDate> Ranges
);
