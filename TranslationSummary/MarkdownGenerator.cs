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
                orderby (metadata?.Name ?? locale).ToLower()
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
            foreach (var (Locale, Metadata) in locales)
                table.Columns.Add(RenderLocaleCell(Locale, Metadata));

            foreach (var mod in modFolders)
            {
                string[] row = new string[locales.Length + 1];

                int i = 0;

                row[i++] = mod.ModName;

                foreach (var (Locale, Metadata) in locales)
                    row[i++] = RenderStatusCell(mod, Locale, linkToFiles, linkMissingToFolder);

                table.Rows.Add(row);
            }
        }

        str.AppendLine(ToMarkdownTable(table));

        var links = locales.Select(p => p.Metadata).Where(p => p?.Url != null).ToArray();

        foreach (var link in links)
            str.AppendLine($"[{link.Name}]: {link.Url}");

        return str.ToString();
    }

    public static string GenerateWarnings(IEnumerable<(string ModName, string RelativePath, HashSet<string> Warnings)> warnings)
    {
        var sb = new StringBuilder();

        sb.AppendLine("\n## Warnings\n");

        foreach (var (ModName, RelativePath, Warnings) in warnings)
        {
            sb.AppendLine($"**Mod:** {ModName} ({RelativePath})");

            foreach (var warn in Warnings)
                sb.AppendLine($"- {warn}");

            sb.AppendLine();
        }

        return sb.ToString();
    }

    private static string RenderLocaleCell(string locale, ModLanguage data)
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
                var arr when arr.All(p => p == TranslationStatusEnum.Complete) => "✓",
                var arr when arr.All(p => p == TranslationStatusEnum.Missing) => "❑",
                _ => "↻"
            };
        }

        return string.Join("<br />",
            modFolder.GetStatusForLocale(locale).Select(p =>
            {
                string relativePath = p.Key;

                TranslationStatusEnum status = p.Value;

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
        HashSet<string> knownLocales = new(languages.Where(p => p.Value.Required).Select(p => p.Key));

        var keys = folders.SelectMany(f => f.TranslationStatus.Values.SelectMany(v => v.Keys));

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