using System;
using UnityEngine;

public static class GameEventHandler
{
    public static EventHandler<BulletHitEventArgs> OnBulletHit;
    public static EventHandler<WeaponShootEventArgs> OnWeaponShoot;
    public static EventHandler<EventArgs> OnPlayerDead;
}

public class BulletHitEventArgs : EventArgs
{
    public float ShotDamage;
    public Collider2D HitCollider;
    public BulletHitEventArgs(float shotDamage, Collider2D hitCollider)
    {
        this.ShotDamage = shotDamage;
        this.HitCollider = hitCollider;
    }
}
public class WeaponShootEventArgs : EventArgs
{
    public float Damage;
    public WeaponShootEventArgs(float damage)
    {
        this.Damage = damage;
    }
}