using SE.Neo.Common.Attributes;
using System.Collections;
using System.Reflection;

namespace SE.Neo.Common.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Compare two versions of an object and define a list of changed properties
        /// </summary>
        /// <typeparam name="T">type of an object to compare</typeparam>
        /// <param name="oldObject">old version of an object</param>
        /// <param name="newObject">new version of an object</param>
        /// <param name="compareEnumerableProperty">a function that defines a strategy for comparing inner collections of an object and returns true if the collections are equal and false otherwise</param>
        /// <returns>true if the versions are equal and false otherwise</returns>
        public static List<string>? FindChangedProperties<T>(this T oldObject,
            T newObject,
            Func<IEnumerable<dynamic>, IEnumerable<dynamic>, Type, bool>? compareEnumerableProperty = null,
            Type parentObjectType = null
            ) where T : class
        {
            var changedFields = new List<string>();
            foreach (PropertyInfo property in oldObject.GetType().GetProperties())
            {
                var propertyComparationAttribute = property.GetCustomAttribute<PropertyComparationAttribute>();
                if (parentObjectType == property.PropertyType || propertyComparationAttribute == null)
                {
                    // skip cicle loops and not needed properties
                    continue;
                }

                Type propertyType = property.PropertyType;
                object? oldValue = property.GetValue(oldObject);
                object? newValue = property.GetValue(newObject);
                if (oldValue == null && newValue == null)
                {
                    continue;
                }
                else if (oldValue == null || newValue == null)
                {
                    changedFields.Add(propertyComparationAttribute.Name ?? property.Name);
                    continue;
                }

                if (propertyType.GetInterface(nameof(IEnumerable)) != null && propertyType != typeof(string))
                {
                    var oldCollection = (IEnumerable<dynamic>)oldValue!;
                    var newCollection = (IEnumerable<dynamic>)newValue!;
                    if (compareEnumerableProperty == null || (oldCollection.Count() == 0 && newCollection.Count() == 0))
                    {
                        continue;
                    }

                    if (!compareEnumerableProperty.Invoke(oldCollection, newCollection, propertyType.GetGenericArguments().First()))
                    {
                        changedFields.Add(propertyComparationAttribute.Name ?? property.Name);
                    }
                }
                else
                {
                    if (propertyType.IsClass && propertyType != typeof(string))
                    {
                        changedFields.AddRange(oldValue.FindChangedProperties(newValue, parentObjectType: oldObject.GetType()));
                    }
                    else
                    {
                        if (!oldValue.Equals(newValue))
                        {
                            changedFields.Add(propertyComparationAttribute.Name ?? property.Name);
                        }
                    }
                }
            }

            return changedFields;
        }
    }
}
