using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    private struct OffsetGunPair
    {
        public Vector3 ValueOffset;
        public Gun ValueGun;
    };

    private OffsetGunPair[] weapons;
    private Vector3 offset;
    private Gun gun;

    private float oldTime = 0;

    private CommandHandler commandHandler;
    private CameraShake shake;

    private WeaponShootEventArgs e = new WeaponShootEventArgs(0f);

    //collider of this ship
    //private Collider2D shipCollider = null;

    private Team shooterTeam;
    private Collider2D[] teamColliders;

    public void SetTeam(Team team)
    {
        this.shooterTeam = team;
        this.teamColliders = new Collider2D[this.shooterTeam.FriendlyShips.Count];
        for (int i = 0; i < this.shooterTeam.FriendlyShips.Count; i++)
        {
            this.teamColliders[i] = this.shooterTeam.FriendlyShips[i].GetComponent<Collider2D>();
        }
    }

    public void ReadWeapons(GunType[] guns, Vector2[] offsets)
    {
        this.weapons = new OffsetGunPair[offsets.Length];

        for (int i = 0; i < offsets.Length; i++)
        {
            this.weapons[i] = new OffsetGunPair();
            this.weapons[i].ValueOffset = offsets[i];

            if (guns[i] != null) { this.weapons[i].ValueGun = Gun.LoadFromGunType(guns[i]); }
            else { this.weapons[i].ValueGun = null; }
        }
    }

    // Use this for initialization
    void Awake()
    {
        this.oldTime = 0;
    }

    void Start()
    {
        this.commandHandler = GameObject.FindObjectOfType<CommandHandler>();
        this.shake = GameObject.FindObjectOfType<CameraShake>();
        //this.shipCollider = this.gameObject.GetComponent<Collider2D>();
    }

    public bool ShootWeapons(params int[] slots)
    {
        int numSlots = slots.Length;

        if (numSlots > this.weapons.Length)
        {
            Debug.Log("Tried to shoot with " + numSlots + " slots and " + this.weapons.Length + " guns...");
            return false;
        }

        bool hasShot = false;

        float interval = Time.time - this.oldTime;
        this.oldTime = Time.time;

        for (int i = 0; i < numSlots; i++)
        {
            this.offset = this.weapons[i].ValueOffset;
            this.gun = this.weapons[i].ValueGun;

            if (this.gun != null)
            {
                //update timers
                this.gun.shotTimer += interval;
                this.gun.burstTimer += interval;
                this.gun.reloadTimer += interval;

                //checking reload timers
                if (this.gun.reloadTimer > this.gun.ReloadDelay &&
                    this.gun.burstTimer > this.gun.BurstDelay &&
                    this.gun.shotTimer > this.gun.ShotDelay)
                {
                    //shoot the gun and add to the shot counters
                    GameObject shotBullet = this.gun.ShootGun(this.transform.position +
                        (Vector3)Utils.RotateVector2(this.offset, this.transform.rotation.eulerAngles.z),
                        this.transform.rotation, this.gameObject);
                    //increment shot counters
                    this.gun.shotCounter++;
                    this.gun.burstCounter++;
                    //reset shot timer every shot
                    this.gun.shotTimer = 0f;

                    Collider2D shotCollider = shotBullet.GetComponent<Collider2D>();
                    //ignore collisions between whoever fired this and the bullet itself
                    for (int c = 0; c < this.teamColliders.Length; c++)
                    {
                        Physics2D.IgnoreCollision(shotCollider, this.teamColliders[c], true);
                    }

                    //if the burst is done, reset the burst counter and timer
                    if (this.gun.burstCounter >= this.gun.BurstAmount)
                    {
                        this.gun.burstCounter = 0;
                        this.gun.burstTimer = 0;
                    }

                    //if the magazine is done, reset the shot counter and timer
                    if (this.gun.shotCounter >= this.gun.ReloadAmount)
                    {
                        this.gun.shotCounter = 0;
                        this.gun.reloadTimer = 0f;

                        this.gun.burstCounter = 0;
                        this.gun.burstTimer = 0f;
                    }

                    this.e.Damage = this.gun.Damage;
                    GameEventHandler.OnWeaponShoot(this, this.e);

                    hasShot = true;
                }
            }
            else
            {
                Debug.Log("Tried to shoot a null gun...");
            }
        }

        return hasShot;
    }
}

public class Gun
{
    //damage
    public float Damage { get; protected set; }
    //exit velocity
    public float Velocity { get; protected set; }
    //time alive
    public float Range { get; protected set; }
    //accuracy of the bullet
    public float Accuracy { get; protected set; }
    //time between shots
    public float ShotDelay { get; protected set; }
    //time between bursts
    public float BurstDelay { get; protected set; }
    //time between reloads
    public float ReloadDelay { get; protected set; }
    //shots per burst
    public int BurstAmount { get; protected set; }
    //shots per reload
    public int ReloadAmount { get; protected set; }

    //timers and counters to keep track of reloads
    public float shotTimer;
    public float burstTimer;
    public float reloadTimer;
    public int burstCounter;
    public int shotCounter;

    //the prefab for the bullet
    private Sprite bulletSprite;
    private GameObject bulletPrefab;

    public static Gun LoadFromGunType(GunType gunType)
    {
        Gun gun = new Gun();

        gun.Damage = gunType.Damage;
        gun.Velocity = gunType.Velocity;
        gun.Range = gunType.Range;
        gun.Accuracy = gunType.Accuracy;

        gun.ShotDelay = gunType.ShotDelay;
        gun.BurstDelay = gunType.BurstDelay;
        gun.ReloadDelay = gunType.ReloadDelay;

        gun.BurstAmount = gunType.BurstAmount;
        gun.ReloadAmount = gunType.ReloadAmount;

        //new bullet prefab with necessary components for a bullet
        try { gun.bulletPrefab = Resources.Load<GameObject>("prefabs/weapons/bullets/bullet"); }
        catch { Debug.Log("Could not find bullet prefab"); }
        try { gun.bulletSprite = Resources.Load<Sprite>("sprites/weapons/bullets/" + gunType.BulletPath); }
        catch { Debug.Log("Could not find bullet sprite with name: " + gunType.BulletPath); }

        gun.shotTimer = 0f;
        gun.burstTimer = 0f;
        gun.burstCounter = 0;
        gun.reloadTimer = 0f;
        gun.shotCounter = 0;

        return gun;
    }

    public GameObject ShootGun(Vector3 pos, Quaternion rot, GameObject shooter)
    {
        GameObject bullet = ObjectPool.Spawn(this.bulletPrefab, pos, rot);
        bullet.transform.Rotate(0, 0, Random.Range(-this.Accuracy, this.Accuracy));
        bullet.GetComponent<Bullet>().Activate(this.Damage, this.Range, this.Velocity, shooter, this.bulletSprite);
        return bullet;
    }
}