namespace COINNP.Client.Mapping;

internal static class COINTypeNameVersionHelper
{
    public const int DefaultVersion = 3;

    /// <summary>
    /// Appends a version to the typename if none is present, so a "cancel" becomes "cancel-v3", but a "cancel-v3" is unchanged.
    /// </summary>
    /// <param name="typeName">The typename to append the version to, if none present.</param>
    /// <param name="version">The version to add. Use a negative version to prevent a version being added.</param>
    /// <returns>
    /// Returns a typename with added version (unless version is less than 0).
    /// </returns>
    public static string AppendVersion(string typeName, int version)
        => version < 0 || typeName.EndsWith($"-v{version}")
            ? typeName
            : $"{typeName}-v{version}";
}
