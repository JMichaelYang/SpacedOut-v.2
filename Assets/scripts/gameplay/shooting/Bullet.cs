using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float duration = 0;
    private float durationTimer = 0;

	// Use this for initialization
	void Start ()
    {
        this.duration = 0;
        this.durationTimer = 0;
	}

    public void Activate()
    {
        this.reset();
        this.gameObject.SetActive(true);
    }

    protected void reset()
    {
        this.duration = 0;
        this.durationTimer = 0;
    }

	// Update is called once per frame
	void Update ()
    {
        this.durationTimer += Time.deltaTime;

        //check if bullet should be removed
        if (this.durationTimer > this.duration)
        {
            this.reset();
            this.gameObject.SetActive(false);
        }   	
	}
}
