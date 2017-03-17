using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AiManager))]
public class ShipHandler : MonoBehaviour
{
    //ship type
    private ShipType shipType;

    //current health of the ship
    public float MaxHealth { get; protected set; }
    public float Health = 100f;

    //explosion particle system
    private GameObject explosion;
    private ParticleSystem explosionSystem;
    //rigid body
    private Rigidbody2D physicsBody;
    //movement
    private Movement movement;
    //collider
    private Collider2D collider2d;

    //whether the ship is on the screen
    private bool isOffScreen = false;
    private Transform shipTransform = null;
    private AiManager shipAi = null;

    void Awake()
    {
        this.shipTransform = this.transform;
        this.shipAi = this.gameObject.GetComponent<AiManager>();
        this.movement = this.gameObject.GetComponent<Movement>();
        this.physicsBody = this.GetComponent<Rigidbody2D>();
        this.collider2d = this.GetComponent<Collider2D>();
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
            this.shipAi.AddBehavior(new AiSeekPointBehavior(Vector2.zero, this.shipTransform, this.movement.MaxAcceleration));

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
        if (e.HitCollider == this.collider2d)
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
        Component[] components = this.gameObject.GetComponents<Component>();
        for (int i = 0; i < components.Length; i++)
        {
            if (!(components[i] is Transform) && !(components[i] is Rigidbody2D) && !(components[i] is ShipHandler) && !(components[i] is AiManager))
            {
                Destroy(components[i]);
            }
        }

        //add drag to rigid body to stop it
        this.physicsBody.drag = 0.5f;
        //tag object as dead
        this.gameObject.tag = "Dead";
        //start explosion on ship location
        //TODO: preload explosion
        ObjectPool.Spawn(this.explosion, this.transform.position, this.transform.rotation);
        Invoke("AfterExplosion", this.explosionSystem.main.duration + this.explosionSystem.main.startLifetime.constantMax);
    }

    private void AfterExplosion()
    {
        ObjectPool.Despawn(this.explosion);
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