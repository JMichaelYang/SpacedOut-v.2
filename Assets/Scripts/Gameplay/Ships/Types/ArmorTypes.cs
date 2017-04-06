public class ArmorTypes
{
    public static readonly ArmorType PA50 = new ArmorType("PA50", 12000, 1, 50f);
    public static readonly ArmorType PA52 = new ArmorType("PA52", 13000, 1, 70f);
    public static readonly ArmorType PA56 = new ArmorType("PA56", 14500, 2, 100f);
}

/// <summary>
/// A type of armor
/// </summary>
public struct ArmorType : IShipPart
{
    /// <summary>
    /// The name of this armor
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// The weight of this armor (in kilograms)
    /// </summary>
    public float Weight { get; private set; }
    /// <summary>
    /// The tier of this armor
    /// </summary>
    public int Tier { get; private set; }
    /// <summary>
    /// The health that this armor provides
    /// </summary>
    public readonly float Health;

    /// <summary>
    /// A new armor type
    /// </summary>
    /// <param name="name">the name of this armor</param>
    /// <param name="weight">the weight of this armor</param>
    /// <param name="tier">the tier of this armor</param>
    /// <param name="health">the health that this armor provides</param>
    public ArmorType(string name, float weight, int tier, float health)
    {
        this.Name = name;
        this.Weight = weight;
        this.Tier = tier;
        this.Health = health;
    }

    /// <summary>
    /// An null armor type
    /// </summary>
    public static ArmorType None = new ArmorType("None", 0f, 0, 0f);
}