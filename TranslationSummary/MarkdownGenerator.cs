using System.Data;
using System.Text;
using TranslationSummary.Enums;

namespace TranslationSummary;

public class MarkdownGenerator
{
    public static string GenerateSummary(ModFolder[] modFolders)
    {
        var languages = Constants.Languages;

        var style = TableStyleEnum.Auto;
        bool linkMissingToFolder = true;
        bool linkToFiles = true;

        var locales = (
                from locale in GetKnownLocales(modFolders, languages)
                let metadata = languages.TryGetValue(locale, out var language) ? language : null
                orderby (metadata?.Name ?? locale).ToLowerInvariant(), locale
                select (Locale: locale, Metadata: metadata)
            ).ToArray();

        var str = new StringBuilder();

        str.AppendLine("## Translating the mods");
        str.AppendLine();

        str.AppendLine("The mods can be translated into any language supported by the game, and SMAPI will automatically use the right translations.");
        str.AppendLine();

        str.AppendLine("Contributions are welcome! See [Modding:Translations](https://stardewvalleywiki.com/Modding:Translations) on the wiki for help contributing translations.");
        str.AppendLine();

        str.AppendLine("(❑ = untranslated, ↻ = partly translated, ✓ = fully translated)");
        str.AppendLine();

        if (style == TableStyleEnum.Auto)
            style = locales.Length > modFolders.Length ? TableStyleEnum.RowPerLocale : TableStyleEnum.RowPerMod;

        DataTable table = new();
        table.Columns.Add("&nbsp;");

        if (style == TableStyleEnum.RowPerLocale)
        {
            foreach (var mod in modFolders)
                table.Columns.Add(mod.ModName);

            foreach (var (locale, row) in from locale in locales let row = new string[modFolders.Length + 1] select (locale, row))
            {
                row[0] = locale.Metadata?.Url != null ? $"[{locale.Metadata.Name}]" : (locale.Metadata?.Name ?? locale.Locale);
                int i = 1;

                foreach (var mod in modFolders)
                    row[i++] = RenderStatusCell(mod, locale.Locale, linkToFiles, linkMissingToFolder);

                table.Rows.Add(row);
            }
        }
        else
        {
            foreach (var (locale, metadata) in locales)
                table.Columns.Add(RenderLocaleCell(locale, metadata));

            foreach (var mod in modFolders)
            {
                string[] row = new string[locales.Length + 1];
                int i = 0;
                row[i++] = mod.ModName;

                foreach (var (locale, _) in locales)
                    row[i++] = RenderStatusCell(mod, locale, linkToFiles, linkMissingToFolder);

                table.Rows.Add(row);
            }
        }

        str.AppendLine(ToMarkdownTable(table));

        var links = locales.Select(pair => pair.Metadata).Where(metadata => metadata?.Url != null).ToArray();
        foreach (var link in links)
            str.AppendLine($"[{link!.Name}]: {link.Url}");

        return str.ToString();
    }

    public static string GenerateWarnings(IEnumerable<(string ModName, string RelativePath, HashSet<string> Warnings)> warnings)
    {
        var sb = new StringBuilder();
        sb.AppendLine("\n## Warnings\n");

        foreach (var (modName, relativePath, modWarnings) in warnings.OrderBy(item => item.RelativePath, StringComparer.Ordinal))
        {
            sb.AppendLine($"**Mod:** {modName} ({relativePath})");

            foreach (var warning in modWarnings.OrderBy(item => item, StringComparer.Ordinal))
                sb.AppendLine($"- {warning}");

            sb.AppendLine();
        }

        return sb.ToString();
    }

    private static string RenderLocaleCell(string locale, ModLanguage? data)
    {
        return data?.Url != null ? $"[{data.Name}]" : (data?.Name ?? locale);
    }

    private static string RenderStatusCell(ModFolder modFolder, string locale, bool linkToFiles, bool linkMissingToFolder)
    {
        if (!linkToFiles)
        {
            var statuses = modFolder.GetStatusForLocale(locale).Values.Distinct().ToArray();
            return statuses switch
            {
                var arr when arr.All(status => status == TranslationStatusEnum.Complete) => "✓",
                var arr when arr.All(status => status == TranslationStatusEnum.Missing) => "❑",
                _ => "↻"
            };
        }

        return string.Join("<br />",
            modFolder.GetStatusForLocale(locale).OrderBy(pair => pair.Key, StringComparer.Ordinal).Select(pair =>
            {
                string relativePath = pair.Key;
                TranslationStatusEnum status = pair.Value;
                string symbol = status switch
                {
                    TranslationStatusEnum.Missing => "❑",
                    TranslationStatusEnum.Complete => "✓",
                    _ => "↻"
                };

                if (status == TranslationStatusEnum.Missing && !linkMissingToFolder)
                    return symbol;

                string url = Path.Combine(modFolder.RelativePath, relativePath);
                if (status != TranslationStatusEnum.Missing)
                    url = Path.Combine(url, $"{locale}.json");

                url = url.Replace("\\", "/").Replace(" ", "%20").Replace("[", "%5B").Replace("]", "%5D");
                return $"[{symbol}]({url})";
            })
        );
    }

    private static HashSet<string> GetKnownLocales(ModFolder[] folders, Dictionary<string, ModLanguage> languages)
    {
        HashSet<string> knownLocales = new(languages.Where(pair => pair.Value.Required).Select(pair => pair.Key));
        var keys = folders.SelectMany(folder => folder.TranslationStatus.Values.SelectMany(statuses => statuses.Keys));
        foreach (var locale in keys)
            knownLocales.Add(locale);
        return knownLocales;
    }

    private static string ToMarkdownTable(DataTable source)
    {
        var columns = source.Columns.Cast<DataColumn>().ToArray();
        var rows = source.Rows.Cast<DataRow>().ToArray();
        int[] colWidths = new int[columns.Length];

        for (int i = 0; i < columns.Length; i++)
            colWidths[i] = columns[i].ColumnName?.Length ?? 0;

        foreach (DataRow row in rows)
        {
            for (int i = 0; i < columns.Length; i++)
                colWidths[i] = Math.Max(colWidths[i], row.Field<object>(i)?.ToString()!.Length ?? 0);
        }

        var str = new StringBuilder();
        str.AppendLine(string.Join(" | ", columns.Select((col, i) => $"{col.ColumnName}".PadRight(colWidths[i]))).TrimEnd());
        str.AppendLine(string.Join(" | ", columns.Select((col, i) => ":".PadRight(colWidths[i], '-'))));

        foreach (var row in rows)
            str.AppendLine(string.Join(" | ", row.ItemArray.Select((field, i) => $"{field}".PadRight(colWidths[i]))).TrimEnd());

        return str.ToString();
    }
}
