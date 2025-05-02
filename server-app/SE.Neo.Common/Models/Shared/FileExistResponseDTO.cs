namespace SE.Neo.Common.Models.Shared
{
    public class FileExistResponseDTO
    {
        public bool IsExist { get; set; }
        public string BlobName { get; set; }
        public string ActualFileName { get; set; }
        public string ActualFileTitle { get; set; }
        public int FileVersion { get; set; }
        public bool? IsOwner { get; set; }
    }
}
