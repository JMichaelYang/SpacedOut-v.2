using UnityEngine;
using System.Collections.Generic;

public static class ObjectPool
{
    //default size for pools
    const int DEFAULT_POOL_SIZE = 3;

    //pool for a specific prefab
    class Pool
    {
        //per-item id
        private int nextId = 1;

        //list of objects
        private List<GameObject> objects;

        //the prefab that we are pooling
        private GameObject prefab;

        public Pool(GameObject prefab, int initialQty)
        {
            this.prefab = prefab;
            this.objects = new List<GameObject>(initialQty);
        }

        //spawn an object from its pool
        public GameObject Spawn(Vector3 pos, Quaternion rot)
        {
            GameObject obj;

            for (int i = 0; i < this.objects.Count; i++)
            {
                //attempt to find an inactive object
                if (!this.objects[i].activeInHierarchy)
                {
                    obj = this.objects[i];
                    obj.transform.position = pos;
                    obj.transform.rotation = rot;
                    obj.SetActive(true);
                    return obj;
                }
            }

            //if there is no new object, create a new one
            obj = (GameObject)GameObject.Instantiate(prefab, pos, rot);
            obj.name = prefab.name + " (" + (nextId++) + ")";
            obj.transform.position = pos;
            obj.transform.rotation = rot;
            this.objects.Add(obj);
            obj.SetActive(true);
            return obj;

        }

        //return an object to the inactive state
        public void Despawn(GameObject obj)
        {
            obj.SetActive(false);
        }

    }

    //dictionary of ll of our pools
    static Dictionary<GameObject, Pool> pools;

    //initialize our dictionary
    static void Init(GameObject prefab = null, int qty = DEFAULT_POOL_SIZE)
    {
        if (ObjectPool.pools == null)
        {
            ObjectPool.pools = new Dictionary<GameObject, Pool>();
        }
        if (prefab != null && ObjectPool.pools.ContainsKey(prefab) == false)
        {
            ObjectPool.pools[prefab] = new Pool(prefab, qty);
        }
    }

    //preload some of the objects for the pool
    static public void Preload(GameObject prefab, int qty = 1)
    {
        ObjectPool.Init(prefab, qty);

        //make an array to grab the objects we're about to pre-spawn.
        GameObject[] obs = new GameObject[qty];
        for (int i = 0; i < qty; i++)
        {
            Debug.Log("this is running at least");
            obs[i] = ObjectPool.Spawn(prefab, Vector3.zero, Quaternion.identity);
            ObjectPool.Despawn(obs[i]);
        }
    }

    //spawn a version of the specified game object
    static public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        ObjectPool.Init(prefab);

        return ObjectPool.pools[prefab].Spawn(pos, rot);
    }

    //despawn the passed object
    static public void Despawn(GameObject obj)
    {
        Pool pool = ObjectPool.pools[obj];
        if (pool == null)
        {
            Debug.Log("Object '" + obj.name + "' wasn't spawned from a pool. Destroying it instead.");
            GameObject.Destroy(obj);
        }
        else
        {
            ObjectPool.pools[obj].Despawn(obj);
        }
    }

}