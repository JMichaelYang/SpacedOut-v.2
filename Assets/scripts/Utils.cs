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
        foreach (int i in numbers)
        {
            maxNum += i;
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

    //rotates a 2D vector
    public static Vector2 RotateVector2(Vector2 vector, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float tx = vector.x;
        float ty = vector.y;

        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }
}
