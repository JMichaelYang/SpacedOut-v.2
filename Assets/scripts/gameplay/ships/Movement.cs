using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    //rigid body to use
    private Rigidbody2D rigidBody;
    //transform to use
    private Transform bodyTransform;

    //force to add every FixedUpdate
    private Vector2 acceleration;
    //rotation to add every FixedUpdate
    private float rotation;

    //should we force object to move in the direction that it is facing
    private bool DampenInertia = true;
    private float DampeningMultiplier = 1f;

    //changes acceleration of object
    public float AccelerationMultiplier = 1f;
    public float RotationMultiplier = 1f;

    //movement speed cap
    public float MaxVelocity = 1f;
    public float MaxRotationalVelocity = 1f;
    //acceleration cap
    public float MaxAcceleration = 1f;

    // Use this for initialization
    void Awake()
    {
        try { this.rigidBody = this.gameObject.GetComponent<Rigidbody2D>(); }
        catch { Debug.Log("Could not find RigidBody2D component of " + this.gameObject.ToString()); }
        try { this.bodyTransform = this.transform; }
        catch { Debug.Log("Could not find Transform component of " + this.gameObject.ToString()); }

        this.acceleration = Vector2.zero;
        this.rotation = 0;

        this.DampenInertia = GameSettings.DampenInteria;
        this.DampeningMultiplier = GameSettings.DampeningMultiplier;
    }

    void FixedUpdate()
    {
        //add accumulated forces
        this.rigidBody.AddForce(this.acceleration * this.AccelerationMultiplier, ForceMode2D.Impulse);
        this.rigidBody.MoveRotation(this.rigidBody.rotation + (this.rotation * this.RotationMultiplier));
        //reset forces
        this.acceleration = Vector2.zero;
        this.rotation = 0;

        float originalMagnitude = this.rigidBody.velocity.magnitude;

        //limit velocity
        if (originalMagnitude > this.MaxVelocity)
        {
            this.rigidBody.velocity *= this.MaxVelocity / originalMagnitude;
            originalMagnitude = this.MaxVelocity;
        }

        //dampen interia
        if (this.DampenInertia)
        {
            if (originalMagnitude != 0)
            {
                Vector2 inertialForce = ((Vector2)this.transform.up * this.rigidBody.velocity.magnitude) - this.rigidBody.velocity;
                this.rigidBody.velocity += inertialForce * this.DampeningMultiplier;

                this.rigidBody.velocity *= originalMagnitude / this.rigidBody.velocity.magnitude;
            }
        }
    }

    /// <summary>
    /// Accelerate the object by a Vector2
    /// </summary>
    /// <param name="xAxis">the x component of the acceleration</param>
    /// <param name="yAxis">the y component of the acceleration</param>
    public void LinearAccelerate(float xAxis, float yAxis, bool limit = true)
    {
        Vector2 accel = new Vector2(xAxis, yAxis);
        if (limit) { accel = Utils.CapVector2(xAxis, yAxis, this.MaxAcceleration); }
        this.acceleration.x += accel.x;
        this.acceleration.y += accel.y;
    }
    /// <summary>
    /// Accelerate the object in the direction of its rotation
    /// </summary>
    /// <param name="magnitude">the magnitude to accelerate the object by</param>
    public void Accelerate(float magnitude, bool limit = true)
    {
        float mag = magnitude;
        if (limit) { Mathf.Clamp(mag, -this.MaxAcceleration, this.MaxAcceleration); }

        Vector2 accel = this.bodyTransform.up * mag;
        //float xAccel = Mathf.Cos((this.rigidBody.rotation + 90f) * Mathf.Deg2Rad) * mag;
        //float yAccel = Mathf.Sin((this.rigidBody.rotation + 90f) * Mathf.Deg2Rad) * mag;

        this.acceleration.x += accel.x;
        this.acceleration.y += accel.y;
    }

    /// <summary>
    /// Rotate the object
    /// </summary>
    /// <param name="magnitude">the amount to rotate the object by</param>
    public void Rotate(float magnitude)
    {
        this.rotation += Mathf.Clamp(magnitude, -this.MaxRotationalVelocity, this.MaxRotationalVelocity);
    }
}
