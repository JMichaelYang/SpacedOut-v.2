using UnityEngine;

/// <summary>
/// The class for anything that uses input to mvoe
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    /// <summary>
    /// Rigidbody of the object
    /// </summary>
    private Rigidbody2D rigidBody;
    /// <summary>
    /// Transform of the object
    /// </summary>
    private new Transform transform;

    /// <summary>
    /// Current force to be added during the FixedUpdate
    /// </summary>
    private Vector2 deltaForce;
    /// <summary>
    /// Rotation to be applied during the FixedUpdate
    /// </summary>
    private float deltaRot;

    /// <summary>
    /// The maximum velocity of this object
    /// </summary>
    public float MaxVel { get; protected set; }
    /// <summary>
    /// The maximum thrust of this object
    /// </summary>
    public float Thrust { get; protected set; }
    /// <summary>
    /// The rotational velocity of this object
    /// </summary>
    public float RotVel { get; protected set; }

    /// <summary>
    /// Should we force the object to move in the direction that it is facing in, and by how much
    /// </summary>
    private bool DampenInertia;
    private float DampeningMultiplier;

    /// <summary>
    /// Find the components for this movement object
    /// </summary>
    void Awake()
    {
        //get the Rigidbody and Transform for his object
        try { this.rigidBody = this.gameObject.GetComponent<Rigidbody2D>(); }
        catch { Debug.Log("Could not find RigidBody2D component of " + this.gameObject.ToString()); }
        try { this.transform = this.gameObject.GetComponent<Transform>(); }
        catch { Debug.Log("Could not find Transform component of " + this.gameObject.ToString()); }

        //set the current acceleration and rotation delta of this object to 0
        this.deltaForce = Vector2.zero;
        this.deltaRot = 0;
    }

    /// <summary>
    /// Set the statistics to be used for this movement component
    /// </summary>
    /// <param name="maxVel">The maximum velocity that this object should experience</param>
    /// <param name="thrust">The thrust that this object should experience</param>
    /// <param name="rotVel">The maximum rotatonal velocity that this object should experience</param>
    /// <param name="dampeningMultiplier">The amount to which we should dampen this object's inertia</param>
    /// <param name="dampenInteria">Whether or not we should dampen this object's inertia</param>
    public void SetStatistics(float maxVel, float thrust, float rotVel, float dampeningMultiplier, bool dampenInteria = true)
    {
        this.MaxVel = maxVel;
        this.Thrust = thrust;
        this.RotVel = rotVel;

        this.DampenInertia = dampenInteria;
        this.DampeningMultiplier = dampeningMultiplier;
    }

    void FixedUpdate()
    {
        #region Update Forces

        if (this.deltaForce.x != 0f || this.deltaForce.y != 0f)
        {
            //add accumulated forces and add them to this rigid body
            this.deltaForce = Utils.CapVector2(this.deltaForce, this.Thrust);
            this.rigidBody.AddForce(this.deltaForce, ForceMode2D.Impulse);

            //reset forces
            this.deltaForce = Vector2.zero;

            //limit velocity
            float originalMagnitude = this.rigidBody.velocity.magnitude;
            if (originalMagnitude > this.MaxVel)
            {
                this.rigidBody.velocity *= this.MaxVel / originalMagnitude;
                originalMagnitude = this.MaxVel;
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
                    inertialForce = this.transform.InverseTransformDirection(this.rigidBody.velocity);
                    inertialForce.x *= -1f;
                    inertialForce.y = 0f;
                    inertialForce = this.transform.TransformDirection(inertialForce);

                    this.rigidBody.velocity += inertialForce * this.DampeningMultiplier;
                    this.rigidBody.velocity *= originalMagnitude / this.rigidBody.velocity.magnitude;
                }
            }
        }

        #endregion Update Forces

        #region Update Rotation

        if (this.deltaRot != 0f)
        {
            this.deltaRot = Mathf.Clamp(this.deltaRot, -this.RotVel, this.RotVel);
            this.rigidBody.MoveRotation(this.rigidBody.rotation + this.deltaRot);
            this.deltaRot = 0f;
        }

        #endregion Update Rotation
    }

    /// <summary>
    /// Accelerate the object by a Vector2
    /// </summary>
    /// <param name="xAxis">the x component of the acceleration</param>
    /// <param name="yAxis">the y component of the acceleration</param>
    /// <param name="limit">whether to cap the thrust applied</param>
    public void ApplyThrust(float xAxis, float yAxis, bool limit = true)
    {
        Vector2 accel = new Vector2(xAxis, yAxis);
        if (limit) { accel = Utils.CapVector2(new Vector2(xAxis, yAxis), this.Thrust); }
        this.deltaForce += accel;
    }
    /// <summary>
    /// Accelerate the object in the direction of its rotation
    /// </summary>
    /// <param name="magnitude">the magnitude to accelerate the object by</param>
    /// <param name="limit">whether to cap the acceleration</param>
    public void ApplyDirectionalThrust(float magnitude, bool limit = true)
    {
        Vector2 accel = this.transform.up * magnitude;
        if(limit) { accel = Utils.CapVector2(accel, this.Thrust); }
        this.deltaForce += accel;
    }

    /// <summary>
    /// Rotate the object
    /// </summary>
    /// <param name="magnitude">the amount to rotate the object by</param>
    /// <param name="limit">whether to limit the rotation</param>
    public void Rotate(float magnitude, bool limit = true)
    {
        this.deltaRot += limit ? Mathf.Clamp(magnitude, -this.RotVel, this.RotVel) : magnitude;
    }
}
