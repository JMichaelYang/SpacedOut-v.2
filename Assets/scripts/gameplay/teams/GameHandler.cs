using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    //singleton pattern
    public static GameHandler Instance = null;

    private List<Team> teams;

    // Use this for initialization
    void Awake()
    {
        //maintain singleton pattern
        if (GameHandler.Instance == null)
        {
            GameHandler.Instance = this;
        }
        else if (GameHandler.Instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        //TODO: Replace this test code

        this.teams = new List<Team>();

        this.teams.Add(new Team());
        this.teams[0].Name = "Team One";
        this.teams[0].TeamColor = Color.blue;
        this.teams[0].Ships = new List<GameObject>();
        this.teams[0].Ships.Add(this.spawnPlayer(Instantiate<GameObject>(Resources.Load<GameObject>(GameSettings.ShipPrefab))));

        this.teams.Add(new Team());
        this.teams[1].Name = "Team Two";
        this.teams[1].TeamColor = Color.red;
        this.teams[1].Ships = new List<GameObject>();

        for (int i = 0; i < 3; i++)
        {
            this.teams[1].Ships.Add(Instantiate<GameObject>(Resources.Load<GameObject>(GameSettings.ShipPrefab)));
            this.teams[1].Ships[i].tag = "AI";
            this.teams[1].Ships[i].transform.position = new Vector3(Random.Range(-10f, 10f), Random.Range(10f, 30f), 0);
            this.teams[1].Ships[i].transform.Rotate(0f, 0f, Random.Range(-180f, 180f));
            //this.teams[1].Ships[i].GetComponent<Movement>().Accelerate(Random.Range(20f, 30f));
        }
    }

    private GameObject spawnPlayer(GameObject playerObject)
    {
        playerObject.tag = "Player";
        Destroy(playerObject.GetComponent<AiManager>());

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
