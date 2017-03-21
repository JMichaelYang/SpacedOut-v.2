using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTypes
{
    public static readonly GunType DebugGun1 = new GunType(15f, 30f, 6f, 6f, 0.1f, 0.5f, 2f, 5, 100, "bullet");
    public static readonly GunType DebugGun2 = new GunType(25f, 25f, 8f, 2f, 0.2f, 1f, 3f, 5, 20, "rocket");
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