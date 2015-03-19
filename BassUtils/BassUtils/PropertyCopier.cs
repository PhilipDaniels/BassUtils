using System;
using System.Linq;
using System.Reflection;

namespace BassUtils
{
    public static class PropertyCopier
    {
        /// <summary>
        /// Copies all possible property values from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">Object to copy properties from.</param>
        /// <param name="destination">Object to copy properties to.</param>
        public static void CopyProperties(this object source, object destination)
        {
            source.ThrowIfNull("source");
            destination.ThrowIfNull("destination");

            // Getting the Types of the objects
            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();

            // Collect all the valid properties to map
            var properties = from srcProp in typeSrc.GetProperties()
                             let targetProperty = typeDest.GetProperty(srcProp.Name)
                             where srcProp.CanRead &&
                                   targetProperty != null &&
                                   (targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) == 0
                                   && targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType)
                             select new { sourceProperty = srcProp, targetProperty = targetProperty };

            // Map the properties.
            foreach (var property in properties)
            {
                property.targetProperty.SetValue(destination, property.sourceProperty.GetValue(source, null), null);
            }
        }
    }
}
