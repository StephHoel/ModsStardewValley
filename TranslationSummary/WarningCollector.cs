namespace TranslationSummary;

public class WarningCollector
{
    public static IEnumerable<(string ModName, string RelativePath, HashSet<string> Warnings)> Collect(ModFolder[] modFolders)
    {
        return modFolders
            .Where(p => p.Warnings.Any())
            .Select(p => (p.ModName, p.RelativePath, p.Warnings))
            .ToArray();
    }
}