using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUtilCalculator
{
    bool Inverted { get; set; }

    /// <summary>
    /// Function to get the utility from a value
    /// </summary>
    /// <param name="valueIn">the input value</param>
    /// <param name="min">the input value's minimum range</param>
    /// <param name="max">the input value's maximum range</param>
    /// <param name="inAdj">the adjustment that should be applied to the input value</param>
    /// <param name="outAdj">the adjustment that should be applied to the output value</param>
    /// <returns>The utility that results from the value (from 0 to 1)</returns>
    float GetValue(float valueIn, float min, float max, float inAdj, float outAdj);
}

public abstract class UtilCalculator : IUtilCalculator
{
    public bool Inverted { get; set; }

    public UtilCalculator(bool inverted = false)
    {
        this.Inverted = inverted;
    }

    public abstract float GetValue(float valueIn, float min, float max, float inAdj, float outAdj);
}

public class LinearUtilCalculator : UtilCalculator
{
    public override float GetValue(float valueIn, float min, float max, float inAdj, float outAdj)
    {
        float valueOut = valueIn;
        valueOut += (max - min) * inAdj;

        valueOut = Utils.ConvertScale(valueOut, min, max, 0f, 1f);
        if(this.Inverted) { valueOut = 1f - valueOut; }

        valueOut += outAdj;
        return Mathf.Clamp01(valueOut);
    }
}

public class ExponentialUtilCalculator : UtilCalculator
{
    public float Exponent { get; set; }

    public ExponentialUtilCalculator(bool inverted, float exponent)
        : base(inverted)
    {
        this.Exponent = exponent;
    }

    public override float GetValue(float valueIn, float min, float max, float inAdj, float outAdj)
    {
        float valueOut = valueIn;
        valueOut += (max - min) * inAdj;

        valueOut = Utils.ConvertScale(valueOut, min, max, 0f, 1f);
        valueOut = Mathf.Pow(valueOut, this.Exponent);
        if (this.Inverted) { valueOut = 1 - valueOut; }

        valueOut += outAdj;
        return Mathf.Clamp01(valueOut);
    }
}