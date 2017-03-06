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

    #region Event Registration

    void OnEnable()
    {
        //register event
        GameEventHandler.OnWeaponShoot += this.StartShakeEvent;
    }
    void OnDisable()
    {
        //de-register event
        GameEventHandler.OnWeaponShoot -= this.StartShakeEvent;
    }
    private void StartShakeEvent(object sender, WeaponShootEventArgs e)
    {
        this.StartShake(GameSettings.ShotShake, GameSettings.ShakeDecrease);
    }

    #endregion Event Registration

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

    //function to shake the camera
    void Shake()
    {
        if (this.strength < 0.01f)
        {
            this.strength = 0f;
            this.decrease = 0f;
            this.transform.localPosition = Vector3.zero;
            this.adjust = Vector3.zero;
            CancelInvoke("Shake");
        }
        else
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

            this.strength /= this.decrease;
        }
    }

    /// <summary>
    /// Function to start a camera shake
    /// </summary>
    /// <param name="strength">The magnitude with which to shake the camera</param>
    private void StartShake(float strength, float decrease)
    {
        this.strength = strength;
        this.decrease = decrease;

        InvokeRepeating("Shake", 0f, 0.03f);
    }
}
