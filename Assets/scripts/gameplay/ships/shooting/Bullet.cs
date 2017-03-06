using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //bulletmovement component
    private Movement movement;
    //shooter
    private GameObject shooter;

    //stats
    public float Damage { get; protected set; }
    private float duration = 0f;

    private BulletHitEventArgs e = new BulletHitEventArgs(null, null);

    // Use this for initialization
    void Awake()
    {
        this.Damage = 0f;
        this.duration = 0f;

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
        this.Damage = damage;
        this.duration = duration;
        //set shooter for this bullet
        this.shooter = shooter;

        //set bullet velocity
        this.movement.Accelerate(velocity);

        //set this bullet up to be destroyed after its duration elapses
        Invoke("Kill", this.duration);
    }

    //resets all of stats for re-pooling
    protected void reset()
    {
        this.duration = 0f;
        this.Damage = 0f;
        this.shooter = null;
    }

    void Kill()
    {
        this.reset();
        ObjectPool.Despawn(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != this.shooter)
        {
            CancelInvoke();

            //apply bullet damage
            this.e.Shot = this;
            this.e.HitCollider = other;
            GameEventHandler.OnBulletHit(this, this.e);

            this.Kill();
        }
    }
}
