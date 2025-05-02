namespace SE.Neo.Common.Attributes
{
    /// <summary>
    /// Defines an attribute, which is used to compare values of model properlies 
    /// for two object to detect which properties were changed.
    /// It may take 'name' argument, which is used to define display name for a property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class PropertyComparationAttribute : Attribute
    {
        public PropertyComparationAttribute(string? name = null)
        {
            this.Name = name;
        }

        public string? Name { get; private set; }
    }
}
