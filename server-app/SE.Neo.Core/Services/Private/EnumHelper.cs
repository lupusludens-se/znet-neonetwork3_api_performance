public static class EnumHelper
{
    public static int GetEnumValue<T>(string str) where T : struct, IConvertible
    {
        Type enumType = typeof(T);
        object value = Enum.Parse(enumType, str);
        return Convert.ToInt32(value);
    }

    public static T GetEnumValue<T>(int intValue) where T : struct, IConvertible
    {
        Type enumType = typeof(T);
        if (!enumType.IsEnum)
        {
            throw new Exception("T must be an Enumeration type.");
        }

        return (T)Enum.ToObject(enumType, intValue);
    }
}

