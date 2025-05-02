using FileTypeChecker.Abstracts;

namespace SE.Neo.Infrastructure.Models.FileTypes
{
    public class SvgFileType : FileType
    {
        private static readonly string _name = "Scalable Vector Graphics";
        private static readonly string _extension = "svg";
        private static readonly byte[][] _magicBytes =         {
            new byte[] { 0x3C, 0x73, 0x76, 0x67, 0x20 },
        };

        public SvgFileType() : base(_name, _extension, _magicBytes.Concat(XmlFileType.MagicNumbers).ToArray()) { }
    }
}
