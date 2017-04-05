using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //components
    private new Transform transform;
    private Rigidbody2D rigidBody;
    private new SpriteRenderer renderer;
    private new CircleCollider2D collider;

    //stats
    public float Damage { get; protected set; }
    private float duration = 0f;

    private BulletHitEventArgs e = new BulletHitEventArgs(0f, null);

    // Use this for initialization
    void Awake()
    {
        this.Damage = 0f;
        this.duration = 0f;

        this.transform = this.GetComponent<Transform>();
        this.rigidBody = this.GetComponent<Rigidbody2D>();
        this.renderer = this.GetComponent<SpriteRenderer>();
        this.collider = this.GetComponent<CircleCollider2D>();
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
        this.renderer.sprite = image;
        this.collider.radius = this.renderer.bounds.extents.x < this.renderer.bounds.extents.y ? this.renderer.bounds.extents.x : this.renderer.bounds.extents.y;
        //accelerate bullet
        this.rigidBody.velocity = this.transform.up * velocity;

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
