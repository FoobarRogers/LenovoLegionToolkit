using System;
using System.Globalization;
using System.IO;

namespace LenovoLegionToolkit.WPF.Utils;

internal static class NotificationOpacityPatchSettings
{
    private const double DefaultOpacity = 1.0;
    private const double MinimumOpacity = 0.25;
    private const double MaximumOpacity = 1.0;

    private static string SettingsPath =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "LenovoLegionToolkit",
            "notification_opacity_patch.txt");

    public static double Load()
    {
        try
        {
            if (!File.Exists(SettingsPath))
                return DefaultOpacity;

            var text = File.ReadAllText(SettingsPath).Trim();
            if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
                return Math.Clamp(value, MinimumOpacity, MaximumOpacity);
        }
        catch
        {
        }

        return DefaultOpacity;
    }

    public static void Save(double value)
    {
        try
        {
            value = Math.Clamp(value, MinimumOpacity, MaximumOpacity);
            var directory = Path.GetDirectoryName(SettingsPath);
            if (!string.IsNullOrWhiteSpace(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(SettingsPath, value.ToString("0.00", CultureInfo.InvariantCulture));
        }
        catch
        {
        }
    }
}
