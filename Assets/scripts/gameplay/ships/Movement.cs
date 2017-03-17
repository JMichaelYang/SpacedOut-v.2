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

    public void SetStatistics(ShipType shipType, EngineType engine)
    {
        this.MaxRotationalVelocity = shipType.RotAccel;

        this.MaxAcceleration = engine.MaxAccel;
        this.MaxVelocity = engine.MaxVelocity;
    }

    void FixedUpdate()
    {
        //add accumulated forces
        this.acceleration = Utils.CapVector2(this.acceleration, this.MaxAcceleration);
        this.rotation = Mathf.Clamp(this.rotation, -this.MaxRotationalVelocity, this.MaxRotationalVelocity);

        this.rigidBody.AddForce(this.acceleration, ForceMode2D.Impulse);
        this.rigidBody.MoveRotation(this.rigidBody.rotation + this.rotation);
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
                Vector2 inertialForce = Vector2.zero;

                // convert Rigidbody2D velocity to local space in terms of the transform
                // take negative of the x component
                // convert this back to world space
                inertialForce = this.bodyTransform.InverseTransformDirection(this.rigidBody.velocity);
                inertialForce.x *= -1f;
                inertialForce.y = 0f;
                inertialForce = this.bodyTransform.TransformDirection(inertialForce);

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
