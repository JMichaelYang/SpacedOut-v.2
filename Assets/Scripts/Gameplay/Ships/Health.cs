using UnityEngine;

/// <summary>
/// Component for any object that has health
/// </summary>
public class Health : MonoBehaviour
{
    /// <summary>
    /// The current of health of the object
    /// </summary>
    [SerializeField]
    private float health;
    /// <summary>
    /// The maximum health of the object
    /// </summary>
    [SerializeField]
    private float maxHealth;
    /// <summary>
    /// The integer version of health
    /// </summary>
    public int IntHealth { get { return (int)this.health; } }
    /// <summary>
    /// The integer version of the max health
    /// </summary>
    public int IntMaxHealth { get { return (int)this.maxHealth; } }
    /// <summary>
    /// Whether the object is alive
    /// </summary>
    public bool IsAlive { get { return this.IntHealth <= 0; } }

    /// <summary>
    /// When the component is initialized, set its values to 0
    /// </summary>
    void Awake()
    {
        this.health = 0f;
        this.maxHealth = 0f;
    }

    /// <summary>
    /// Set the health and max health of this component
    /// </summary>
    /// <param name="health">the initial health</param>
    /// <param name="maxHealth">the initial max health</param>
    /// <returns>The updated health object</returns>
    public Health SetStatistics(float health, float maxHealth)
    {
        this.health = health;
        this.maxHealth = maxHealth;

        return this;
    }
    /// <summary>
    /// Set the health and the max health of this component to the same value
    /// </summary>
    /// <param name="maxHealth">the new health and max health of this object</param>
    /// <returns>The updated health object</returns>
    public Health SetStatistics(float maxHealth)
    {
        return this.SetStatistics(maxHealth, maxHealth);
    }

    /// <summary>
    /// Sets the health to a new value
    /// </summary>
    /// <param name="health">the new health</param>
    /// <returns>The new health (clamped between 0f and the max health)</returns>
    public float SetHealth(float health)
    {
        this.health = Mathf.Clamp(health, 0f, this.maxHealth);
        return this.health;
    }
    /// <summary>
    /// Apply damage to the health
    /// </summary>
    /// <param name="damage">the amount of damage to deal</param>
    /// <returns>The new amount of health that the object has</returns>
    public float ApplyDamage(float damage)
    {
        this.health -= damage;
        if(this.health < 0f) { this.health = 0f; }
        return this.health;
    }
    /// <summary>
    /// Apply integer damage to the health
    /// </summary>
    /// <param name="damage">the amount of damage to deal</param>
    /// <returns>The new health that the object has</returns>
    public int ApplyDamage(int damage)
    {
        this.health -= damage;
        if (this.health < 0f) { this.health = 0f; }
        return this.IntHealth;
    }
    /// <summary>
    /// Apply healing to the health
    /// </summary>
    /// <param name="healing">the amount of healing to apply</param>
    /// <returns>The new amount of health that the object has</returns>
    public float ApplyHealing(float healing)
    {
        this.health += healing;
        if (this.health > this.maxHealth) { this.health = this.maxHealth; }
        return this.health;
    }
    /// <summary>
    /// Apply integer healing to the health
    /// </summary>
    /// <param name="healing">the amount of healing to apply</param>
    /// <returns>The new health that the object has</returns>
    public int ApplyHealing(int healing)
    {
        this.health += healing;
        if (this.health > this.maxHealth) { this.health = this.maxHealth; }
        return this.IntHealth;
    }
}
