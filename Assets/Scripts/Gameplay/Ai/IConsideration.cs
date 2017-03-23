using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConsideration
{
    /// <summary>
    /// The actual values of the consideration
    /// </summary>
    List<float> Values { get; set; }
    /// <summary>
    /// Dictionary that stores how to evaluate each consideration
    /// </summary>
    Dictionary<float, IUtilCalculator> UtilCalculators { get; set; }

    /// <summary>
    /// The default utility of the object
    /// </summary>
    float DefaultUtility { get; set; }

    /// <summary>
    /// The utility that the consideration provides
    /// </summary>
    float Utility { get; set; }

    /// <summary>
    /// The final multiplier for the Utility of the object
    /// </summary>
    float FinalMultiplier { get; set; }
}