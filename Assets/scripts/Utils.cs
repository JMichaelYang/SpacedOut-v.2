using UnityEngine;

public class Utils : MonoBehaviour
{
    //returns a random Vector3 between the min and max
    public static Vector3 RandomVector3(Vector3 min, Vector3 max)
    {
        return Utils.RandomVector3(min.x, max.x, min.y, max.y, min.z, max.z);
    }
    public static Vector3 RandomVector3(float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
    {
        return new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), Random.Range(zMin, zMax));
    }

    //casts a number from one range to another retaining ratio
    public static float ConvertScale(float num, float inScaleMin, float inScaleMax, float outScaleMin, float outScaleMax)
    {
        return (((num - inScaleMin) * (outScaleMax - outScaleMin)) / (inScaleMax - inScaleMin)) + outScaleMin;
    }

    /// <summary>
    /// Returns a random index for a weight based on an array of weights
    /// </summary>
    /// <param name="numbers">The different weights</param>
    /// <returns>The index of the randomly selected weight</returns>
    public static int WeightedRandom(params int[] numbers)
    {
        int maxNum = 0;
        for (int i = 0; i < numbers.Length; i++)
        {
            maxNum += numbers[i];
        }

        float genNum = Random.Range(0f, maxNum);

        int result, total = 0;
        for (result = 0; result < numbers.Length; result++)
        {
            total += numbers[result];
            if (total >= genNum)
            {
                break;
            }
        }

        return result;
    }
    /// <summary>
    /// Generates a random z-score value
    /// </summary>
    /// <returns>The z-score value</returns>
    public static float RandomGaussianFloat()
    {
        float u, v, S = 0f;

        do
        {
            u = 2f * Random.Range(0f, 1f) - 1f;
            v = 2f * Random.Range(0f, 1f) - 1f;
            S = u * u + v * v;
        }
        while (S >= 1f || S == 0f);

        float fac = Mathf.Sqrt(-2f * Mathf.Log(S) / S);
        return u * fac;
    }
    /// <summary>
    /// Generates a random value from the normal curve within the range (3 * sigma on both sides)
    /// </summary>
    /// <param name="min">The minimum value</param>
    /// <param name="max">The maximum value</param>
    /// <returns>The random gaussian value</returns>
    public static float RandomGaussianFloat(float min, float max)
    {
        float mean = (min + max) / 2f;
        float sigma = (max - mean) / 3f;

        return Mathf.Clamp(min, max, Utils.RandomGaussianFloat() * sigma + mean);
    }

    /// <summary>
    /// Rotates a 2D vector
    /// </summary>
    /// <param name="vector">the original vector to be rotated</param>
    /// <param name="degrees">the number of degrees that it is to be rotated by</param>
    /// <returns>the rotated vector</returns>
    public static Vector2 RotateVector2(Vector2 vector, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float tx = vector.x;
        float ty = vector.y;

        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }
    /// <summary>
    /// Limits a vector's magnitude
    /// </summary>
    /// <param name="vector">the vector to be limited</param>
    /// <param name="maxMag">the magnitude to limit the vector to</param>
    /// <returns>The limited vector</returns>
    public static Vector2 CapVector2(Vector2 vector, float maxMag)
    {
        if (vector.sqrMagnitude > maxMag * maxMag)
        {
            vector *= maxMag / vector.magnitude;
        }

        return vector;
    }
    /// <summary>
    /// Limits a vector's magnitude
    /// </summary>
    /// <param name="x">the x component of the input vector</param>
    /// <param name="y">the y component of the input vector</param>
    /// <param name="maxMag">the magnitude that the vector should be limited to</param>
    /// <returns>The limited vector</returns>
    public static Vector2 CapVector2(float x, float y, float maxMag)
    {
        return Utils.CapVector2(new Vector2(x, y), maxMag);
    }

    /// <summary>
    /// Get the corresponding unit vector for an angle
    /// </summary>
    /// <param name="angle">the angle from which to derive the vector in degrees</param>
    /// <returns>The resulting vector</returns>
    public static Vector2 GetUnitVectorFromAngle(float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Rad2Deg), Mathf.Sin(angle * Mathf.Rad2Deg));
    }

    /// <summary>
    /// Finds the smallest angle between two angles
    /// </summary>
    /// <param name="angle1">the first angle to be compared</param>
    /// <param name="angle2">the second angle to be compared</param>
    /// <returns>The smallest angle</returns>
    public static float FindAngleDifference(float angle1, float angle2)
    {
        float diff = (angle2 - angle1 + 180f) % 360 - 180f;
        return diff < -180f ? diff + 360f : diff;
    }
}
