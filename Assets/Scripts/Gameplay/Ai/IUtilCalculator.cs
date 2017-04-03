using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface representing the items that make operations upon considerations
/// </summary>
public interface IEvaluator
{
    /// <summary>
    /// Function to get the utility from a value
    /// </summary>
    /// <param name="valueIn">the input value</param>
    /// <param name="min">the input value's minimum range</param>
    /// <param name="max">the input value's maximum range</param>
    /// <param name="inAdj">the adjustment that should be applied to the input value</param>
    /// <param name="outAdj">the adjustment that should be applied to the output value</param>
    /// <param name="inverted">whether the final value should be inverted</param>
    /// <returns>The utility that results from the value (from 0 to 1)</returns>
    float GetValue(float valueIn, float min, float max, float inAdj, float outAdj, bool inverted);
}

/// <summary>
/// Struct for an evaluator that uses a linear function for evaluation
/// </summary>
public struct EvaluatorLinear : IEvaluator
{
    public float GetValue(float valueIn, float min, float max, float inAdj, float outAdj, bool inverted)
    {
        float valueOut = valueIn + inAdj;
        
        valueOut = Utils.ConvertScale(valueOut, min, max, 0f, 1f);
        if (inverted) { valueOut = 1 - valueOut; }

        return Mathf.Clamp01(valueOut + outAdj);
    }
}

/// <summary>
/// Struct for an evaluator that uses an exponential function for evaluation
/// </summary>
public struct EvaluatorExponential : IEvaluator
{
    public float Exponent { get; set; }

    /// <summary>
    /// An exponential evaluator
    /// </summary>
    /// <param name="exponent">the exponent of the function (0 - 1 will result in decreasing urgency, 1 and higher will result in increasing)</param>
    public EvaluatorExponential(float exponent)
    {
        this.Exponent = exponent;
    }

    public float GetValue(float valueIn, float min, float max, float inAdj, float outAdj, bool inverted)
    {
        float valueOut = valueIn + inAdj;

        valueOut = Utils.ConvertScale(valueOut, min, max, 0f, 1f);
        valueOut = Mathf.Pow(valueOut, this.Exponent);
        if (inverted) { valueOut = 1 - valueOut; }

        return Mathf.Clamp01(valueOut + outAdj);
    }
}

/// <summary>
/// Struct for an evaluator that uses a step function for evaluation
/// </summary>
public struct EvaluatorStep : IEvaluator
{
    public float[] Steps { get; set; }

    /// <summary>
    /// An evaluator that uses a step function
    /// </summary>
    /// <param name="steps">The points at which to make the "steps" (should be in increasing order)</param>
    public EvaluatorStep(params float[] steps)
    {
        this.Steps = steps;
    }

    public float GetValue(float valueIn, float min, float max, float inAdj, float outAdj, bool inverted)
    {
        float valueOut = valueIn + inAdj;
        float value = -1f;

        //how large the steps are
        float step = 1 / this.Steps.Length;

        //get the index where the value belongs
        for (int i = 0; i < this.Steps.Length; i++)
        {
            if (valueOut < this.Steps[i])
            {
                value = i;
                break;
            }
        }

        //if the value is greater than all the steps, place it in the highest bracket
        if (value == -1) { value = this.Steps.Length; }

        //set the value out
        valueOut = value * step;

        if (inverted) { valueOut = 1 - valueOut; }

        return Mathf.Clamp01(valueOut + outAdj);
    }
}

/// <summary>
/// Struct for an evaluator that returns a constant value
/// </summary>
public struct EvaluatorConstant : IEvaluator
{
    public float Constant { get; set; }

    /// <summary>
    /// An evaluator that always returns the same constant
    /// </summary>
    /// <param name="constant">the constant to return (0 - 1)</param>
    public EvaluatorConstant(float constant)
    {
        this.Constant = Mathf.Clamp01(constant);
    }

    /// <summary>
    /// Function to get the value for a constant calculation
    /// </summary>
    /// <param name="valueIn">0f</param>
    /// <param name="min">0f</param>
    /// <param name="max">0f</param>
    /// <param name="inAdj">0f</param>
    /// <param name="outAdj">the degree to which we should adjust the constant</param>
    /// <param name="inverted">false</param>
    /// <returns></returns>
    public float GetValue(float valueIn, float min, float max, float inAdj, float outAdj, bool inverted)
    {
        return this.Constant + outAdj;
    }
}
