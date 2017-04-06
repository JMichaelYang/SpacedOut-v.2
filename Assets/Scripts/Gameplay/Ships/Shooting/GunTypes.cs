public class GunTypes
{
    public static readonly GunType LM20 = new GunType("LM20", 1500f, 1, 15f, 420f, 6f, 4f, 0.3f, 2f, 8f, 10, 100, "bullet");
    public static readonly GunType HLM10 = new GunType("HLM10", 2000f, 1, 30f, 400f, 8f, 2f, .6f, 3f, 10f, 8, 20, "rocket");

    public static readonly GunType LM30 = new GunType("LM30", 1700f, 2, 17f, 420f, 6f, 3.5f, 0.3f, 2f, 8f, 10, 100, "bullet");

    public static readonly GunType HAX = new GunType("HAX", 0f, 10, 50f, 600f, 6f, 1f, 0.05f, 0.2f, 0.4f, 20, 1000, "bullet");
}

/// <summary>
/// A type of gun
/// </summary>
public struct GunType : IShipPart
{
    /// <summary>
    /// The name of this gun
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// The weight of this gun (in kilograms)
    /// </summary>
    public float Weight { get; private set; }
    /// <summary>
    /// The tier of this gun
    /// </summary>
    public int Tier { get; private set; }
    /// <summary>
    /// The damage that this gun deals
    /// </summary>
    public float Damage;
    /// <summary>
    /// The velocity of bullets that this gun shoots, their range (in seconds), and their accuracy (in degrees variance)
    /// </summary>
    public float Velocity, Range, Accuracy;
    /// <summary>
    /// The amount of time between individual shots, bursts and clips
    /// </summary>
    public readonly float ShotDelay, BurstDelay, ReloadDelay;
    /// <summary>
    /// The amount of shots per burst and clip
    /// </summary>
    public readonly int BurstAmount, ReloadAmount;
    /// <summary>
    /// The path to the bullet's texture
    /// </summary>
    public readonly string BulletPath;

    /// <summary>
    /// A gun type
    /// </summary>
    /// <param name="name">the name of this gun</param>
    /// <param name="weight">the weight of this gun (in kilograms)</param>
    /// <param name="tier">the tier of this gun</param>
    /// <param name="damage">the damage that this gun deals</param>
    /// <param name="velocity">the velocity at which the bullets fire (in m/s)</param>
    /// <param name="range">the range of the bullets (in seconds)</param>
    /// <param name="accuracy">the accuracy of the bullets (how many degrees they will vary by)</param>
    /// <param name="shotDelay">the delay between shots</param>
    /// <param name="burstDelay">the delay between bursts</param>
    /// <param name="reloadDelay">the delay between clips</param>
    /// <param name="burstAmount">the number of shots per burst</param>
    /// <param name="reloadAmount">the number of shots per clip</param>
    /// <param name="bulletPath">the path to the bullet's texture</param>
    public GunType(string name, float weight, int tier, float damage,
        float velocity, float range, float accuracy, float shotDelay, float burstDelay, float reloadDelay, int burstAmount, int reloadAmount,
        string bulletPath)
    {
        this.Name = name;
        this.Weight = weight;
        this.Tier = tier;
        this.Damage = damage;
        this.Velocity = velocity;
        this.Range = range;
        this.Accuracy = accuracy;
        this.ShotDelay = shotDelay;
        this.BurstDelay = burstDelay;
        this.ReloadDelay = reloadDelay;
        this.BurstAmount = burstAmount;
        this.ReloadAmount = reloadAmount;
        this.BulletPath = bulletPath;
    }

    /// <summary>
    /// An empty weapon type
    /// </summary>
    public static GunType None = new GunType("None", 0f, 0, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, null);
}