using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TranslationSummary.Enums;

namespace TranslationSummary;

public class ModScanner
{
    public static ModFolder[] GetModFolders(string solutionFolder, Dictionary<string, string> renameMods)
    {
        var i18nDirs = new DirectoryInfo(solutionFolder).GetFiles("default.json", SearchOption.AllDirectories)
            .Select(p => p.Directory)
            .Where(dir => dir!.Name! == "i18n")
            .OrderByDescending(dir => dir!.FullName!)
            .ToArray();

        var folders = new List<ModFolder>();

        ModFolder folder = null!;

        foreach (var dir in i18nDirs)
        {
            var modDir = dir!.Parent!;

            string relativePath = Path.GetRelativePath(solutionFolder, modDir.FullName);

            if (folder == null || !relativePath.StartsWith(folder.RelativePath + Path.DirectorySeparatorChar))
            {
                if (folder != null)
                {
                    folders.Add(folder);
                    folder = null!;
                }

                FileInfo manifestFile = modDir.GetFiles("manifest.json").FirstOrDefault()!;

                if (manifestFile == null)
                    continue;

                Manifest manifest = JsonConvert.DeserializeObject<Manifest>(File.ReadAllText(manifestFile.FullName))!;

                string modName = manifest.UniqueId != null
                    ? renameMods.GetValueOrDefault(manifest.UniqueId) ?? manifest.Name
                    : manifest.Name;

                folder = new ModFolder(modDir, relativePath, modName);
            }

            folder.TranslationDirs.Add(dir);
        }

        if (folder != null)
            folders.Add(folder);

        return folders
            .Where(p => !ShouldIgnore(p.RelativePath))
            .OrderBy(p => p.ModName.ToLower())
            .ToArray();
    }

    public static void PopulateTranslationStatus(ModFolder[] modFolders, Dictionary<string, ModLanguage> languages)
    {
        foreach (var folder in modFolders)
            PopulateTranslationStatusEnum(folder);

        var knownLocales = GetKnownLocales(modFolders, languages);
        foreach (var folder in modFolders)
        {
            PopulateTranslationStatusEnum(folder);
            foreach (var entry in folder.TranslationStatus.Values)
            {
                foreach (var locale in knownLocales)
                    entry.TryAdd(locale, TranslationStatusEnum.Missing);
            }
        }
    }

    private static void PopulateTranslationStatusEnum(ModFolder modFolder)
    {
        foreach (DirectoryInfo dir in modFolder.TranslationDirs)
        {
            string dirRelativePath = Path.GetRelativePath(modFolder.ModDir.FullName, dir.FullName);
            var statuses = modFolder.TranslationStatus[dirRelativePath] = new();
            HashSet<string> defaultKeys = new(StringComparer.OrdinalIgnoreCase);
            var defaultEntry = dir.GetFiles("default.json").First();
            string content = File.ReadAllText(defaultEntry.FullName);
            foreach (var key in JsonConvert.DeserializeObject<Dictionary<string, string>>(content)!.Keys)
                defaultKeys.Add(key);
            foreach (FileInfo file in dir.GetFiles())
            {
                string warnPrefix = $"[{Path.GetRelativePath(modFolder.ModDir.FullName, file.FullName)}]";
                if (file.Name == "default.json") continue;
                if (file.Extension != ".json") continue;
                string locale = Path.GetFileNameWithoutExtension(file.Name);
                string fileContent = File.ReadAllText(file.FullName);
                HashSet<string> keys;
                try
                {
                    keys = new HashSet<string>(JsonConvert.DeserializeObject<Dictionary<string, string>>(fileContent)!.Keys, StringComparer.OrdinalIgnoreCase);
                }
                catch { continue; }
                TranslationStatusEnum status = TranslationStatusEnum.Complete;
                if (Regex.IsMatch(fileContent, @"//[ \t]*TODO\b", RegexOptions.IgnoreCase) || defaultKeys.Any(p => !keys.Contains(p)))
                    status = TranslationStatusEnum.Incomplete;
                statuses[locale] = status;
            }
        }
    }

    private static HashSet<string> GetKnownLocales(ModFolder[] folders, Dictionary<string, ModLanguage> languages)
    {
        HashSet<string> knownLocales = new(languages.Where(p => p.Value.Required).Select(p => p.Key));
        var keys = folders.SelectMany(f => f.TranslationStatus.Values.SelectMany(v => v.Keys));
        foreach (var locale in keys)
            knownLocales.Add(locale);
        return knownLocales;
    }

    private static bool ShouldIgnore(string relativePath)
    {
        if (Constants.IgnoreRelativePaths.Any(ignorePath => relativePath == ignorePath
                                                  || relativePath.StartsWith(ignorePath + Path.DirectorySeparatorChar)))
            return true;

        if (Constants.IgnorePathSubstrings.Any(ignoreSubstr => relativePath.Contains(ignoreSubstr)))
            return true;

        return false;
    }
}