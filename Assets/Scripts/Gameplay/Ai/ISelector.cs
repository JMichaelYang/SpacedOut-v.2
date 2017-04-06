using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectorMode
{
    HIGHEST,
    WEIGHTED,
    THRESHOLD
}

/// <summary>
/// Interface for an object to select the next action
/// </summary>
public interface ISelector
{ 
    /// <summary>
    /// Dictionary of considerations and actions to complete if they are selected
    /// </summary>
    Dictionary<IConsideration, IAction> Options { get; set; }

    /// <summary>
    /// Method to get the action to take
    /// </summary>
    /// <returns>The action that should be conducted</returns>
    IAction GetAction();
}
