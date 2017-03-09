using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    private Gun[] guns;
    private Vector3[] offsets;

    //timers and counters to keep track of reloads
    private float[] shotTimers;
    private float[] burstTimers;
    private float[] reloadTimers;
    private int[] burstCounters;
    private int[] shotCounters;

    private float oldTime = 0;

    public WeaponType type = WeaponTypes.DebugWeapon;

    private CommandHandler commandHandler;
    private CameraShake shake;

    private WeaponShootEventArgs e = new WeaponShootEventArgs(0f);

    //collider of this ship
    private Collider2D shipCollider = null;

    public void ReadWeapons(WeaponType weapons)
    {
        this.guns = new Gun[weapons.guns.Length];
        this.offsets = new Vector3[weapons.guns.Length];

        //load guns and offsets from weapon type
        this.offsets = new Vector3[weapons.offsets.Length];
        for (int i = 0; i < guns.Length; i++)
        {
            this.guns[i] = Gun.LoadFromGunType(weapons.guns[i]);
            this.offsets[i] = (Vector3)weapons.offsets[i];
        }
    }

    // Use this for initialization
    void Awake()
    {
        this.ReadWeapons(this.type);
        this.shotTimers = new float[this.guns.Length];
        this.burstTimers = new float[this.guns.Length];
        this.burstCounters = new int[this.guns.Length];
        this.reloadTimers = new float[this.guns.Length];
        this.shotCounters = new int[this.guns.Length];

        this.oldTime = 0;
    }

    void Start()
    {
        this.commandHandler = GameObject.FindObjectOfType<CommandHandler>();
        this.shake = GameObject.FindObjectOfType<CameraShake>();
        this.shipCollider = this.gameObject.GetComponent<Collider2D>();
    }

    public bool ShootWeapons(params int[] slots)
    {
        int numSlots = slots.Length;

        if (numSlots > this.guns.Length)
        {
            Debug.Log("Tried to shoot with " + numSlots + " slots and " + this.guns.Length + " guns...");
            return false;
        }

        bool hasShot = false;

        float interval = Time.time - this.oldTime;
        this.oldTime = Time.time;

        for (int i = 0; i < numSlots; i++)
        {
            //update timers
            this.shotTimers[i] += interval;
            this.burstTimers[i] += interval;
            this.reloadTimers[i] += interval;

            //checking reload timers
            if (this.reloadTimers[i] > this.guns[i].ReloadDelay && this.burstTimers[i] > this.guns[i].BurstDelay && this.shotTimers[i] > this.guns[i].ShotDelay)
            {
                //shoot the gun and add to the shot counters
                GameObject shotBullet = this.guns[slots[i]].ShootGun(this.transform.position +
                    (Vector3)Utils.RotateVector2(this.offsets[slots[i]], this.transform.rotation.eulerAngles.z),
                    this.transform.rotation, this.gameObject);
                //increment shot counters
                this.shotCounters[i]++;
                this.burstCounters[i]++;

                //ignore collisions between whoever fired this and the bullet itself
                Physics2D.IgnoreCollision(shotBullet.GetComponent<Collider2D>(), this.shipCollider, true);

                //reset shot timer every shot
                this.shotTimers[i] = 0;

                //if the burst is done, reset the burst counter and timer
                if (this.burstCounters[i] >= this.guns[i].BurstAmount)
                {
                    this.burstCounters[i] = 0;
                    this.burstTimers[i] = 0;
                }

                //if the magazine is done, reset the shot counter and timer
                if (this.shotCounters[i] >= this.guns[i].ReloadAmount)
                {
                    this.shotCounters[i] = 0;
                    this.reloadTimers[i] = 0;

                    this.burstCounters[i] = 0;
                    this.burstTimers[i] = 0;
                }

                this.e.Damage = this.guns[slots[i]].Damage;
                GameEventHandler.OnWeaponShoot(this, this.e);

                hasShot = true;
            }
        }

        return hasShot;
    }
}

public class WeaponType
{
    public GunType[] guns;
    public Vector2[] offsets;

    public WeaponType(GunType[] guns, Vector2[] offsets)
    {
        this.guns = guns;
        this.offsets = offsets;
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

public class GunType
{
    //damage
    public float Damage;
    //exit velocity
    public float Velocity;
    //time alive
    public float Range;
    //accuracy of the bullet
    public float Accuracy;
    //time between shots
    public float ShotDelay;
    //time between bursts
    public float BurstDelay;
    //time between reloads
    public float ReloadDelay;
    //shots per burst
    public int BurstAmount;
    //shots per reload
    public int ReloadAmount;
    //image path of the bullet
    public string BulletPath;

    /// <summary>
    /// Constructor that defines the stats of this gun type
    /// </summary>
    /// <param name="damage">damage dealt by the gun</param>
    /// <param name="velocity">exit velocity of gun projectiles</param>
    /// <param name="range">time that bullets travel in seconds</param>
    /// <param name="accuracy">accuracy of the bullet (lower is greater accuracy)</param>
    /// <param name="shotDelay">time between individual shots</param>
    /// <param name="burstDelay">time between burst reloads (0 for full auto)</param>
    /// <param name="reloadDelay">time between reloads (0 for semi auto)</param>
    /// <param name="burstAmount">number of shots per burst</param>
    /// <param name="reloadAmount">number of shots per reload</param>
    /// <param name="bulletPath">image path of the bullet</param>
    public GunType(float damage, float velocity, float range, float accuracy, float shotDelay, float burstDelay, float reloadDelay, 
        int burstAmount, int reloadAmount, string bulletPath)
    {
        this.Damage = damage;
        this.Velocity = velocity;
        this.Range = range;
        this.Accuracy = accuracy;

        this.ShotDelay = shotDelay;
        this.BurstDelay = burstDelay;
        this.ReloadDelay = reloadDelay;

        this.BurstAmount = burstAmount;
        this.ReloadAmount = reloadAmount;

        this.BulletPath = bulletPath;
    }
}