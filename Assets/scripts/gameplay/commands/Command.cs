using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base command class
public class Command
{
    //the item to receive the action
    protected object receiver;
    //method name
    protected string name;
    //method arguments
    protected object[] args;

    public Command(object target, string name, params object[] args)
    {
        this.receiver = target;
        this.name = name;
        this.args = args;
    }

    public void Execute()
    {
        try
        {
            this.receiver.GetType().GetMethod(this.name).Invoke(this.receiver, this.args);
        }
        catch (MissingMethodException)
        {
            Debug.Log("Could not find method " + this.name + " in class " + this.receiver);
        }
        catch
        {
            Debug.Log(this.receiver);
            Debug.Log(this.name);
            Debug.Log(this.args[0]);
            Debug.Log("Unknown error when calling command");
        }
    }
}

public class AccelerateCommand : Command
{
    public AccelerateCommand(object target, float magnitude, bool limit)
        : base(target, "Accelerate", magnitude, limit) { }

    public AccelerateCommand(object target, float xAccel, float yAccel, bool limit)
        : base(target, "LinearAccelerate", xAccel, yAccel, limit) { }
}

public class RotateCommand : Command
{
    public RotateCommand(object target, float magnitude)
        : base(target, "Rotate", magnitude) { }
}

public class ZoomCommand : Command
{
    public ZoomCommand(object target, float magnitude)
        : base(target, "ChangeZoom", magnitude) { }
}

public class ShootCommand : Command
{
    public ShootCommand(object target, params int[] slots)
        : base(target, "ShootWeapons", slots) { }
}

public class ShakeCommand : Command
{
    public ShakeCommand(object target, float strength, float decrease)
        : base(target, "StartShake", strength, decrease) { }
}