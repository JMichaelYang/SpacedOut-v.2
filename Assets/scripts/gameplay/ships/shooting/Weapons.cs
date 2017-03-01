using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    private Gun[] guns;
    private Vector3[] offsets;
    private float[] shotTimers;
    private float[] burstTimers;
    private int[] burstCounters;
    private float[] reloadTimers;
    private int[] shotCounters;

    public WeaponType type = WeaponTypes.DebugWeapon;

    private CommandHandler commandHandler;
    private CameraShake shake;

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

        //TODO: delete this test code
        for (int i = 0; i < this.guns.Length; i++)
        {
            this.guns[i].BurstDelay = 0.05f;
        }
    }

    void Start()
    {
        this.commandHandler = GameObject.FindObjectOfType<CommandHandler>();
        this.shake = GameObject.FindObjectOfType<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < this.burstTimers.Length; i++)
        {
            this.shotTimers[i] += Time.deltaTime;
            this.burstTimers[i] += Time.deltaTime;
            this.reloadTimers[i] += Time.deltaTime;
        }
    }

    public bool ShootWeapons(params int[] slots)
    {
        //this.guns[0].ShootGun(this.transform.position + this.offsets[0], this.transform.rotation);
        for (int i = 0; i < slots.Length; i++)
        {
            //checking reload timers
            if (this.reloadTimers[i] > this.guns[i].Reload)
            {
                //checking burst timers
                if (this.burstTimers[i] > this.guns[i].BurstReload)
                {
                    //checking shot timers
                    if (this.shotTimers[i] > this.guns[i].BurstDelay)
                    {
                        //shoot the gun and add to the shot counters
                        GameObject shotBullet = this.guns[slots[i]].ShootGun(this.transform.position + (Vector3)Utils.RotateVector2(this.offsets[slots[i]], this.transform.rotation.eulerAngles.z), 
                            this.transform.rotation, this.gameObject);
                        this.shotCounters[i]++;
                        this.burstCounters[i]++;

                        //ignore collisions between whoever fired this and the bullet itself
                        Physics2D.IgnoreCollision(shotBullet.GetComponent<Collider2D>(), this.gameObject.GetComponent<Collider2D>(), true);

                        //reset shot timer every shot
                        this.shotTimers[i] = 0;

                        //if the burst is done, reset the burst counter and timer
                        if (this.burstCounters[i] > this.guns[i].BurstAmount)
                        {
                            this.burstCounters[i] = 0;
                            this.burstTimers[i] = 0;
                        }

                        //if the magazine is done, reset the shot counter and timer
                        if (this.shotCounters[i] > this.guns[i].ReloadAmount)
                        {
                            this.shotCounters[i] = 0;
                            this.reloadTimers[i] = 0;

                            this.burstCounters[i] = 0;
                            this.burstTimers[i] = 0;
                        }

                        this.shake.StartShake(GameSettings.ShotShake, GameSettings.ShakeDecrease);

                        return true;
                    }
                }
            }
        }

        return false;
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
    public float Damage { get; protected set; }
    public float Velocity { get; protected set; }
    public float Range { get; protected set; }
    public float BurstDelay { get; set; }//protected set; }
    public int BurstAmount { get; protected set; }
    public float BurstReload { get; protected set; }
    public float Reload { get; protected set; }
    public int ReloadAmount { get; protected set; }
    public float Accuracy { get; protected set; }

    //the prefab for the bullet
    private Sprite BulletSprite;
    private GameObject bulletPrefab;

    public static Gun LoadFromGunType(GunType gunType)
    {
        Gun gun = new Gun();
        gun.Damage = gunType.Damage;
        gun.Velocity = gunType.Velocity;
        gun.Range = gunType.Range;
        gun.BurstDelay = gunType.BurstDelay;
        gun.BurstAmount = gunType.BurstAmount;
        gun.BurstReload = gunType.BurstReload;
        gun.Reload = gunType.Reload;
        gun.ReloadAmount = gunType.ReloadAmount;
        gun.Accuracy = gunType.Accuracy;

        try { gun.BulletSprite = Resources.Load<Sprite>("sprites/weapons/bullets/" + gunType.BulletPath); }
        catch { Debug.Log("Could not find bullet sprite with name: " + gunType.BulletPath); }

        //new bullet prefab with necessary components for a bullet
        try { gun.bulletPrefab = Resources.Load<GameObject>("prefabs/weapons/bullets/bullet"); }
        catch { Debug.Log("Could not find bullet prefab"); }
        gun.bulletPrefab.GetComponent<SpriteRenderer>().sprite = gun.BulletSprite;

        return gun;
    }

    public GameObject ShootGun(Vector3 pos, Quaternion rot, GameObject shooter)
    {
        GameObject bullet = ObjectPool.Spawn(this.bulletPrefab, pos, rot);
        bullet.transform.Rotate(0, 0, Random.Range(-this.Accuracy, this.Accuracy));
        bullet.GetComponent<Bullet>().Activate(this.Damage, this.Range, this.Velocity, shooter);;
        return bullet;
    }
}

public class GunType
{
    public float Damage;
    public float Velocity;
    public float Range;
    public float BurstDelay;
    public int BurstAmount;
    public float BurstReload;
    public float Reload;
    public int ReloadAmount;
    public string BulletPath;
    public float Accuracy;

    /// <summary>
    /// class that stores data on gun types
    /// <para damage>the amount of damage that this gun deals</para>
    /// </summary>
    public GunType(float damage, float velocity, float range, float burstDelay, int burstAmount, float burstReload,
        float reload, int reloadAmount, float accuracy, string bulletPath)
    {
        this.Damage = damage;
        this.Velocity = velocity;
        this.Range = range;
        this.BurstAmount = burstAmount;
        this.BurstDelay = burstReload;
        this.Reload = reload;
        this.ReloadAmount = reloadAmount;
        this.BulletPath = bulletPath;
        this.Accuracy = accuracy;
    }
}