public class EngineTypes
{
    public static readonly EngineType F1100 = new EngineType("F1100", 5200f, 1, 100000f);
    public static readonly EngineType F1110 = new EngineType("F1110", 5500f, 1, 110000f);
    public static readonly EngineType F1120 = new EngineType("F1120", 5700f, 1, 120000f);
}

/// <summary>
/// A type of engine
/// </summary>
public struct EngineType : IShipComponent
{
    /// <summary>
    /// The name of this engine
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// The weight of this engine (in kilograms)
    /// </summary>
    public float Weight { get; private set; }
    /// <summary>
    /// The tier of this engine
    /// </summary>
    public int Tier { get; private set; }
    /// <summary>
    /// The maximum thrust that a ship using this engine can attain (in newtons)
    /// </summary>
    public readonly float Thrust;

    /// <summary>
    /// A type of engine
    /// </summary>
    /// <param name="name">the name of this engine</param>
    /// <param name="weight">the weight of this engine (in kilograms)</param>
    /// <param name="maxVelocity">the maximum velocity that a ship using this engine can attain (in m/s)</param>
    /// <param name="thrust">the maximum thrust that a ship using this engine can attain (in newtons)</param>
    public EngineType(string name, float weight, int tier, float thrust)
    {
        this.Name = name;
        this.Weight = weight;
        this.Tier = tier;
        this.Thrust = thrust;
    }

    /// <summary>
    /// An engine with no values
    /// </summary>
    public static EngineType None = new EngineType("None", 0f, 0, 0f);
}
