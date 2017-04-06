/// <summary>
/// The interface to be used for any actual part of a ship
/// </summary>
interface IShipPart
{
    /// <summary>
    /// The name of this component
    /// </summary>
    string Name { get; }
    /// <summary>
    /// The weight of this component (in kilograms)
    /// </summary>
    float Weight { get; }
    /// <summary>
    /// The tier of this component
    /// </summary>
    int Tier { get; }
}
