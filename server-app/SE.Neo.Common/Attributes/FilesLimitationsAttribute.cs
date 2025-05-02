namespace SE.Neo.Common.Attributes
{
    public class FilesLimitationsAttribute : Attribute
    {
        public string[] FileTypesKeys { get; }

        public FilesLimitationsAttribute(params string[] fileTypesKeys)
        {
            FileTypesKeys = fileTypesKeys;
        }
    }
}
