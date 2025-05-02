using SE.Neo.Common.Attributes;

namespace SE.Neo.Core.Enums
{
    public enum BlobType : int
    {
        [FilesLimitations("Image")]
        Users = 1,

        [FilesLimitations("Image")]
        Conversations = 2,

        [FilesLimitations("Image", "Document")]
        Companies = 3,

        [FilesLimitations("Image")]
        Forums = 4,

        [FilesLimitations("Image", "Icon")]
        Tools = 5,

        [FilesLimitations("Image")]
        Announcement = 6,

        [FilesLimitations("Image", "Document")]
        Initiative = 7
    }
}
