using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //life timer
    private float durationTimer = 0;
    //bulletmovement component
    private Movement movement;
    //shooter
    private GameObject shooter;

    //stats
    private float damage;
    private float duration = 0;

    // Use this for initialization
    void Awake ()
    {
        this.duration = 0;
        this.durationTimer = 0;

        try
        {
            this.movement = this.gameObject.GetComponent<Movement>();
        }
        catch
        {
            Debug.Log("Could not find Movement component of " + this.gameObject.ToString());
        }
    }

    public void Activate(float damage, float duration, float velocity, GameObject shooter)
    {
        this.reset();

        //set bullet stats
        this.damage = damage;
        this.duration = duration;

        //set shooter for this bullet
        this.shooter = shooter;

        //set bullet velocity
        this.movement.Accelerate(velocity);
    }

    //resets all of stats for re-pooling
    protected void reset()
    {
        this.duration = 0;
        this.durationTimer = 0;

        this.damage = 0;

        this.shooter = null;
    }

	// Update is called once per frame
	void Update ()
    {
        this.durationTimer += Time.deltaTime;

        //check if bullet should be removed
        if (this.durationTimer > this.duration)
        {
            this.reset();
            ObjectPool.Despawn(this.gameObject);
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != this.shooter)
        {
            this.durationTimer = this.duration + 1f;

            //apply bullet damage
            ShipHandler shipHandler = other.GetComponent<ShipHandler>();
            if (shipHandler != null)
            {
                shipHandler.DamageShip(this.damage);
            }
        }
    }
}
