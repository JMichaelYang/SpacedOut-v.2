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
    private CircleCollider2D bulletCollider;

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
        try { this.bulletCollider = this.gameObject.GetComponent<CircleCollider2D>(); }
        catch { Debug.Log("Could not find Collider component of " + this.gameObject.ToString()); }
    }

    public void Activate(float damage, float duration, float velocity, TeamIndex team, Sprite image)
    {
        this.reset();

        //set bullet stats
        this.Damage = damage;
        this.duration = duration;

        //set bullet layer
        #region Layer

        switch (team)
        {
            case TeamIndex.ONE:
                this.gameObject.layer = GameSettings.TeamOneBulletLayer;
                break;

            case TeamIndex.TWO:
                this.gameObject.layer = GameSettings.TeamTwoBulletLayer;
                break;

            case TeamIndex.THREE:
                this.gameObject.layer = GameSettings.TeamThreeBulletLayer;
                break;

            case TeamIndex.FOUR:
                this.gameObject.layer = GameSettings.TeamFourBulletLayer;
                break;
        }

        #endregion Layer

        //set bullet image and collider
        this.bulletRenderer.sprite = image;
        this.bulletCollider.radius = this.bulletRenderer.bounds.extents.x < this.bulletRenderer.bounds.extents.y ? this.bulletRenderer.bounds.extents.x : this.bulletRenderer.bounds.extents.y;

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
    }

    void Kill()
    {
        this.reset();
        ObjectPool.Despawn(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        CancelInvoke();

        //apply bullet damage
        this.e.ShotDamage = this.Damage;
        this.e.HitCollider = other;
        GameEventHandler.OnBulletHit(this, this.e);

        this.Kill();
    }
}
