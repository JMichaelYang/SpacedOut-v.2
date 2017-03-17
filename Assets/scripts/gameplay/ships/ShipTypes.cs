using UnityEngine;

public class ShipTypes
{
    public static readonly ShipType Debug = new ShipType(100f, 1.5f, "one/Debug", new Vector2(-0.2f, 0.2f), new Vector2(0.2f, 0.2f));
    public static readonly ShipType DebugSlow = new ShipType(200f, 1f, "one/DebugSlow", new Vector2(-0.2f, 0.2f), new Vector2(0.2f, 0.2f));
}

public class ShipType
{
    public float Health { get; protected set; }
    public float RotAccel { get; protected set; }
    public string SpritePath { get; protected set; }
    public Vector2[] Offsets { get; protected set; }

    /// <summary>
    /// Constructor for a ship type
    /// </summary>
    /// <param name="health">the health of the ship</param>
    /// <param name="rotAccel">how fast the ship can rotate</param>
    /// <param name="offsets">the offsets of the ship's weapons</param>
    public ShipType(float health, float rotAccel, string spritePath, params Vector2[] offsets)
    {
        this.Health = health;
        this.RotAccel = rotAccel;
        this.SpritePath = spritePath;
        this.Offsets = offsets;
    }
}