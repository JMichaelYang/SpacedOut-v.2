using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    //strength and rate of decrease of current shake
    private float strength = 0f;
    private float decrease = 0f;

    //adjustment vector
    private Vector3 adjust = Vector3.zero;

    // Use this for initialization
    void Awake()
    {
        this.strength = 0f;
        this.decrease = 0f;
    }

    void Start()
    {
        this.adjust = Vector3.zero;
        this.transform.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    //TODO: Add in between object between camera and player, change zoom to use that object
    void Update()
    {
        if (this.strength < 0.01f)
        {
            this.strength = 0f;
            this.decrease = 0f;
            this.adjust = Vector3.zero;
        }
        else
        {
            this.strength /= this.decrease;
        }

        if (this.strength == 0f)
        {
            if (this.transform.localPosition != Vector3.zero)
            {
                this.transform.localPosition = Vector3.zero;
                this.adjust = Vector3.zero;
            }
        }
        else if (this.strength >= 0.01f)
        {
            this.adjust.x += Random.Range(-this.strength, this.strength);
            this.adjust.y += Random.Range(-this.strength, this.strength);
            this.adjust.z = 0f;
            if (this.adjust.x < -this.strength)
            {
                this.adjust.x = -this.strength;
            }
            else if (this.adjust.x > this.strength)
            {
                this.adjust.x = this.strength;
            }
            if (this.adjust.y < -this.strength)
            {
                this.adjust.y = -this.strength;
            }
            else if (this.adjust.y > this.strength)
            {
                this.adjust.y = this.strength;
            }
            //Mathf.Clamp(this.adjust.x, -this.strength, this.strength);
            //Mathf.Clamp(this.adjust.y, -this.strength, this.strength);

            this.transform.localPosition = this.adjust;
        }
    }

    /// <summary>
    /// Function to start a camera shake
    /// </summary>
    /// <param name="strength">The magnitude with which to shake the camera</param>
    public void StartShake(float strength, float decrease)
    {
        this.strength = strength;
        this.decrease = decrease;
    }
}
