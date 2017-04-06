using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthText : MonoBehaviour
{
    private Health health;
    private Shield shield;
    private Text text;

	// Use this for initialization
	void Start ()
    {
        this.text = this.GetComponent<Text>();
        this.health = DeathmatchGameHandler.Instance.PlayerShip.GetComponent<Health>();
        this.shield = DeathmatchGameHandler.Instance.PlayerShip.GetComponentInChildren<Shield>(true);
    }
	
	// Update is called once per frame
	void Update ()
    {
        this.text.text = this.health.IntHealth + "/" + this.health.IntMaxHealth;
        this.text.text += "    " + this.shield.ShieldHealth + "/" + this.shield.ShieldMaxHealth;
	}
}
