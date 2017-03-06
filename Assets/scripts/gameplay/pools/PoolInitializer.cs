using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class for initializing the pools that need to be initialized (different pools)
/// </summary>
public class PoolInitializer : MonoBehaviour
{
    public GameObject DebugBullet = null;
    public int NumBullets = 0;
    public GameObject Explosion = null;
    public int NumExplosions = 0;

	// Use this for initialization
	void Start ()
    {
        //if a debug bullet object exists, preload a pool
        if (DebugBullet != null)
        {
            ObjectPool.Preload(this.DebugBullet, this.NumBullets);
        }
        if (Explosion != null)
        {
            ObjectPool.Preload(this.Explosion, this.NumExplosions);
        }
	}
}
