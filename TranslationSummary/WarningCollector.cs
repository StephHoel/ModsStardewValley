namespace TranslationSummary;

public class WarningCollector
{
    public static IEnumerable<(string ModName, string RelativePath, HashSet<string> Warnings)> Collect(ModFolder[] modFolders)
    {
        return modFolders
            .Where(folder => folder.Warnings.Any())
            .OrderBy(folder => folder.RelativePath, StringComparer.Ordinal)
            .Select(folder => (folder.ModName, folder.RelativePath, folder.Warnings))
            .ToArray();
    }
}
