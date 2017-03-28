using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for an object to select the next action
/// </summary>
public interface ISelector
{ 
    /// <summary>
    /// Dictionary of consideratioins and actions to complete if they are selected
    /// </summary>
    Dictionary<IConsideration, IAction> Options { get; set; }

    /// <summary>
    /// Method to get the action to take
    /// </summary>
    /// <returns>The action that should be conducted</returns>
    IAction GetAction();
}
