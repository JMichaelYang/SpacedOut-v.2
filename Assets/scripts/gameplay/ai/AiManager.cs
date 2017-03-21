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
        public ShipHandler handler;
    }

    //the team of this ship
    private Team aiTeam;

    //current list of behaviors in use
    private List<AiMoveBehavior> currentMoveBehavior;

    //local storage of components that we are interested in
    private Dictionary<GameObject, ComponentsOfInterest> friendlyComponents;
    private Dictionary<GameObject, ComponentsOfInterest> enemyComponents;
    //the currently focused enemy and friends
    [SerializeField]
    private GameObject enemyTarget = null;
    private GameObject friendlyTarget;

    //reference to this AI's components
    private ComponentsOfInterest aiComponents;

    void Awake()
    {
        this.currentMoveBehavior = new List<AiMoveBehavior>();
        
        //assign components
        this.aiComponents.transform = this.transform;
        this.aiComponents.movement = this.gameObject.GetComponent<Movement>();
        this.aiComponents.rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
        this.aiComponents.handler = this.gameObject.GetComponent<ShipHandler>();

        //set initial targets to null (to be found later)
        enemyTarget = null;
        friendlyTarget = null;
    }

    void Start()
    {
        //TODO: Test code, please remove
        //this.AddBehavior(new AiFleeBehabior(player.transform, this.aiTransform, this.movement.MaxAcceleration));
        //this.AddBehavior(new AiPursueBehavior(player.transform, this.aiTransform, player.GetComponent<Rigidbody2D>(), player.GetComponent<Movement>().MaxVelocity, this.movement.MaxAcceleration));
    }

    public void SetTeam(Team team)
    {
        //set our team to the new team and create our dictionaries
        this.aiTeam = team;
        this.friendlyComponents = new Dictionary<GameObject, ComponentsOfInterest>();
        this.enemyComponents = new Dictionary<GameObject, ComponentsOfInterest>();

        //add the friendly team's components to our dictionary
        for (int i = 0; i < team.FriendlyShips.Count; i++)
        {
            GameObject shipObject = team.FriendlyShips[i];

            ComponentsOfInterest components = new ComponentsOfInterest();
            components.transform = shipObject.transform;
            components.movement = shipObject.GetComponent<Movement>();
            components.rigidBody = shipObject.GetComponent<Rigidbody2D>();
            components.handler = shipObject.GetComponent<ShipHandler>();

            this.friendlyComponents.Add(shipObject, components);
        }

        //add the enemy team's components to our dictionary
        for (int i = 0; i < team.EnemyShips.Count; i++)
        {
            GameObject shipObject = team.EnemyShips[i];

            ComponentsOfInterest components = new ComponentsOfInterest();
            components.transform = shipObject.transform;
            components.movement = shipObject.GetComponent<Movement>();
            components.rigidBody = shipObject.GetComponent<Rigidbody2D>();
            components.handler = shipObject.GetComponent<ShipHandler>();

            this.enemyComponents.Add(shipObject, components);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if we have a target, check if it still alive, otherwise, try to acquire a target (if we can't, just leave at null)
        if (this.enemyTarget != null)
        {
            //if its dead, set our target index to null (if this fails then we probably tried to check an enemy that wasn't on the team list)
            try
            {
                if (!this.enemyComponents[this.enemyTarget].handler.IsAlive)
                {
                    this.enemyTarget = null;
                }
            }
            catch
            {
                Debug.Log("Ai target alive check failed, probably trying to check an object not on a team somehow");
            }
        }
        else if(this.aiTeam != null && this.aiTeam.EnemyShips != null && this.aiTeam.EnemyShips.Count > 0)
        {
            this.enemyTarget = this.acquireTargetObject();
            this.AddBehavior(new AiSeekBehavior(this.enemyComponents[this.acquireTargetObject()].transform, this.aiComponents.transform, this.aiComponents.movement.MaxAcceleration));
        }

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

    /// <summary>
    /// Acquire the closest target among the list of enemies from our team
    /// </summary>
    /// <returns>The index of the closest target</returns>
    private GameObject acquireTargetObject()
    {
        //the closest target's distance squared
        float closestSqr = -1;
        float distance = 0;
        int closestIndex = -1;

        for (int i = 0; i < this.aiTeam.EnemyShips.Count; i++)
        {
            //make sure the target is alive
            if (this.enemyComponents[this.aiTeam.EnemyShips[i]].handler.IsAlive)
            {
                //get the distance between the enemy and the ai
                distance = (this.enemyComponents[this.aiTeam.EnemyShips[i]].transform.position - this.aiComponents.transform.position).sqrMagnitude;

                if (distance < closestSqr || closestSqr == -1f)
                {
                    closestIndex = i;
                    closestSqr = distance;
                }
            }
        }

        return this.aiTeam.EnemyShips[closestIndex];
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