using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Struct for components of the enemies and friends that we should keep track of
/// </summary>
[System.Serializable]
public struct ComponentsOfInterest
{
    public Transform transform;
    public Movement movement;
    public Rigidbody2D rigidBody;
    public ShipHandler handler;
}

public enum AvailableBehaviors
{
    SEEK,
    PURSUE,
    FLEE,
    SHOOT,
    RETURN_CENTER
}

public class AiManager : MonoBehaviour
{
    //the team of this ship
    private Team aiTeam;

    //local storage of components that we are interested in
    private Dictionary<GameObject, ComponentsOfInterest> friendlyComponents;
    private Dictionary<GameObject, ComponentsOfInterest> enemyComponents;
    //reference to this AI's components
    private ComponentsOfInterest aiComponents;
    //reference to this Ai's weapons
    private Weapons aiWeapons;
    //the currently focused enemy and friends
    [SerializeField]
    private GameObject enemyTarget = null;
    [SerializeField]
    private GameObject friendlyTarget = null;

    #region Delegate Behavior Stuff

    [System.Serializable]
    public delegate void AiBehavior();
    //dictionary of available behaviors
    private Dictionary<AvailableBehaviors, AiBehavior> availableBehaviors;
    //current list of behaviors in use
    [SerializeField]
    private List<AiBehavior> currentBehavior;

    //steering vector for use by delegate behaviors
    [SerializeField]
    private Vector2 steering = Vector2.zero;
    //bool to determine whether to shoot to be used by delegate behaviors
    [SerializeField]
    private bool shouldShoot = false;

    #endregion Delegate Behavior Stuff

    void Awake()
    {
        this.availableBehaviors = new Dictionary<AvailableBehaviors, AiBehavior>();
        //populate our list of available behaviors
        this.availableBehaviors.Add(AvailableBehaviors.SEEK, BehaviorSeek);
        this.availableBehaviors.Add(AvailableBehaviors.FLEE, BehaviorFlee);
        this.availableBehaviors.Add(AvailableBehaviors.PURSUE, BehaviorPursue);
        this.availableBehaviors.Add(AvailableBehaviors.SHOOT, BehaviorShoot);
        this.availableBehaviors.Add(AvailableBehaviors.RETURN_CENTER, BehaviorReturnCenter);

        //set up our behavior architecture
        this.currentBehavior = new List<AiBehavior>();
        this.steering = Vector2.zero;
        this.shouldShoot = false;

        //assign components
        this.aiComponents.transform = this.transform;
        this.aiComponents.movement = this.gameObject.GetComponent<Movement>();
        this.aiComponents.rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
        this.aiComponents.handler = this.gameObject.GetComponent<ShipHandler>();
        this.aiWeapons = this.gameObject.GetComponent<Weapons>();

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
        #region Targeting

        //if we have a target, check if it still alive, otherwise, try to acquire a target (if we can't, just leave at null)
        if (this.enemyTarget != null)
        {
            //if its dead, set our target index to null (if this fails then we probably tried to check an enemy that wasn't on the team list)
            try
            {
                if (!this.enemyComponents[this.enemyTarget].handler.IsAlive)
                {
                    this.enemyTarget = null;
                    this.ClearBehavior();
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
            if (this.enemyTarget != null)
            {
                this.AddBehavior(AvailableBehaviors.SEEK);
                this.AddBehavior(AvailableBehaviors.SHOOT);
            }
        }

        //TODO: Make it so that all Ai's on one team do not target the same target

        #endregion Targeting

        #region Steering

        //reset delegate behvaior variables
        this.shouldShoot = false;
        this.steering = Vector2.zero;

        //find current rotation
        float currentRot = this.aiComponents.transform.rotation.eulerAngles.z;

        //execute all behaviors
        for (int i = 0; i < this.currentBehavior.Count; i++) { this.currentBehavior[i](); }

        //cap our steering force
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

        #endregion Steering

        if (this.shouldShoot)
        {
            CommandHandler.Instance.AddCommands(new ShootCommand(this.aiWeapons, 0, 1));
        }
    }

    /// <summary>
    /// Function to add a behavior to the ai
    /// </summary>
    /// <param name="behavior">the behavior to be added</param>
    public void AddBehavior(AvailableBehaviors behavior)
    {
        this.currentBehavior.Add(this.availableBehaviors[behavior]);
    }
    public void ClearBehavior()
    {
        this.currentBehavior.Clear();
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

        return closestIndex < 0 ? null : this.aiTeam.EnemyShips[closestIndex];
    }

    #region Behaviors

    /// <summary>
    /// Guide the ship towards the current enemy target
    /// </summary>
    public void BehaviorSeek()
    {
        Vector2 targetHeading = this.enemyComponents[this.enemyTarget].transform.position - this.aiComponents.transform.position;
        this.steering += Utils.CapVector2(targetHeading, this.aiComponents.movement.MaxAcceleration);
    }
    /// <summary>
    /// Guide the ship away from the current enemy target
    /// </summary>
    public void BehaviorFlee()
    {
        Vector2 targetHeading = this.aiComponents.transform.position - this.enemyComponents[this.enemyTarget].transform.position;
        this.steering += Utils.CapVector2(targetHeading, this.aiComponents.movement.MaxAcceleration);
    }
    /// <summary>
    /// Guide the ship towards the current enemy target accounting for its velocity
    /// </summary>
    public void BehaviorPursue()
    {
        Transform enemyTransform = this.enemyComponents[this.enemyTarget].transform;
        float t = (enemyTransform.position - this.aiComponents.transform.position).magnitude / this.aiComponents.movement.MaxVelocity;
        this.steering += Utils.CapVector2((Vector2)enemyTransform.position + this.enemyComponents[this.enemyTarget].rigidBody.velocity * t - (Vector2)this.aiComponents.transform.position,
            this.aiComponents.movement.MaxAcceleration);
    }
    /// <summary>
    /// Determine whether the ship should attempt to shoot the target
    /// </summary>
    public void BehaviorShoot()
    {
        //find out if target is in range
        Vector2 vectorBetween = this.enemyComponents[this.enemyTarget].transform.position - this.aiComponents.transform.position;
        float distanceBetweenSqr = vectorBetween.sqrMagnitude;
        float rangeSqr = this.aiWeapons.MaxRange * this.aiWeapons.MaxRange;

        if (distanceBetweenSqr < rangeSqr)
        {
            //check if target is within acceptable field in front of ai
            if (Mathf.Abs(Utils.FindAngleDifference(Mathf.Atan2(vectorBetween.y, vectorBetween.x) * Mathf.Rad2Deg - 90f, this.aiComponents.transform.eulerAngles.z)) < this.aiWeapons.MaxSpread)
            {
                this.shouldShoot = true;
            }
        }
    }
    /// <summary>
    /// Guide the ship towards the center of the arena (0, 0)
    /// </summary>
    public void BehaviorReturnCenter()
    {
        Vector2 targetHeading = (Vector2)this.aiComponents.transform.position - Vector2.zero;
        this.steering += Utils.CapVector2(targetHeading, this.aiComponents.movement.MaxAcceleration);
    }

    #endregion Behaviors
}

/// <summary>
/// Parent class for all AI behavior functions
/// </summary>
[System.Obsolete("Now using delegates instead of classes; this has been left in case we want to revert to classes for behaviors...")]
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