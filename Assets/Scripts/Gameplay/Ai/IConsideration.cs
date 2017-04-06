using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for any consideration used by an object in order to make a decision
/// </summary>
public interface IConsideration
{
    /// <summary>
    /// Name of the consideration
    /// </summary>
    string Name { get; set; }
    /// <summary>
    /// The consideration's raw, unadjusted value
    /// </summary>
    float Value { get; set; }
    /// <summary>
    /// The calculator used to get the utility of this consideration
    /// </summary>
    IEvaluator Calculator { get; set; }
    /// <summary>
    /// The minimum value that the consideration can have
    /// </summary>
    float Min { get; set; }
    /// <summary>
    /// The maximum value that the consideration can have
    /// </summary>
    float Max { get; set; }
    /// <summary>
    /// The adjustment to be made to the value before it is evaluated
    /// </summary>
    float InAdj { get; set; }
    /// <summary>
    /// The adjustment to be made to the value after it is evaluated
    /// </summary>
    float OutAdj { get; set; }
    /// <summary>
    /// Whether to invert the final utility
    /// </summary>
    bool Inverted { get; set; }
    /// <summary>
    /// The final number to multiply the consideration by
    /// </summary>
    float FinalMultiplier { get; set; }

    /// <summary>
    /// Get the utility of this consideration using the calculator
    /// </summary>
    /// <returns>The utility of this consideration</returns>
    float GetUtility();
    /// <summary>
    /// Set the value of this consideration
    /// </summary>
    void SetValue(float value);
}

/// <summary>
/// A class representing a basic consideration
/// </summary>
public class ConsiderationBase : IConsideration
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float Min { get; set; }
    public float Max { get; set; }
    public IEvaluator Calculator { get; set; }
    public float InAdj { get; set; }
    public float OutAdj { get; set; }
    public bool Inverted { get; set; }
    public float FinalMultiplier { get; set; }

    /// <summary>
    /// Constructor for a new basic consideration
    /// </summary>
    /// <param name="name">the name of the consideration</param>
    /// <param name="min">the minimum value of the consideration</param>
    /// <param name="max">the maximum value of the consideration</param>
    /// <param name="value">the initial value of the consideration</param>
    public ConsiderationBase(string name, float min, float max, IEvaluator calculator, float value = 0f)
    {
        this.Name = name;
        this.Min = min;
        this.Max = max;
        this.Calculator = calculator;
        this.InAdj = 0f;
        this.OutAdj = 0f;
        this.Inverted = false;
        this.FinalMultiplier = 1f;
        this.Value = value;
    }
    /// <summary>
    /// Constructor for a new basic consideration
    /// </summary>
    /// <param name="name">the name of the consideration</param>
    /// <param name="min">the minimum value of the consideration</param>
    /// <param name="max">the maximum value of the consideration</param>
    /// <param name="inAdj">the adjustment to be made to the value initally</param>
    /// <param name="outAdj">the adjustment to be made to the final value</param>
    /// <param name="value">the initial value of the consideration</param>
    public ConsiderationBase(string name, float min, float max, IEvaluator calculator, float inAdj, float outAdj, bool inverted, float finalMultiplier, float value = 0f)
    {
        this.Name = name;
        this.Min = min;
        this.Max = max;
        this.Calculator = calculator;
        this.InAdj = InAdj;
        this.OutAdj = outAdj;
        this.Inverted = inverted;
        this.FinalMultiplier = finalMultiplier;
        this.Value = value;
    }
    
    public float GetUtility()
    {
        return this.Calculator.GetValue(this.Value, this.Min, this.Max, this.InAdj, this.OutAdj, this.Inverted);
    }
    public void SetValue(float value)
    {
        this.Value = value;
    }
}