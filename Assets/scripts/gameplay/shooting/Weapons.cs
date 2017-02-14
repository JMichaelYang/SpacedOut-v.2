using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public class Gun
{
    public float Damage { get; protected set; }
    public float Range { get; protected set; }
    public float BurstDelay { get; protected set; }
    public float BurstAmount { get; protected set; }
    public float BurstReload { get; protected set; }
    public float Reload { get; protected set; }
    public GameObject BulletTemplate { get; protected set; }

    public void LoadFromGunType(GunType gunType)
    {
        this.Damage = gunType.Damage;
        this.Range = gunType.Range;
        this.BurstDelay = gunType.BurstDelay;
        this.BurstAmount = gunType.BurstAmount;
        this.BurstReload = gunType.BurstReload;
        this.Reload = gunType.Reload;
    }

    public Gun(GunType gunType)
    {
        this.LoadFromGunType(gunType);
    }

    public GameObject ShootGun(Vector3 pos, Quaternion rot)
    {
        GameObject bullet = ObjectPool.Spawn(BulletTemplate, pos, rot);
        bullet.GetComponent<Bullet>().Activate(this.Damage, this.Range);
        return bullet;
    }
}

public struct GunType
{
    public float Damage;
    public float Range;
    public float BurstDelay;
    public float BurstAmount;
    public float BurstReload;
    public float Reload;
    public float BulletType;
}