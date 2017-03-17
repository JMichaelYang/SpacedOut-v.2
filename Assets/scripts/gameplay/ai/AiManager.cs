using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{
    public struct ComponentsOfInterest
    {
        public Transform transform;
        public Movement movement;
        public Rigidbody2D rigidBody;
    }

    //the team of this ship
    private Team aiTeam;

    //current list of behaviors in use
    private List<AiMoveBehavior> currentMoveBehavior;

    //local storage of components that we are interested in
    private Dictionary<GameObject, ComponentsOfInterest> friendlyComponents;
    private Dictionary<GameObject, ComponentsOfInterest> enemyComponents;
    //the currently focused enemy and friends
    private int enemyTarget;
    private int friendlyTarget;

    //reference to this AI's components
    private ComponentsOfInterest aiComponents;

    void Awake()
    {
        this.currentMoveBehavior = new List<AiMoveBehavior>();
        
        //assign components
        this.aiComponents.transform = this.transform;
        this.aiComponents.movement = this.gameObject.GetComponent<Movement>();
        this.aiComponents.rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        //TODO: Test code, please remove
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        this.AddBehavior(new AiSeekBehavior(player.transform, this.aiComponents.transform, this.aiComponents.movement.MaxAcceleration));
        //this.AddBehavior(new AiFleeBehabior(player.transform, this.aiTransform, this.movement.MaxAcceleration));
        //this.AddBehavior(new AiPursueBehavior(player.transform, this.aiTransform, player.GetComponent<Rigidbody2D>(), player.GetComponent<Movement>().MaxVelocity, this.movement.MaxAcceleration));
    }

    // Update is called once per frame
    void Update()
    {
        float currentRot = this.aiComponents.transform.rotation.eulerAngles.z;

        //add up all steering forces
        Vector2 steering = Vector2.zero;
        for (int i = 0; i < this.currentMoveBehavior.Count; i++) { steering += this.currentMoveBehavior[i].GetSteeringForce(); }
        steering = Utils.CapVector2(steering, this.aiComponents.movement.MaxAcceleration);

        //get magnitude of movement in steering direction
        Vector2 heading = this.aiComponents.transform.up;
        float magnitude = heading.x * steering.x + heading.y * steering.y;

        //get a desired rotation from the steering force
        float desiredRotation = Mathf.Atan2(steering.y, steering.x) * Mathf.Rad2Deg + 90f;
        desiredRotation = Utils.FindAngleDifference(desiredRotation, currentRot);

        //add command with aggregated steering forces
        CommandHandler.Instance.AddCommands(new AccelerateCommand(this.aiComponents.movement, magnitude),
            new RotateCommand(this.aiComponents.movement, desiredRotation));
    }

    /// <summary>
    /// Function to add a behavior to the ai
    /// </summary>
    /// <param name="behavior">the behavior to be added</param>
    public void AddBehavior(AiMoveBehavior behavior)
    {
        this.currentMoveBehavior.Add(behavior);
    }
    public void ClearBehavior()
    {
        this.currentMoveBehavior.Clear();
    }

    public void ShootBehavior()
    {

    }
}

/// <summary>
/// Parent class for all AI behavior functions
/// </summary>
public abstract class AiMoveBehavior
{
    protected Transform target = null;
    protected Transform aiTransform = null;
    protected float maxAccel = 0f;

    //parent function, all AiBehaviors should be able to get a force from a target
    public abstract Vector2 GetSteeringForce(Transform target, Transform ai);
    public Vector2 GetSteeringForce(Transform target) { return this.GetSteeringForce(target, this.aiTransform); }
    public Vector2 GetSteeringForce() { return this.GetSteeringForce(this.target, this.aiTransform); }
}

#region AI Movement Behavior Classes

/// <summary>
/// Behavior that follows the exact position of a target
/// </summary>
public class AiSeekBehavior : AiMoveBehavior
{
    public AiSeekBehavior(Transform target, Transform aiTransform, float maxAccel)
    {
        this.target = target;
        this.aiTransform = aiTransform;
        this.maxAccel = maxAccel;
    }

    public override Vector2 GetSteeringForce(Transform target, Transform ai)
    {
        return Utils.CapVector2(target.position - ai.position, this.maxAccel);
    }
}
public class AiSeekPointBehavior : AiMoveBehavior
{
    protected Vector2 targetPoint;

    public AiSeekPointBehavior(Vector2 target, Transform aiTransform, float maxAccel)
    {
        this.targetPoint = target;
        this.aiTransform = aiTransform;
        this.maxAccel = maxAccel;
        this.target = null;
    }
    
    public override Vector2 GetSteeringForce(Transform target, Transform ai)
    {
        return Utils.CapVector2((Vector3)this.targetPoint - ai.position, this.maxAccel);
    }
}
/// <summary>
/// Behavior that runs away from the target
/// </summary>
public class AiFleeBehabior : AiMoveBehavior
{
    public AiFleeBehabior(Transform target, Transform aiTransform, float maxAccel)
    {
        this.target = target;
        this.aiTransform = aiTransform;
        this.maxAccel = maxAccel;
    }

    public override Vector2 GetSteeringForce(Transform target, Transform ai)
    {
        return Utils.CapVector2(ai.position - target.position, this.maxAccel);
    }
}
/// <summary>
/// Behavior that follows the future position of a target
/// </summary>
public class AiPursueBehavior : AiMoveBehavior
{
    protected Rigidbody2D targetBody;
    protected float aiMaxSpeed;

    public AiPursueBehavior(Transform target, Transform aiTransform, Rigidbody2D targetBody, float aiMaxSpeed, float maxAccel)
    {
        this.target = target;
        this.aiTransform = aiTransform;

        this.targetBody = targetBody;
        this.aiMaxSpeed = aiMaxSpeed;
        this.maxAccel = maxAccel;
    }

    public override Vector2 GetSteeringForce(Transform target, Transform ai)
    {
        try
        {
            //determine how many frames it will take to get to the target
            float t = (target.position - ai.position).magnitude / this.aiMaxSpeed;
            return Utils.CapVector2((Vector2)target.position + this.targetBody.velocity * t - (Vector2)ai.position, maxAccel);
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.Message);
            return Vector2.zero;
        }
    }
}

#endregion AI Movement Behavior Classes