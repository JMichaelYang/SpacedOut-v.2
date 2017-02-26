using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamsHandler : MonoBehaviour
{
    public List<Team> Teams { get; protected set; }

	// Use this for initialization
    {
        //TODO: Replace this test code

        this.Teams = new List<Team>();

        this.Teams.Add(new Team());
        this.Teams[0].Name = "Team One";
        this.Teams[0].TeamColor = Color.blue;
        this.Teams[0].Ships = new List<GameObject>();
        this.Teams[0].Ships.Add(Instantiate<GameObject>(Resources.Load<GameObject>("prefabs/Ship")));
        this.Teams[0].Ships[0].tag = "Player";

        this.Teams.Add(new Team());
        this.Teams[1].Name = "Team Two";
        this.Teams[1].TeamColor = Color.red;
        this.Teams[1].Ships = new List<GameObject>();
        this.Teams[1].Ships.Add(Instantiate<GameObject>(Resources.Load<GameObject>("prefabs/Ship")));
        this.Teams[1].Ships[0].tag = "AI";
        this.Teams[1].Ships[0].transform.position = new Vector3(0, 10, 0);
        
        GameObject camera = Instantiate<GameObject>(Resources.Load<GameObject>("prefabs/PlayerCamera"));
        camera.transform.SetParent(this.Teams[0].Ships[0].transform);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
