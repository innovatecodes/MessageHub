namespace Common.Options
{
    public sealed class BodyRenderOptions
    {
        public string From { get; init; } = string.Empty;

        public ApplicationInfo ApplicationInfo { get; init; } = new ApplicationInfo();
    }
}
