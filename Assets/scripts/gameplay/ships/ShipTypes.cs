using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTypes : MonoBehaviour
{
    public static readonly ShipType Debug = new ShipType(100f, 1.5f, new Vector2(-0.2f, 0.2f), new Vector2(0.2f, 0.2f));

	// Use this for initialization
	void Awake ()
    {
        DontDestroyOnLoad(this);
	}
}

public class ShipType
{
    public float Health;
    public float RotAccel;
    public Vector2[] Offsets;

    /// <summary>
    /// Constructor for a ship type
    /// </summary>
    /// <param name="health">the health of the ship</param>
    /// <param name="rotAccel">how fast the ship can rotate</param>
    /// <param name="offsets">the offsets of the ship's weapons</param>
    public ShipType(float health, float rotAccel, params Vector2[] offsets)
    {
        this.Health = health;
        this.RotAccel = rotAccel;
        this.Offsets = offsets;
    }
}