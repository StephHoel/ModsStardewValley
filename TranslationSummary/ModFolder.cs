using TranslationSummary.Enums;

namespace TranslationSummary;

public record ModFolder(DirectoryInfo ModDir, string RelativePath, string ModName)
{
    public List<DirectoryInfo> TranslationDirs { get; } = new();
    public Dictionary<string, Dictionary<string, TranslationStatusEnum>> TranslationStatus { get; } = new();
    public HashSet<string> Warnings { get; } = new();

    public IDictionary<string, TranslationStatusEnum> GetStatusForLocale(string locale)
    {
        Dictionary<string, TranslationStatusEnum> result = new();

        foreach ((string relativePath, IDictionary<string, TranslationStatusEnum> statuses) in this.TranslationStatus)
        {
            result[relativePath] = statuses.TryGetValue(locale, out TranslationStatusEnum status)
                ? status
                : TranslationStatusEnum.Missing;
        }

        return result;
    }
}

public record ModLanguage(string Name, bool Required = true, string Url = null);

public record Manifest(string UniqueId, string Name);
