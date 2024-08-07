﻿using System;
using System.Text;
using System.Collections.Generic;

using Specter.Terminal.Output;
using Specter.Terminal.UI.Components;
using Specter.Terminal.UI.Application.Exceptions;


namespace Specter.Terminal.UI.Application;


/// <summary>
/// Initializes a terminal UI app.
/// </summary>
public abstract class App
{
    public bool Exit { get; set; }

    /// <summary>
    /// The base Component of an UI App
    /// </summary>
    public RootComponent Root { get; private set; }


    /// <summary>
    /// The current running App instance.
    /// </summary>
    private static App? s_current_app;
    public static App CurrentApp
    {
        get => s_current_app ?? throw new AppException("There's no current app.");
        set => s_current_app = value;
    }

    public static bool HasCurrentApp => s_current_app is not null;



    /// <summary>
    /// Queue containing the Components that are going to be drawed/rendered.
    /// </summary>
    public List<Component> RenderQueue { get; private set; } = [];


    public bool DrawAllRequested { get; private set; } = false;


    public App()
    {
        TerminalAttributes.EchoEnabled = false;
        TerminalAttributes.CursorVisible = false;
        Console.OutputEncoding = new UTF8Encoding();

        // on CTRL+C pressed
        Console.CancelKeyPress += delegate { OnExit(); };

        SetThisAsCurrentApp();

        Exit = false;
        Root = new();

        Load();
    }



    /// <summary>
    /// Called after the construction of the class.
    /// </summary>
    protected virtual void Load() { }

    /// <summary>
    /// Called before the App starts the updating cycle.
    /// </summary>
    protected virtual void Start()
    {
        // first drawing

        Update();
        Draw();
    }

    /// <summary>
    /// Called every frame. Should update all the program.
    /// </summary>
    protected virtual void Update()
    {
        TerminalAttributes.Update();

        Root.Update();
    }

    /// <summary>
    /// Called every frame. Should draw the components that need to be drawn.
    /// </summary>
    protected virtual void Draw()
    {
        bool drawAll = TerminalAttributes.TerminalResized;

        if (drawAll is true)
            TerminalStream.ClearAllScreen();

        if (drawAll || DrawAllRequested)
            DrawAll(true);
        else
            DrawRenderQueue();

        DrawAllRequested = false;
    }

    /// <summary>
    /// Called at the end of the program. Should deinitialize stuff and back to normal state.
    /// </summary>
    protected virtual void End()
        => OnExit();



    /// <summary>
    /// Starts running the App.
    /// </summary>
    public void Run()
    {
        SetThisAsCurrentApp();

        try
        {
            Start();

            while (!Exit)
            {
                Update();
                Draw();
            }
        }

        finally
        {
            End();
        }
    }



    public void SetThisAsCurrentApp() => CurrentApp = this;



    public bool TryGetComponentByName(string name, out Component? component)
    {
        try
        {
            component = GetComponentByName(name);
            return true;
        }
        catch
        {
            component = null;
            return false;
        }
    }

    public Component GetComponentByName(string name)
    {
        Component? component = null;

        Component.ForeachChildIn(Root, child =>
        {
            if (child.Name == name)
                component = child;
        });

        if (component is null)
            throw new ComponentException(name, "The Component couldn't be found.");

        return component;
    }



    public static implicit operator Component(App app) => app.Root;



    public void AddComponentToRenderQueue(Component component)
    {
        if (!RenderQueue.Contains(component))
            RenderQueue.Add(component);
    }



    private void DrawRenderQueue()
    {
        foreach (Component component in RenderQueue)
            Console.Write(component.Draw());

        RenderQueue.Clear();
    }


    public void RequestDrawAll() => DrawAllRequested = true;

    private void DrawAll(bool clearRenderQueue = false)
    {
        Console.Write(Root.Draw());

        if (clearRenderQueue)
            CurrentApp.RenderQueue.Clear();
    }




    /// <summary>
    /// On exiting stuff.
    /// </summary>
    private static void OnExit()
    {
        Console.Write(ControlCodes.CursorToHome(), ControlCodes.EraseScreen());
        TerminalAttributes.EchoEnabled = true;
        TerminalAttributes.CursorVisible = true;
    }
}
