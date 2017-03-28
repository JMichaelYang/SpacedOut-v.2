using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface representing the items that make operations upon others
/// </summary>
public interface IUtilCalculator
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
/// Struct for a calculator that uses a linear function for evaluation
/// </summary>
public struct UtilCalculatorLinear : IUtilCalculator
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
/// Struct for a calculator that uses an exponential function for evaluation
/// </summary>
public struct UtilCalculatorExponential : IUtilCalculator
{
    public float Exponent { get; set; }

    public UtilCalculatorExponential(float exponent)
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