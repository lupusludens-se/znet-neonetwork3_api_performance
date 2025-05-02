namespace SE.Neo.Common.Attributes
{
    public class CustomException : Exception
    {
        public CustomException(string message, Exception? innerException = null) : base(message, innerException)
        {

        }
    }
}
