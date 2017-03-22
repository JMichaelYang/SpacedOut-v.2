using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthText : MonoBehaviour
{
    private ShipHandler playerShipHandler;
    private Text text;

	// Use this for initialization
	void Start ()
    {
        this.text = this.GetComponent<Text>();
        this.playerShipHandler = DeathmatchGameHandler.Instance.PlayerShip.GetComponent<ShipHandler>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.text.text = this.playerShipHandler.Health + "/" + this.playerShipHandler.MaxHealth;
	}
}
