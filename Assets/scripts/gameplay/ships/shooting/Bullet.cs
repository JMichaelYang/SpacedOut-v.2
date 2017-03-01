using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float duration = 0;
    private float durationTimer = 0;

    //bullet collider
    private BoxCollider2D boxCollider;
    //shooter
    private GameObject shooter;

    //stats
    private float damage;

	// Use this for initialization
	void Start ()
    {
        this.duration = 0;
        this.durationTimer = 0;
        this.boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
    }

    public void Activate(float damage, float duration, float velocity, GameObject shooter)
    {
        this.reset();

        //set bullet stats
        this.damage = damage;
        this.duration = duration;

        Debug.Log("Shooter set to: " + this.shooter.ToString());

        //set bullet velocity
        this.gameObject.GetComponent<Movement>().Accelerate(velocity);

        //set shooter for this bullet
        this.shooter = shooter;

        //Debug.Log("Shooter set to: " + this.shooter.ToString());
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
        Debug.Log(other.gameObject.ToString());
        Debug.Log("was hit by");
        Debug.Log(this.shooter.ToString());

        if (other.gameObject != this.shooter)
        {
            Debug.Log("Hit registered");

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
