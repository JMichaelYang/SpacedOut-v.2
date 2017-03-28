using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AiManager))]
public class ShipHandler : MonoBehaviour
{
    //ship type
    //private ShipType shipType;

    //current health of the ship
    public float MaxHealth { get; protected set; }
    public float Health { get; protected set; }

    //explosion particle system
    private GameObject explosion;
    private ParticleSystem explosionSystem;
    //rigid body
    private new Rigidbody2D rigidbody;
    //movement
    private Movement movement;
    //collider
    private new Collider2D collider;
    //renderer
    private new SpriteRenderer renderer;

    //whether the ship is on the screen
    private bool isOffScreen = false;
    private Transform shipTransform = null;
    private AiManager shipAi = null;

    public bool IsAlive { get; protected set; }

    void Awake()
    {
        this.shipTransform = this.transform;
        this.shipAi = this.gameObject.GetComponent<AiManager>();
        this.movement = this.gameObject.GetComponent<Movement>();
        this.rigidbody = this.GetComponent<Rigidbody2D>();
        this.collider = this.GetComponent<Collider2D>();
        this.renderer = this.GetComponent<SpriteRenderer>();
        this.IsAlive = true;
    }

    public void SetStatistics(ShipType shipType)
    {
        this.MaxHealth = shipType.Health;
    }

    // Use this for initialization
    void Start()
    {
        this.explosion = Resources.Load<GameObject>(GameSettings.ShipExplosion);
        this.explosionSystem = this.explosion.GetComponent<ParticleSystem>();

        this.Health = this.MaxHealth;
    }

    //used for when the ship exits the arena
    void Update()
    {
        if (!this.isOffScreen &&
            (this.shipTransform.position.x > GameSettings.ArenaWidth / 2 ||
            this.shipTransform.position.x < -GameSettings.ArenaWidth / 2 ||
            this.shipTransform.position.y > GameSettings.ArenaHeight / 2 ||
            this.shipTransform.position.y < -GameSettings.ArenaHeight / 2))
        {
            if (this.gameObject.CompareTag("Player"))
            {
                InputHandler.Instance.enabled = false;
                this.shipAi.enabled = true;
            }

            this.shipAi.ClearBehavior();
            this.shipAi.AddBehavior(AvailableBehaviors.RETURN_CENTER);

            this.isOffScreen = true;
        }
        else if (this.isOffScreen &&
            (this.shipTransform.position.x > -GameSettings.ArenaWidth / 2 &&
            this.shipTransform.position.x < GameSettings.ArenaWidth / 2 &&
            this.shipTransform.position.y > -GameSettings.ArenaHeight / 2 &&
            this.shipTransform.position.y < GameSettings.ArenaHeight / 2))
        {
            this.shipAi.ClearBehavior();

            if (this.gameObject.CompareTag("Player"))
            {
                InputHandler.Instance.enabled = true;
                this.shipAi.enabled = false;
            }

            this.isOffScreen = false;
        }
    }

    /// <summary>
    /// Damage the ship
    /// </summary>
    /// <param name="damage">Amount to damage the ship by</param>
    void DamageShip(object sender, BulletHitEventArgs e)
    {
        if (e.HitCollider == this.collider)
        {
            this.Health -= e.Shot.Damage;

            //ship death
            if (this.Health < 0)
            {
                this.Health = 0;

                this.DestroyShip();
            }
        }
    }

    /// <summary>
    /// Remove all the components from the ship except for the transform, the handler, and the rigid body (for particle movement)
    /// </summary>
    private void DestroyShip()
    {
        //broadcast event that the player has died
        if (this.CompareTag("Player")) { GameEventHandler.OnPlayerDead(this, new EventArgs()); }

        MonoBehaviour[] components = this.gameObject.GetComponents<MonoBehaviour>();
        for (int i = 0; i < components.Length; i++)
        {
            if (!(components[i] is ShipHandler))
            {
                components[i].enabled = false;
            }
        }
        this.renderer.enabled = false;
        this.collider.enabled = false;
        //add drag to rigid body to stop it
        this.rigidbody.drag = 1f;
        //tag object as dead
        this.gameObject.tag = "Dead";
        //start explosion on ship location
        //TODO: preload explosion
        ObjectPool.Spawn(this.explosion, this.transform.position, this.transform.rotation);
        Invoke("AfterExplosion", this.explosionSystem.main.duration + this.explosionSystem.main.startLifetime.constantMax);

        //set this ship to dead
        this.IsAlive = false;
    }

    private void AfterExplosion()
    {
        ObjectPool.Despawn(this.explosion);
        this.rigidbody.Sleep();
        //Debug.Log("Deactivated explosion");
    }

    #region Event Registration

    void OnEnable()
    {
        //register event
        GameEventHandler.OnBulletHit += this.DamageShip;
    }
    void OnDisable()
    {
        //de-register event
        GameEventHandler.OnBulletHit -= this.DamageShip;
    }

    #endregion Event Registration
}

public class Ship
{
    public Team ShipTeam { get; protected set; }
    public ShipType Type { get; protected set; }
    public EngineType Engine { get; protected set; }
    public GunType[] Guns { get; protected set; }

    public Ship(Team team, ShipType type, EngineType engine, params GunType[] guns)
    {
        this.ShipTeam = team;
        this.Type = type;
        this.Engine = engine;
        this.Guns = guns;
    }

    /// <summary>
    /// Set the corresponding stats for an instance of the Ship prefab
    /// </summary>
    /// <param name="shipObject"></param>
    /// <returns></returns>
    public GameObject GetShipObject(GameObject shipObject)
    {
        shipObject.GetComponent<Weapons>().ReadWeapons(this.Guns, this.Type.Offsets);
        shipObject.GetComponent<Movement>().SetStatistics(this.Type, this.Engine);
        shipObject.GetComponent<ShipHandler>().SetStatistics(this.Type);

        SpriteRenderer spriteRenderer = shipObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>(GameSettings.ShipTexPath + this.Type.SpritePath);

        return shipObject;
    }
}