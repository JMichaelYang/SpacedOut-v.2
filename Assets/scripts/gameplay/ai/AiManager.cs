using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{	
	// Update is called once per frame
	void Update ()
    {
        Vector2 steering = Vector2.zero;
        steering += AiSeek.GetSteeringForce(this.transform);
        CommandHandler.Instance.AddCommands(new AccelerateCommand(this.gameObject, steering.x, steering.y));
	}
}

/// <summary>
/// Base class for AI behaviors
/// </summary>
public abstract class AiBehavior
{
    /// <summary>
    /// Function to get result steering force
    /// </summary>
    /// <returns>Returns the force to use</returns>
    public abstract Vector2 GetSteeringForce(Transform target);
}

#region Behaviors

public class AiSeek : AiBehavior
{
    public static override Vector2 GetSteeringForce(Transform target)
    {
        return Vector2.zero;
    }
}

#endregion Behaviors