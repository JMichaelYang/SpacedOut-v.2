using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHandler : MonoBehaviour
{
    //ship type
    private ShipType shipType;

    //current health of the ship
    public float Health { get; protected set; }

    //explosion particle system
    private GameObject explosion;
    //rigid body
    private Rigidbody2D physicsBody;

	// Use this for initialization
	void Start ()
    {
        this.Health = 100;

        this.explosion = Resources.Load<GameObject>(GameSettings.ShipExplosion);
        this.physicsBody = this.GetComponent<Rigidbody2D>();
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

        //ship death
        if (this.Health < 0)
        {
            this.Health = 0;

            this.DestroyShip();
        }
    }

    /// <summary>
    /// Remove all the components from the ship except for the transform, the handler, and the rigid body (for particle movement)
    /// </summary>
    private void DestroyShip()
    {
        foreach (Component component in this.gameObject.GetComponents<Component>())
        {
            if (!(component is Transform) && !(component is Rigidbody2D) && !(component is ShipHandler))
            {
                Destroy(component);
            }
        }

        //add drag to rigid body to stop it
        this.physicsBody.drag = 0.5f;
        //start explosion on ship location
        Instantiate(this.explosion, this.transform).transform.localPosition = Vector3.zero;
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