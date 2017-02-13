using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    //singleton instance
    public static ObjectPool Instance;
    //list of pooled objects
    private List<GameObject> pooledObjects;
    //object template
    public GameObject Template;

    public int InitialObjects = 100;
    public bool WillGrow = true;

    void Awake()
    {
        ObjectPool.Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        this.pooledObjects = new List<GameObject>();
        for (int i = 0; i < this.InitialObjects; i++)
        {
            GameObject obj = (GameObject)Instantiate(this.Template);
            obj.SetActive(false);
            this.pooledObjects[i] = obj;
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in this.pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        if (WillGrow)
        {
            GameObject obj = (GameObject)Instantiate(this.Template);
            this.pooledObjects.Add(obj);
            return obj;
        }
        else
        {
            return null;
        }
    }
}
