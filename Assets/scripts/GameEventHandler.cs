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
    public Bullet Shot;
    public Collider2D HitCollider;
    public BulletHitEventArgs(Bullet shot, Collider2D hitCollider)
    {
        this.Shot = shot;
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