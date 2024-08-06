using System.Linq;
using System.Collections.Generic;


namespace Specter.Terminal.UI.Components.Property;


public static class ComponentPropertyExtensions
{
    public static T[] PropertiesOfType<T>(this IEnumerable<ComponentProperty> properties) where T : class
        => (from property in properties
                let convertedProperty = property as T
                where convertedProperty is not null
                select convertedProperty).ToArray();
}
