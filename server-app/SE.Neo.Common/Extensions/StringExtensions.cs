using System.Text;

namespace SE.Neo.Common.Extensions
{
    public static class StringExtensions
    {
        public static T ToEnum<T>(this string input) where T : struct
        {
            return Enum.Parse<T>(input, true);
        }

        public static string ToLower(this string input, int startIndex, int length)
        {
            var builder = new StringBuilder();
            switch (input)
            {
                case null:
                    throw new ArgumentNullException(nameof(input));
                case "":
                    return string.Empty;
                default:
                    builder.Append(input.Substring(0, startIndex));
                    builder.Append(input.Substring(startIndex, length).ToLowerInvariant());
                    break;
            }

            builder.Append(input.Substring(startIndex + length, input.Length - (startIndex + length)));

            return builder.ToString();
        }
    }
}
