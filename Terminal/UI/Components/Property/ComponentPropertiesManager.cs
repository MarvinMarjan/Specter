using System;
using System.Collections.Generic;

using Specter.Terminal.UI.Application.Exceptions;


namespace Specter.Terminal.UI.Components.Property;


/// <summary>
/// Manages component properties.
/// </summary>
/// <param name="owner"> The component that the manager will be a child of </param>
public class ComponentPropertiesManager(Component owner)
{
    public Component Owner { get; set; } = owner;

    private readonly List<ComponentProperty> _properties = [];



    /// <summary>
    /// Adds a new ComponentProperty to the manager.
    /// </summary>
    /// <param name="property"> The property to be added. </param>
    /// <exception cref="ComponentPropertyException"> Exception thrown if the property has already been added to the manager. </exception>
    public void Add(ComponentProperty property)
    {
        if (_properties.Contains(property))
            throw new ComponentPropertyException(
                property.GetData(),
                "Can't add a new property because it has already been added."
            );

        _properties.Add(property);
    }



    public ComponentProperty GetProperty(string propertyName)
    {
        foreach (ComponentProperty property in _properties)
            if (property.Name == propertyName)
                return property;

        throw new ComponentPropertyException(
            new(propertyName, null, null),
            "Property couldn't be found."
        );
    }

    public bool TryGetProperty(string propertyName, out ComponentProperty? property)
    {
        try
        {
            property = GetProperty(propertyName);
            return true;
        }
        catch
        {
            property = null;
            return false;
        }
    }


    public T GetPropertyAs<T>(string propertyName) where T : class
    {
        ComponentProperty property = GetProperty(propertyName);

        if (property is not T convertedProperty)
            throw new ComponentPropertyException(property.GetData(), $@"Could not convert property to type ""{typeof(T).Name}"".");
    
        return convertedProperty;
    }

    public bool TryGetPropertyAs<T>(string propertyName, out T? property) where T : class
    {
        try
        {
            property = GetPropertyAs<T>(propertyName);
            return true;
        }
        catch
        {
            property = null;
            return false;
        }
    }


    public ComponentProperty[] GetAllProperties()
        => [.. _properties];


    public T[] GetAllPropertiesOfType<T>() where T : class
        => GetAllProperties().PropertiesOfType<T>();



    public void ForeachProperty(Action<ComponentProperty> action)
    {
        foreach (ComponentProperty property in _properties)
            action(property);
    }

    public void ForeachPropertyOfType<T>(Action<T> action) where T : class
    {
        foreach (T property in GetAllPropertiesOfType<T>())
            action(property);
    }


    public void SetPropertiesInherit(bool value)
        => ForeachPropertyOfType<IInheritable>(property => property.Inherit = value);

    public void SetPropertiesCanBeInherited(bool value)
        => ForeachPropertyOfType<IInheritable>(property => property.CanBeInherited = value);
}
