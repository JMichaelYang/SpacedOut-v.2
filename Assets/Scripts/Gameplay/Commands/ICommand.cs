using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic command interface
/// </summary>
public interface ICommand
{
    /// <summary>
    /// The function to execute the command
    /// </summary>
    void Execute();
}

/// <summary>
/// Command to accelerate the movement component in the direction that it is facing
/// </summary>
public struct CommandAccelerateDirectional : ICommand
{
    private readonly Movement target;
    private readonly float magnitude;
    private readonly bool limit;

    /// <summary>
    /// A command to accelerate the movement component in the direction that it is facing
    /// </summary>
    /// <param name="target">the movement component to accelerate</param>
    /// <param name="magnitude">the thrust to accelerate it by</param>
    /// <param name="limit">whether to limit the acceleration</param>
    public CommandAccelerateDirectional(Movement target, float magnitude, bool limit = true)
    {
        this.target = target;
        this.magnitude = magnitude;
        this.limit = limit;
    }

    public void Execute()
    {
        this.target.ApplyDirectionalThrust(this.magnitude, this.limit);
    }
}

/// <summary>
/// Command to accelerate the movement component with the specified x and y
/// </summary>
public struct CommandAccelerateXY : ICommand
{
    private readonly Movement target;
    private readonly float x, y;
    private readonly bool limit;

    /// <summary>
    /// A command to accelerate the movement component with the specified x and y thrusts
    /// </summary>
    /// <param name="target">the movement component to accelerate</param>
    /// <param name="x">the x component of the thrust</param>
    /// <param name="y">the y component of the thrust</param>
    /// <param name="limit">whether to limit the acceleration</param>
    public CommandAccelerateXY(Movement target, float x, float y, bool limit = true)
    {
        this.target = target;
        this.x = x;
        this.y = y;
        this.limit = limit;
    }

    public void Execute()
    {
        this.target.ApplyThrust(this.x, this.y, this.limit);
    }
}

/// <summary>
/// Command to rotate the movement component
/// </summary>
public struct CommandRotate : ICommand
{
    private readonly Movement target;
    private readonly float magnitude;
    private readonly bool limit;

    /// <summary>
    /// A command to rotate a movement component
    /// </summary>
    /// <param name="target">the movement component to rotate</param>
    /// <param name="magnitude">the amount to rotate it by</param>
    /// <param name="limit">whether to limit the rotation</param>
    public CommandRotate(Movement target, float magnitude, bool limit = true)
    {
        this.target = target;
        this.magnitude = magnitude;
        this.limit = limit;
    }

    public void Execute()
    {
        this.target.Rotate(this.magnitude, this.limit);
    }
}

/// <summary>
/// Command to zoom the camera
/// </summary>
public struct CommandZoom : ICommand
{
    private readonly CameraZoom target;
    private readonly float magnitude;

    /// <summary>
    /// A command to change the zoom of the camera
    /// </summary>
    /// <param name="target">the camera zoom component to change</param>
    /// <param name="magnitude">the amount to zoom by</param>
    public CommandZoom(CameraZoom target, float magnitude)
    {
        this.target = target;
        this.magnitude = magnitude;
    }

    public void Execute()
    {
        this.target.ChangeZoom(magnitude);
    }
}

/// <summary>
/// Command to shoot with a weapons component
/// </summary>
public struct CommandShoot : ICommand
{
    private readonly Weapons target;
    private readonly int[] slots;

    /// <summary>
    /// A command to shoot with a weapons component
    /// </summary>
    /// <param name="target">the weapons component to shoot with</param>
    /// <param name="slots">which slots to shoot with</param>
    public CommandShoot(Weapons target, params int[] slots)
    {
        this.target = target;
        this.slots = slots;
    }

    public void Execute()
    {
        this.target.ShootWeapons(this.slots);
    }
}

/// <summary>
/// Command to shake the camera
/// </summary>
public struct CommandShake : ICommand
{
    private readonly CameraShake target;
    private readonly float strength;
    private readonly float decrease;

    /// <summary>
    /// A command to shake the camera
    /// </summary>
    /// <param name="target">the camera shake component to shake</param>
    /// <param name="strength">the strength with which to shake the camera</param>
    /// <param name="decrease">how quickly to decrease the camera shake</param>
    public CommandShake(CameraShake target, float strength, float decrease)
    {
        this.target = target;
        this.strength = strength;
        this.decrease = decrease;
    }

    public void Execute()
    {
        this.target.StartShake(this.strength, this.decrease);
    }
}