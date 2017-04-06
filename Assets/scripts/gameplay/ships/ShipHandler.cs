﻿using System;
using UnityEngine;

/// <summary>
/// Controller attached to every ship
/// </summary>
[RequireComponent(typeof(AiManager))]
public class ShipHandler : MonoBehaviour
{
    /// <summary>
    /// References to all of the ship's components
    /// </summary>
    #region Components

    //explosion particle system
    public GameObject explosion;
    private ParticleSystem explosionSystem;

    public Transform cTransform { get; protected set; }
    public Rigidbody2D cRigidbody { get; protected set; }
    public Movement cMovement { get; protected set; }
    public Collider2D cCollider { get; protected set; }
    public SpriteRenderer cRenderer { get; protected set; }
    public Health cHealth;
    private AiManager shipAi;
    /// <summary>
    /// Whether this ship has a shield
    /// </summary>
    public bool HasShield;
    public Shield cShield;

    #endregion Components

    /// <summary>
    /// Area for storing modifiers to this ship's statistics
    /// </summary>
    #region Type Statistics

    #endregion Type Statistics

    /// <summary>
    /// Whether the ship is on the screen
    /// </summary>
    private bool isOffScreen = false;

    public bool IsAlive { get { return this.cHealth.IsAlive; } }

    /// <summary>
    /// Get references to all of this ship's components
    /// </summary>
    void Awake()
    {
        #region Get Component References

        this.cTransform = this.gameObject.GetComponent<Transform>();
        this.cRigidbody = this.GetComponent<Rigidbody2D>();
        this.cMovement = this.gameObject.GetComponent<Movement>();
        this.cCollider = this.GetComponent<Collider2D>();
        this.cRenderer = this.GetComponent<SpriteRenderer>();
        this.cHealth = this.GetComponent<Health>();
        this.shipAi = this.gameObject.GetComponent<AiManager>();
        this.cShield = this.gameObject.GetComponentInChildren<Shield>();
        if (this.cShield != null) { this.HasShield = true; }
        else { this.HasShield = false; }

        #endregion Get Component References
    }

    /// <summary>
    /// Set the component statistics of this ship
    /// </summary>
    /// <param name="shipType">the ShipType of this ship</param>
    /// <param name="armorType">the ArmorType of this ship</param>
    /// <param name="shieldType">the ShieldType of this ship</param>
    public void SetStatistics(ShipType shipType, ArmorType armorType, ShieldType shieldType)
    {
        if(this.HasShield) { this.cShield.Activate(shieldType); }
    }

    // Use this for initialization
    void Start()
    {
        this.explosion = Resources.Load<GameObject>(GameSettings.ShipExplosion);
        this.explosionSystem = this.explosion.GetComponent<ParticleSystem>();

        this.cHealth.SetHealth(this.cHealth.IntMaxHealth);
    }

    //used for when the ship exits the arena
    void Update()
    {
        if (!this.isOffScreen &&
            (this.cTransform.position.x > GameSettings.ArenaWidth / 2 ||
            this.cTransform.position.x < -GameSettings.ArenaWidth / 2 ||
            this.cTransform.position.y > GameSettings.ArenaHeight / 2 ||
            this.cTransform.position.y < -GameSettings.ArenaHeight / 2))
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
            (this.cTransform.position.x > -GameSettings.ArenaWidth / 2 &&
            this.cTransform.position.x < GameSettings.ArenaWidth / 2 &&
            this.cTransform.position.y > -GameSettings.ArenaHeight / 2 &&
            this.cTransform.position.y < GameSettings.ArenaHeight / 2))
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
        if (e.HitCollider == this.cCollider)
        {
            if(this.cHealth.ApplyDamage(e.ShotDamage) <= 0f) { this.DestroyShip(); }
        }
    }

    /// <summary>
    /// Remove all the components from the ship except for the transform, the handler, and the rigid body (for particle movement)
    /// </summary>
    private void DestroyShip()
    {
        //If this is the player, broadcast an event that the player has died
        if (this.CompareTag("Player")) { GameEventHandler.OnPlayerDead(this, new EventArgs()); }

        //disable components
        MonoBehaviour[] components = this.gameObject.GetComponents<MonoBehaviour>();
        for (int i = 0; i < components.Length; i++) { if (!(components[i] is ShipHandler)) { components[i].enabled = false; } }
        this.cRenderer.enabled = false;
        this.cCollider.enabled = false;

        //Disable the shield if it exists
        if (this.HasShield) { this.cShield.Disable(); }

        //add drag to rigid body to stop it
        this.cRigidbody.drag = 1f;
        //tag object as dead
        this.gameObject.tag = "Dead";
        //start explosion on ship location
        //TODO: preload explosion
        ObjectPool.Spawn(this.explosion, this.cTransform.position, this.cTransform.rotation);
        Invoke("AfterExplosion", this.explosionSystem.main.duration + this.explosionSystem.main.startLifetime.constantMax);
    }

    /// <summary>
    /// Invoked after the explosion finishes playing out
    /// </summary>
    private void AfterExplosion()
    {
        ObjectPool.Despawn(this.explosion);
        this.cRigidbody.Sleep();
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

/// <summary>
/// A ship with all of its components
/// </summary>
public class Ship
{
    public Team ShipTeam { get; protected set; }
    public ShipType Type { get; protected set; }
    public EngineType Engine { get; protected set; }
    public ShieldType Shield { get; protected set; }
    public ArmorType Armor { get; protected set; }
    public GunType[] Guns { get; protected set; }
    
    protected readonly float totalWeight;

    public Ship(Team team, ShipType type, EngineType engine, ShieldType shield, ArmorType armor, params GunType[] guns)
    {
        this.ShipTeam = team;
        this.Type = type;
        this.Engine = engine;
        this.Shield = shield;
        this.Armor = armor;
        this.Guns = guns;

        this.totalWeight += type.Weight + engine.Weight + shield.Weight + armor.Weight;
        foreach(GunType gun in guns) { this.totalWeight += gun.Weight; }
    }

    /// <summary>
    /// Set the corresponding stats for an instance of the Ship prefab
    /// </summary>
    /// <param name="shipObject"></param>
    /// <returns></returns>
    public GameObject GetShipObject(GameObject shipObject)
    {
        shipObject.GetComponent<Weapons>().ReadWeapons(this.Guns, this.Type.Offsets);
        shipObject.GetComponent<Movement>().SetStatistics(this.Type.MaxVel, this.Engine.Thrust, this.Type.RotVel, GameSettings.DampeningMultiplier, GameSettings.DampenInteria);
        shipObject.GetComponent<Health>().SetStatistics(this.Type.Health + this.Armor.Health);
        shipObject.GetComponent<ShipHandler>().SetStatistics(this.Type, this.Armor, this.Shield);

        SpriteRenderer spriteRenderer = shipObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>(GameSettings.ShipTexPath + this.Type.SpritePath);

        CircleCollider2D collider = shipObject.GetComponent<CircleCollider2D>();
        collider.radius = (spriteRenderer.bounds.extents.x + spriteRenderer.bounds.extents.y) / 2;

        Rigidbody2D rigidBody = shipObject.GetComponent<Rigidbody2D>();
        rigidBody.mass = this.totalWeight;

        #region Layers

        switch (this.ShipTeam.Index)
        {
            case TeamIndex.ONE:
                shipObject.layer = GameSettings.TeamOneShipLayer;
                foreach (Transform child in shipObject.GetComponentsInChildren<Transform>()) { child.gameObject.layer = GameSettings.TeamOneShipLayer; }
                break;
            case TeamIndex.TWO:
                shipObject.layer = GameSettings.TeamTwoShipLayer;
                foreach (Transform child in shipObject.GetComponentsInChildren<Transform>()) { child.gameObject.layer = GameSettings.TeamTwoShipLayer; }
                break;
            case TeamIndex.THREE:
                shipObject.layer = GameSettings.TeamThreeShipLayer;
                foreach (Transform child in shipObject.GetComponentsInChildren<Transform>()) { child.gameObject.layer = GameSettings.TeamThreeShipLayer; }
                break;
            case TeamIndex.FOUR:
                shipObject.layer = GameSettings.TeamFourShipLayer;
                foreach (Transform child in shipObject.GetComponentsInChildren<Transform>()) { child.gameObject.layer = GameSettings.TeamFourShipLayer; }
                break;
        }

        #endregion Layers

        return shipObject;
    }
}