public class ShieldTypes
{
    public static readonly ShieldType KS1 = new ShieldType("KS1", 7500f, 1, 50f, 10f);
    public static readonly ShieldType KS2 = new ShieldType("KS2", 7800f, 1, 70f, 10f);
    public static readonly ShieldType KS22 = new ShieldType("KS22", 8000f, 2, 85f, 12f);
}

/// <summary>
/// The struct to represent a type of shield
/// </summary>
public struct ShieldType : IShipComponent
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
}