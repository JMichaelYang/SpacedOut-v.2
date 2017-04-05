using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ShieldState
{
    ACTIVE,
    REGENERATING,
    INACTIVE
}

public class Shield : MonoBehaviour
{
    private ShieldType type;
    private float regenAmount;

    public int ShieldHealth { get { return (int)this.shieldHealthFloat; } }
    public int ShieldMaxHealth { get { return (int)this.type.Health; } }
    private float shieldHealthFloat;
    private ShieldState state;

    private new CircleCollider2D collider;
    private new SpriteRenderer renderer;
    private Color color;

    void Awake()
    {
        this.collider = this.gameObject.GetComponent<CircleCollider2D>();
        this.renderer = this.gameObject.GetComponent<SpriteRenderer>();
        this.color = this.renderer.color;
    }

    public void Activate(ShieldType type)
    {
        this.type = type;

        if (this.type == ShieldTypes.None) { this.Disable(); }
        else
        {
            this.shieldHealthFloat = this.type.Health;
            this.regenAmount = this.type.Regen * GameSettings.ShieldRegenDelay;
            this.state = ShieldState.ACTIVE;
        }
    }

    void Update()
    {
        switch (this.state)
        {
            case ShieldState.ACTIVE:
                this.shieldHealthFloat += this.regenAmount;
                if (this.shieldHealthFloat > this.type.Health) { this.shieldHealthFloat = this.type.Health; }
                this.color.a = this.shieldHealthFloat / this.type.Health;
                this.renderer.color = this.color;
                break;

            case ShieldState.REGENERATING:
                this.shieldHealthFloat += this.regenAmount;
                if (this.shieldHealthFloat > this.type.Health)
                {
                    this.shieldHealthFloat = this.type.Health;
                    this.collider.enabled = true;
                    this.renderer.enabled = true;
                    this.state = ShieldState.ACTIVE;
                }
                break;
        }
    }

    /// <summary>
    /// Hit by bullet (should only be triggered when active)
    /// </summary>
    /// <param name="sender">the bullet that hit this shield</param>
    /// <param name="e">the arguments describing the event</param>
    private void TakeHit(object sender, BulletHitEventArgs e)
    {
        if (e.HitCollider == this.collider && state == ShieldState.ACTIVE)
        {
            this.shieldHealthFloat -= e.ShotDamage;
            if (this.shieldHealthFloat <= 0f)
            {
                this.shieldHealthFloat = 0f;
                this.state = ShieldState.REGENERATING;
                this.collider.enabled = false;
                this.renderer.enabled = false;
            }
        }
    }

    #region Event Registration

    void OnEnable()
    {
        //register event
        GameEventHandler.OnBulletHit += this.TakeHit;
    }
    void OnDisable()
    {
        //de-register event
        GameEventHandler.OnBulletHit -= this.TakeHit;
    }

    #endregion Event Registration

    public void Disable()
    {
        this.collider.enabled = false;
        this.renderer.enabled = false;
        this.state = ShieldState.INACTIVE;
        this.gameObject.SetActive(false);
    }
}
