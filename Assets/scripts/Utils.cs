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
    /// <param name="vector">the original vector</param>
    /// <param name="origMax">the original magnitude of the vector</param>
    /// <param name="maxMag">the maximum magnitude of the vector</param>
    /// <returns>The capped vector</returns>
    public static Vector2 CapVector2(Vector2 vector, float origMax, float maxMag)
    {
        if (vector.sqrMagnitude > maxMag * maxMag)
        {
            vector *= maxMag / origMax;
        }

        return vector;
    }
    /// <summary>
    /// Limits a vector's magnitude
    /// </summary>
    /// <param name="vector">the vector to be limited</param>
    /// <param name="maxMag">the magnitude to limit the vector to</param>
    /// <returns>The limited vector</returns>
    public static Vector2 CapVector2(Vector2 vector, float maxMag)
    {
        return Utils.CapVector2(vector, vector.magnitude, maxMag);
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

    /// <summary>
    /// Returns a copy of the original color with certain pixels repainted
    /// </summary>
    /// <param name="original">the original texture</param>
    /// <param name="colorFrom">the pixel color to change from</param>
    /// <param name="colorTo">the pixel color to change to</param>
    /// <returns>The new recolored copy of the texture</returns>
    public static Texture2D ColorTexture2D(Texture2D original, Color colorFrom, Color colorTo)
    {
        //create a new Texture2D copy
        Texture2D newTex = new Texture2D(original.width, original.height);
        newTex.filterMode = original.filterMode;
        newTex.wrapMode = original.wrapMode;
        newTex.name = original.name;

        for (int y = 0; y < newTex.height; y++)
        {
            for(int x = 0; x < newTex.width; x++)
            {
                //replace pixels that need to be replaced
                if (original.GetPixel(x, y) == colorFrom)
                {
                    newTex.SetPixel(x, y, colorTo);
                }
                else
                {
                    newTex.SetPixel(x, y, original.GetPixel(x, y));
                }
            }
        }

        //apply changes
        newTex.Apply();
        
        return newTex;
    }
    /// <summary>
    /// Updates a sprite's color with new colors
    /// </summary>
    /// <param name="sr">the sprite renderer</param>
    /// <param name="colorFrom">the color to change from</param>
    /// <param name="colorTo">the color to change to</param>
    public static void UpdateSpriteColor(SpriteRenderer sr, Color colorFrom, Color colorTo)
    {
        //recolor new sprite
        Texture2D tex = Utils.ColorTexture2D(sr.sprite.texture, colorFrom, colorTo);
        string tempName = sr.sprite.name;
        
        //replace the SpriteRenderer sprite with a new one using the new texture
        sr.sprite = Sprite.Create(tex, sr.sprite.rect, new Vector2(sr.sprite.pivot.x / sr.sprite.rect.width, sr.sprite.pivot.y / sr.sprite.rect.height), sr.sprite.pixelsPerUnit);
        sr.sprite.name = tempName;
        sr.material.mainTexture = tex;
    }
}
