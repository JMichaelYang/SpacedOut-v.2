using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public List<Team> Teams { get; protected set; }

	// Use this for initialization
	void Awake ()
    {
        //TODO: Replace this test code

        this.Teams = new List<Team>();

        this.Teams.Add(new Team());
        this.Teams[0].Name = "Team One";
        this.Teams[0].TeamColor = Color.blue;
        this.Teams[0].Ships = new List<GameObject>();
        this.Teams[0].Ships.Add(this.spawnPlayer(Instantiate<GameObject>(Resources.Load<GameObject>(GameSettings.ShipPrefab))));

        this.Teams.Add(new Team());
        this.Teams[1].Name = "Team Two";
        this.Teams[1].TeamColor = Color.red;
        this.Teams[1].Ships = new List<GameObject>();
        this.Teams[1].Ships.Add(Instantiate<GameObject>(Resources.Load<GameObject>(GameSettings.ShipPrefab)));
        this.Teams[1].Ships[0].tag = "AI";
        this.Teams[1].Ships[0].transform.position = new Vector3(0, 10, 0);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private GameObject spawnPlayer(GameObject playerObject)
    {
        playerObject.tag = "Player";

        GameObject shakeMedium = Instantiate<GameObject>(new GameObject(), playerObject.transform);
        shakeMedium.name = "Intermediate";
        shakeMedium.tag = "Intermediate";
        shakeMedium.transform.localPosition = new Vector3(0, 16, -40);
        shakeMedium.AddComponent<CameraZoom>();
        GameObject camera = Instantiate<GameObject>(Resources.Load<GameObject>("prefabs/PlayerCamera"), shakeMedium.transform);
        camera.name = "Player Camera";
        camera.tag = "MainCamera";

        return playerObject;
    }
}
