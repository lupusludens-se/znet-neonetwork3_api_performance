using Microsoft.Extensions.Options;
using SE.Neo.Common.Attributes;
using SE.Neo.Core.Enums;
using System.Reflection;

namespace SE.Neo.Infrastructure.Configs
{
    public class BlobTypesFilesLimitationsConfig : Dictionary<BlobType, IEnumerable<FileTypeLimitation>>
    {
        public BlobTypesFilesLimitationsConfig(IOptions<GeneralFilesLimitationsConfig> generalFilesLimitationsConfigOptions, IOptions<AttachmentLimitationsConfig> attachmentLimitationsConfigOptions)
        {
            GeneralFilesLimitationsConfig generalFilesLimitationsConfig = generalFilesLimitationsConfigOptions.Value;
            AttachmentLimitationsConfig attachmentLimitationsConfig = attachmentLimitationsConfigOptions.Value;

            IEnumerable<BlobType> blobTypes = Enum.GetValues(typeof(BlobType)).Cast<BlobType>();
            foreach (BlobType blobType in blobTypes)
            {
                MemberInfo memberInfo = blobType.GetType().GetMember(blobType.ToString()).First();
                var filesLimitationsAttribute = (FilesLimitationsAttribute?)memberInfo.GetCustomAttributes(typeof(FilesLimitationsAttribute), false).FirstOrDefault();

                if (filesLimitationsAttribute == null)
                    this[blobType] = generalFilesLimitationsConfig.Values.AsEnumerable();
                else if (filesLimitationsAttribute != null && blobType == BlobType.Initiative || filesLimitationsAttribute != null && blobType == BlobType.Companies)
                    this[blobType] = filesLimitationsAttribute.FileTypesKeys.Select(ftk => attachmentLimitationsConfig[ftk]);
                else
                    this[blobType] = filesLimitationsAttribute.FileTypesKeys.Select(ftk => generalFilesLimitationsConfig[ftk]);
            }
        }
    }
}
