using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class for initializing the pools that need to be initialized (different pools)
/// </summary>
public class PoolInitializer : MonoBehaviour
{
    public GameObject debugBullet = null;

	// Use this for initialization
	void Start ()
    {
        //if a debug bullet object exists, preload a pool
        if (debugBullet != null)
        {
            ObjectPool.Preload(debugBullet, 100);
        }
	}

    // Update is called once per frame
    void Update() { }
}
