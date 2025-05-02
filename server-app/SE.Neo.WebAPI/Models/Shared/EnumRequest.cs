namespace SE.Neo.WebAPI.Models.Shared
{
    public class EnumRequest<T> where T : Enum
    {
        public T Id;
    }
}
