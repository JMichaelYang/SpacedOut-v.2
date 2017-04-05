using System;

public class ShieldTypes
{
    public static readonly ShieldType None = new ShieldType("None", 0f, 0, 0f, 0f);

    public static readonly ShieldType KS1 = new ShieldType("KS1", 7500f, 1, 50f, 10f);
    public static readonly ShieldType KS2 = new ShieldType("KS2", 7800f, 1, 70f, 10f);
    public static readonly ShieldType KS22 = new ShieldType("KS22", 8000f, 2, 85f, 12f);
}

/// <summary>
/// The struct to represent a type of shield
/// </summary>
public struct ShieldType : IShipPart
{
    /// <summary>
    /// The name of this shield
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// The weight of this shield (in kilograms)
    /// </summary>
    public float Weight { get; private set; }
    /// <summary>
    /// The tier of this shield
    /// </summary>
    public int Tier { get; private set; }
    /// <summary>
    /// The max health of this shield
    /// </summary>
    public readonly float Health;
    /// <summary>
    /// How much health this shield regenerates per second
    /// </summary>
    public readonly float Regen;

    /// <summary>
    /// A type of shield
    /// </summary>
    /// <param name="name">the name of this shield</param>
    /// <param name="weight">the weight of this shield (in kilograms)</param>
    /// <param name="health">the max health of this shield</param>
    /// <param name="regen">how much health this shield regenerates per second</param>
    public ShieldType(string name, float weight, int tier, float health, float regen)
    {
        this.Name = name;
        this.Weight = weight;
        this.Tier = tier;
        this.Health = health;
        this.Regen = regen;
    }

    public override bool Equals(Object obj)
    {
        // If parameter is null return false.
        if (!(obj is ShieldType)) { return false; }

        // Return true if the fields match:
        if ((ShieldType)obj == this) { return true; }
        else { return false; }
    }
    public override int GetHashCode()
    {
        return (int)(((int)this.Weight ^ this.Tier) * this.Health - this.Regen);
    }
    public static bool operator ==(ShieldType a, ShieldType b)
    {
        // If both are null, or both are same instance, return true.
        if (Object.ReferenceEquals(a, b)) { return true; }

        // If one is null, but not both, return false.
        if (((object)a == null) || ((object)b == null)) { return false; }

        return a.Name == b.Name && a.Weight == b.Weight && a.Tier == b.Tier && a.Health == b.Health && a.Regen == b.Regen;
    }
    public static bool operator !=(ShieldType a, ShieldType b)
    {
        return !(a == b);
    }
}