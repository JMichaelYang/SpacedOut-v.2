using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOption
{
    IConsideration Consideration { get; set; }
    IAction Action { get; set; }
    float Inertia { get; set; }
}
