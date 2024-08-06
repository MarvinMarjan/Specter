using System;

using Specter.Terminal.UI.Application;


namespace Specter.Terminal.UI.Components.Property;


public readonly struct ComponentPropertyData(string name, string? typeName, Component? owner)
{
    public string Name { get; init; } = name;
    public string? TypeName { get; init; } = typeName;
    public Component? Owner { get; init; } = owner;
}


// TODO: change whole project to use .NET event conventions.


/// <summary>
/// The base class of every other ComponentProperty.
/// </summary>
public abstract class ComponentProperty
{
    /// <summary>
    /// The Component that owns and manage this property.
    /// </summary>
    public Component Owner { get; set; }
    public ComponentPropertiesManager Manager => Owner.PropertiesManager;


    /// <summary>
    /// Name of the property.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// The value of the property as raw Object.
    /// </summary>
    public abstract object ValueObject { get; }


    public bool RequestRenderOnValueChange { get; set; } = false;
    public bool DrawAllRequest { get; set; } = false;


    public event EventHandler? ValueChanged;


    protected virtual void OnValueChanged()
        => ValueChanged?.Invoke(this, EventArgs.Empty);



    public ComponentProperty(Component owner, string name)
    {
        Owner = owner;
        Manager.Add(this);

        Name = name;
    }



    public ComponentPropertyData GetData()
        => new(Name, GetType().Name, Owner);
}


/// <summary>
/// A generic ComponentProperty with extended behaviour. 
/// </summary>
/// <typeparam name="T"> The property value type. </typeparam>
public class ComponentProperty<T>(Component owner, string name, T value)
    : ComponentProperty(owner, name)
        where T : notnull
{
    public override object ValueObject => Value;


    protected T value = value;

    /// <summary>
    /// The typed value of this property.
    /// </summary>
    public T Value
    {
        get => value;
        set
        {
            if (this.value.Equals(value))
                return;

            this.value = value;
            OnValueChanged();
        }
    }


    protected override void OnValueChanged()
    {
        base.OnValueChanged();

        if (!RequestRenderOnValueChange)
            return;

        if (DrawAllRequest)
            App.CurrentApp.RequestDrawAll();
        else
            Owner.AddThisToRenderQueue();
    }


    public static implicit operator T(ComponentProperty<T> property) => property.Value;
}



/// <summary>
/// Provides behaviour for inheriting a value from a parent.
/// </summary>
public interface IInheritable : IUpdateable
{
    public IInheritable? Parent { get; set; }

    public bool Inherit { get; set; }
    public bool CanBeInherited { get; set; }

    public void InheritValue();
}


/// <summary>
/// ComponentProperty that can inherit values from its parent.
/// </summary>
/// <typeparam name="T"> The property value type. </typeparam>
public class InheritableComponentProperty<T> : ComponentProperty<T>, IInheritable
    where T : notnull
{
    public IInheritable? Parent { get; set; }

    public InheritableComponentProperty<T>? ParentAsProperty => Parent as InheritableComponentProperty<T>;


    public bool Inherit { get; set; } = true;
    public bool CanBeInherited { get; set; } = true;


    public InheritableComponentProperty(

        Component owner,
        string name,
        T value,
        InheritableComponentProperty<T>? parent

    ) : base(
        owner, name, value
    )
    {
        Parent = parent;

        if (ParentAsProperty is null)
            return;

        if (CanInherit())
            this.value = ParentAsProperty.Value;

        ParentAsProperty.ValueChanged += delegate { TryInheritValue(); };
    }


    public bool CanInherit()
        => Inherit && ParentAsProperty is not null && ParentAsProperty.CanBeInherited;


    /// <summary>
    /// Forces the value inheriting
    /// </summary>
    public void InheritValue()
    {
        if (ParentAsProperty is not null && !ParentAsProperty.Value.Equals(Value))
            Value = ParentAsProperty.Value;
    }


    public bool TryInheritValue()
    {
        if (CanInherit())
        {
            InheritValue();
            return true;
        }

        return false;
    }


    public static implicit operator T(InheritableComponentProperty<T> property) => property.Value;


    public virtual void Update() => TryInheritValue();
}
