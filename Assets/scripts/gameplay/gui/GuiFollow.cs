using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiFollow : MonoBehaviour
{
    //object to follow
    public GameObject FollowObject;

	// Use this for initialization
	void Start ()
    {
        this.FollowObject = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.position = this.FollowObject.transform.position;	
	}
}
