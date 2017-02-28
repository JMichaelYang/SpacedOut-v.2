using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHandler : MonoBehaviour
{
    //ship type
    private ShipType shipType;

    //current health of the ship
    public float Health { get; protected set; }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    /// <summary>
    /// Damage the ship
    /// </summary>
    /// <param name="damage">Amount to damage the ship by</param>
    public void DamageShip(float damage)
    {
        this.Health -= damage;

        if (this.Health < 0)
            this.Health = 0;
    }
}

public class ShipType
{
    public float Health;

    public ShipType(float health)
    {
        this.Health = health;
    }
}