using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHandler : MonoBehaviour
{
    //ship type
    private ShipType shipType;

    //current health of the ship
    public float Health { get; protected set; }

    //explosion particle system
    private GameObject explosion;
    private ParticleSystem explosionSystem;
    //rigid body
    private Rigidbody2D physicsBody;
    //collider
    private Collider2D collider2d;

	// Use this for initialization
	void Start ()
    {
        this.Health = 100;

        this.explosion = Resources.Load<GameObject>(GameSettings.ShipExplosion);
        this.explosionSystem = this.explosion.GetComponent<ParticleSystem>();
        this.physicsBody = this.GetComponent<Rigidbody2D>();
        this.collider2d = this.GetComponent<Collider2D>();
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
        for(int i = 0; i < components.Length; i++)
        {
            if (!(components[i] is Transform) && !(components[i] is Rigidbody2D) && !(components[i] is ShipHandler))
            {
                Destroy(components[i]);
            }
        }

        //add drag to rigid body to stop it
        this.physicsBody.drag = 0.5f;
        //start explosion on ship location 
        //TODO: preload explosion
        ObjectPool.Spawn(this.explosion, this.transform.position, this.transform.rotation);
        Invoke("AfterExplosion", this.explosionSystem.main.duration + this.explosionSystem.main.startLifetime.constantMax);
    }

    void AfterExplosion()
    {
        ObjectPool.Despawn(this.explosion);
        Debug.Log("Deactivated explosion");
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

public class ShipType
{
    public float Health;
    public Vector2[] Offsets;

    public ShipType(float health)
    {
        this.Health = health;
    }
}