﻿using System;
using System.Collections.Generic;
using System.Text;

using Specter.Color;
using Specter.Terminal.UI.Components.Property;
using Specter.Terminal.UI.Application;
using Specter.Terminal.UI.Application.Exceptions;


namespace Specter.Terminal.UI.Components;


public interface IUpdateable
{
    public void Update();
}

public interface IDrawable
{
    public string Draw();
}


/// <summary>
/// Classes that inherit from this can't be parent of any component.
/// </summary>
public interface IChildLess { }



/// <summary>
/// Base class of all UI components.
/// </summary>
public abstract class Component : IUpdateable, IDrawable
{

    // Properties
    public string Name { get; set; }

    public Component? Parent { get; set; }
    public List<Component> Childs { get; private set; }

    public ComponentPropertiesManager PropertiesManager { get; protected set; }

    public Bounds Bounds => Bounds.FromRectangle(Position, Size);


    /// <summary>
    /// The position of this Component relative to its parent, if it has one.
    /// </summary>
    public Point RelativePosition => Parent is not null ? Parent.RelativePosition + Position : Position;


    /// <summary>
    /// The Rect object of this Component.
    /// </summary>
    public Rect Rect => new(Position, Size);


    // Component properties

    public ComponentProperty<Point> Position { get; }
    public ComponentProperty<Size> Size { get; }
    public InheritableComponentProperty<Alignment> Alignment { get; }
    public InheritableComponentProperty<ColorObject> Color { get; }



    public delegate void UpdateEventHandler();

    public event UpdateEventHandler? UpdateEvent;



    public Component(string name, Component? parent, Point position, Size size)
    {
        Name = name;

        Parent = parent;
        ChildLessParentCheck(); // checks if Parent is a IChildLess

        Childs = [];
        PropertiesManager = new(this);


        Position = new(this, "Position", position)
        {
            RequestRenderOnValueChange = true,
            DrawAllRequest = true
        };

        Size = new(this, "Size", size)
        {
            RequestRenderOnValueChange = true,
            DrawAllRequest = true
        };

        Alignment = new(this, "Alignment", UI.Alignment.None,Parent?.Alignment)
        {
            RequestRenderOnValueChange = true,
            DrawAllRequest = true
        };

        Color = new(this, "Color", ColorObject.None, Parent?.Color)
        {
            RequestRenderOnValueChange = true
        };


        if (Parent is null)
            return;

        // add this component as child of parent
        if (!Parent.Childs.Contains(this))
            Parent.Childs.Add(this);
    }


    /// <summary>
    /// Checks recursively if this Component is child of another one.
    /// </summary>
    /// <param name="component"> The Component to check. </param>
    /// <returns> True if this Component is child of the specified one, false otherwise. </returns>
    public bool IsChildOf(Component component)
    {
        if (component.Childs.Count == 0)
            return false;

        bool isChild = false;

        ForeachChildIn(component, child =>
        {
            if (child == this)
                isChild = true;
        });

        return isChild;
    }


    public static void ForeachChildIn(Component component, Action<Component> action, bool ignoreParent = true)
    {
        if (!ignoreParent)
            action(component);

        foreach (Component child in component.Childs)
            ForeachChildIn(child, action, false);
    }



    private void ChildLessParentCheck()
    {
        if (Parent is IChildLess)
            throw new ComponentException(Name, @"Can't have a ""IChildLess"" as parent.");
    }



    /// <typeparam name="T"> The Component derived type to convert. </typeparam>
    /// <returns> This Component converted to the specified Component derived type. </returns>
    public T? As<T>() where T : Component => this as T;



    /// <summary>
    /// Adds this Component to the App render queue (App.RenderQueue). If one of the parents of this Component
    /// is in the queue, then this Component is not added, since drawing the parent also draws its childs.
    /// </summary>
    public void AddThisToRenderQueue()
    {
        // * do not add if there is already a parent in the queue, since
        // * drawing the parent also draws the child.

        foreach (Component component in App.CurrentApp.RenderQueue)
            if (IsChildOf(component))
                return;

        App.CurrentApp.AddComponentToRenderQueue(this);
    }



    public virtual string Draw()
    {
        StringBuilder builder = new();

        foreach (Component child in Childs)
            builder.Append(child.Draw());

        return builder.ToString();
    }


    public virtual void Update()
    {
        foreach (IUpdateable property in PropertiesManager.GetAllPropertiesOfType<IUpdateable>())
            property.Update();

        Position.Value = Alignment.Value.CalculatePosition(this);

        UpdateEvent?.Invoke();

        foreach (Component child in Childs)
            child.Update();
    }
}
