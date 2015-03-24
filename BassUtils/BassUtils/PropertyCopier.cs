using System;
using System.Linq;
using System.Reflection;

namespace BassUtils
{
    /// <summary>
    /// Copies properties from one object to another.
    /// </summary>
    public static class PropertyCopier
    {
        /// <summary>
        /// Copies all possible property values from <paramref name="source"/> to <paramref name="destination"/>.
        /// Properties are matched by name, and the property on the destination must have a setter and
        /// a type that is compatible with the source.
        /// </summary>
        /// <param name="source">Object to copy properties from.</param>
        /// <param name="destination">Object to copy properties to.</param>
        public static void CopyProperties(object source, object destination)
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

            // Se the properties in the destination.
            foreach (var property in properties)
                property.targetProperty.SetValue(destination, property.sourceProperty.GetValue(source, null), null);
        }
    }
}
