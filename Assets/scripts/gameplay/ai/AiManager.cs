using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{
    //current list of behaviors in use
    private List<AiBehavior> currentBehavior;

    //reference to GameObject components
    private Transform aiTransform;
    private Movement movement;

    void Awake()
    {
        this.currentBehavior = new List<AiBehavior>();

        //assign components
        this.aiTransform = this.transform;
        this.movement = this.gameObject.GetComponent<Movement>();
    }

    void Start()
    {
        //TODO: Test code, pleas remove
        this.addBehavior(new AiSeekBehavior(GameObject.FindGameObjectWithTag("Player").transform, this.aiTransform));
    }

    // Update is called once per frame
    void Update()
    {
        //add up all steering forces
        Vector2 steering = Vector2.zero;
        for (int i = 0; i < this.currentBehavior.Count; i++) { steering += this.currentBehavior[i].GetSteeringForce(); }

        //get a desired rotation from the steering force
        float desiredRotation = Mathf.Atan2(steering.y, steering.x) * Mathf.Rad2Deg - 90f;
        //add command with aggregated steering forces
        CommandHandler.Instance.AddCommands(new AccelerateCommand(this.movement, steering.x, steering.y),
            new RotateCommand(this.movement, desiredRotation - this.aiTransform.rotation.eulerAngles.z));
    }

    /// <summary>
    /// Function to add a behavior to the ai
    /// </summary>
    /// <param name="behavior">The behavior to be added</param>
    protected void addBehavior(AiBehavior behavior)
    {
        this.currentBehavior.Add(behavior);
    }
}

/// <summary>
/// Parent class for all AI behavior functions
/// </summary>
public abstract class AiBehavior
{
    protected Transform target;
    protected Transform aiTransform;

    //parent function, all AiBehaviors should be able to get a force from a target
    public abstract Vector2 GetSteeringForce(Transform target, Transform ai);
    public Vector2 GetSteeringForce(Transform target) { return this.GetSteeringForce(target, this.aiTransform); }
    public Vector2 GetSteeringForce() { return this.GetSteeringForce(this.target, this.aiTransform); }
}

#region AI Behavior Classes

/// <summary>
/// Behavior that follows the exact position of a target
/// </summary>
public class AiSeekBehavior : AiBehavior
{
    public AiSeekBehavior(Transform target, Transform aiTransform)
    {
        this.target = target;
        this.aiTransform = aiTransform;
    }

    public override Vector2 GetSteeringForce(Transform target, Transform ai)
    {
        Vector2 steeringForce = target.position - ai.position;
        return steeringForce;
    }
}

#endregion AI Behavior Classes