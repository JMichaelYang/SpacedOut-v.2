using UnityEngine;

public class ShipTypes
{
    public static readonly ShipType A23F = new ShipType("A23F", 40000f, 1, 200f, 1.5f, 14f, "one/Debug", new Vector2(-0.2f, 0.2f), new Vector2(0.2f, 0.2f));
    public static readonly ShipType C20H = new ShipType("C20H", 60000f, 1, 400f, 1f, 10f, "one/DebugSlow", new Vector2(-0.2f, 0.2f), new Vector2(0.2f, 0.2f));
    public static readonly ShipType B12M = new ShipType("B12M", 50000f, 1, 300f, 1.2f, 12f, "one/DebugMedium", new Vector2(-0.2f, 0.2f), new Vector2(0.2f, 0.2f));
}

/// <summary>
/// A type of ship
/// </summary>
public struct ShipType : IShipComponent
{
    /// <summary>
    /// The name of this ship
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// The weight of this ship
    /// </summary>
    public float Weight { get; private set; }
    /// <summary>
    /// The tier of this ship
    /// </summary>
    public int Tier { get; private set; }
    /// <summary>
    /// The health of this ship
    /// </summary>
    public readonly float Health;
    /// <summary>
    /// The rotational velocity of this ship (in degrees)
    /// </summary>
    public readonly float RotVel;
    /// <summary>
    /// The maximum velocity that this ship can attain (in m/s)
    /// </summary>
    public readonly float MaxVel;
    /// <summary>
    /// The path to this ship's sprite
    /// </summary>
    public readonly string SpritePath;
    /// <summary>
    /// The places where this ship can shoot from
    /// </summary>
    public Vector2[] Offsets;

    public ShipType(string name, float weight, int tier, float health, float rotVel, float maxVel, string spritePath, params Vector2[] offsets)
    {
        this.Name = name;
        this.Weight = weight;
        this.Tier = tier;
        this.Health = health;
        this.RotVel = rotVel;
        this.MaxVel = maxVel;
        this.SpritePath = spritePath;
        this.Offsets = offsets;
    }
}