﻿
public class EngineTypes
{
    public static readonly EngineType Debug = new EngineType(12f, .14f);
    public static readonly EngineType DebugSlow = new EngineType(8f, 0.1f);
    public static readonly EngineType DebugMedium = new EngineType(10f, 0.12f);
}

public class EngineType
{
    public float MaxVelocity { get; protected set; }
    public float MaxAccel { get; protected set; }

    public EngineType(float maxVelocity, float maxAccel)
    {
        this.MaxVelocity = maxVelocity;
        this.MaxAccel = maxAccel;
    }
}
