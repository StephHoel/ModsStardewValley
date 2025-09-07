using StardewModdingAPI;

namespace Utils;

public static class FileUtils
{
    public static void RemoveObsoleteFiles(IModHelper helper, string[] files, IMonitor monitor)
    {
        foreach (var file in files)
        {
            string fullPath = Path.Combine(helper.DirectoryPath, file);

            if (File.Exists(fullPath))
            {
                try
                {
                    File.Delete(fullPath);
                    monitor.Log($"Removed obsolete file '{file}'.", LogLevel.Debug);
                }
                catch (Exception ex)
                {
                    monitor.Log($"Failed deleting obsolete file '{file}':\n{ex}", LogLevel.Debug);
                }
            }
        }
    }
}