using System.Reflection;

namespace Seventy.Common;
public static class ResourceAccessor
{
    public enum ResourceName
    {
        icons8_about,
        icons8_cancel,
        icons8_checkmark,
        icons8_edit,
        icons8_female_user,
        icons8_file,
        icons8_folder,
        icons8_home,
        icons8_info,
        icons8_menu,
        icons8_plus,
        icons8_settings,
        icons8_trash,
        icons8_unavailable,
        icons8_user_male
    }

    public static string GetSvg(this ResourceName resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Resources.{resourceName}.svg") ?? throw new ArgumentException("Resource not found.", nameof(resourceName));
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
