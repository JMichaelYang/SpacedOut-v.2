using UnityEngine;
using System.Collections;

public class ParticleStarSpawner : MonoBehaviour
{
    //this spawner's current transform
    private Transform thisTransform = null;
    private Transform followTransform = null;

    // Use this for initialization
    void Start()
    {
        this.thisTransform = this.transform;
        this.followTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.thisTransform.position = new Vector3(this.followTransform.position.x, this.followTransform.position.y, this.thisTransform.position.z);
    }
}