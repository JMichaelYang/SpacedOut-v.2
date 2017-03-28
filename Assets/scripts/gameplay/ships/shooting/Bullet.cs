using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //components
    private Transform bulletTransform;
    private Rigidbody2D bulletBody;
    private SpriteRenderer bulletRenderer;
    private BoxCollider2D bulletCollider;
    //shooter and its shield
    private GameObject shooter;
    private Collider2D shieldCollider;

    //stats
    public float Damage { get; protected set; }
    private float duration = 0f;

    private BulletHitEventArgs e = new BulletHitEventArgs(0f, null);

    // Use this for initialization
    void Awake()
    {
        this.Damage = 0f;
        this.duration = 0f;

        try { this.bulletTransform = this.transform; }
        catch { Debug.Log("Could not find Transform component of " + this.gameObject.ToString()); }
        try { this.bulletBody = this.gameObject.GetComponent<Rigidbody2D>(); }
        catch { Debug.Log("Could not find RigidBody component of " + this.gameObject.ToString()); }
        try { this.bulletRenderer = this.gameObject.GetComponent<SpriteRenderer>(); }
        catch { Debug.Log("Could not find Renderer component of " + this.gameObject.ToString()); }
        try { this.bulletCollider = this.gameObject.GetComponent<BoxCollider2D>(); }
        catch { Debug.Log("Could not find Collider component of " + this.gameObject.ToString()); }
    }

    public void Activate(float damage, float duration, float velocity, GameObject shooter, Collider2D shield, Sprite image)
    {
        this.reset();

        //set bullet stats
        this.Damage = damage;
        this.duration = duration;
        //set shooter for this bullet
        this.shooter = shooter;
        this.shieldCollider = shield;

        this.bulletRenderer.sprite = image;
        this.bulletCollider.size = this.bulletRenderer.bounds.size;

        //accelerate bullet
        this.bulletBody.velocity = this.bulletTransform.up * velocity;

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
            if (this.shieldCollider != null && other.Equals(shieldCollider))
            {
                return;
            }

            CancelInvoke();

            //apply bullet damage
            this.e.ShotDamage = this.Damage;
            this.e.HitCollider = other;
            GameEventHandler.OnBulletHit(this, this.e);

            this.Kill();
        }
    }
}
