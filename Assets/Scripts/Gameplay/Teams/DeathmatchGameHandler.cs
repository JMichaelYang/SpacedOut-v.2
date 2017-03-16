using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathmatchGameHandler : MonoBehaviour
{
    //singleton pattern
    public static DeathmatchGameHandler Instance = null;

    private List<Team> teams;
    private List<GameObject> ships;

    public GameObject PlayerShip { get; protected set; }

    // Use this for initialization
    void Awake()
    {
        //maintain singleton pattern
        if (DeathmatchGameHandler.Instance == null)
        {
            DeathmatchGameHandler.Instance = this;
        }
        else if (DeathmatchGameHandler.Instance != this)
        {
            Destroy(this.gameObject);
        }

        //TODO: Replace this test code

        this.teams = new List<Team>();
        this.ships = new List<GameObject>();

        this.teams.Add(new Team());
        this.teams[0].Name = "Team One";
        this.teams[0].TeamColor = Color.blue;
        this.teams[0].Ships = new List<Ship>();
        for (int i = 0; i < 8; i++)
        {
            this.teams[0].Ships.Add(new Ship(ShipTypes.Debug, WeaponTypes.DebugGun1, WeaponTypes.DebugGun1));
        }

        this.teams.Add(new Team());
        this.teams[1].Name = "Team Two";
        this.teams[1].TeamColor = Color.red;
        this.teams[1].Ships = new List<Ship>();
        for (int i = 0; i < 8; i++)
        {
            this.teams[1].Ships.Add(new Ship(ShipTypes.Debug, WeaponTypes.DebugGun1, WeaponTypes.DebugGun1));
        }

        //spawn ships
        for (int j = 0; j < teams.Count; j++)
        {
            for (int i = 0; i < teams[j].Ships.Count; i++)
            {
                Vector3 nextSpawn = this.getSpawnPoint(j, i);
                GameObject shipSpawn = spawnAi(teams[j].Ships[i]);
                if (i == 0 && j == 0)
                {
                    this.convertPlayer(shipSpawn);
                }
                shipSpawn.transform.position = (Vector2)nextSpawn;
                shipSpawn.transform.Rotate(0f, 0f, nextSpawn.z);
            }
        }
    }

    /// <summary>
    /// Function to retrieve the next spawn location for a ship
    /// </summary>
    /// <param name="teamIndex">which team is currently being spawned</param>
    /// <param name="spawnIndex">which index in the team to spawn</param>
    /// <returns>A vector three with the X and Y representing the X and Y coordinates of the spawn point and the Z representing the rotation of the object</returns>
    private Vector3 getSpawnPoint(int teamIndex, int spawnIndex)
    {
        Vector3 spawn = Vector3.zero;

        switch (teamIndex)
        {
            //left spawn
            case 0:
                spawn.z = -90f;
                spawn.x = -(GameSettings.ArenaWidth / 2) + 20 - (1 * spawnIndex);
                if (spawnIndex % 2 == 0) { spawn.y = spawnIndex / 2 * -4; }
                else { spawn.y = (spawnIndex + 1) / 2 * 4; }
                break;

            //right spawn
            case 1:
                spawn.z = 90f;
                spawn.x = (GameSettings.ArenaWidth / 2) - 20 + (1 * spawnIndex);
                if (spawnIndex % 2 == 0) { spawn.y = spawnIndex / 2 * 4; }
                else { spawn.y = (spawnIndex + 1) / 2 * -4; }
                break;

            //top spawn
            case 2:
                spawn.z = 180f;
                spawn.y = (GameSettings.ArenaHeight / 2) - 20 + (1 * spawnIndex);
                if (spawnIndex % 2 == 0) { spawn.x = spawnIndex / 2 * -4; }
                else { spawn.x = (spawnIndex + 1) / 2 * 4; }
                break;

            //bottom spawn
            case 3:
                spawn.z = 0f;
                spawn.y = -(GameSettings.ArenaHeight / 2) + 20 - (1 * spawnIndex);
                if (spawnIndex % 2 == 0) { spawn.x = spawnIndex / 2 * 4; }
                else { spawn.x = (spawnIndex + 1) / 2 * -4; }
                break;
        }

        return spawn;
    }

    private GameObject spawnAi(Ship ship)
    {
        GameObject aiObject = Instantiate<GameObject>(Resources.Load<GameObject>(GameSettings.ShipPrefab));

        aiObject.tag = "AI";

        aiObject.GetComponent<Weapons>().ReadWeapons(ship.Guns, ship.Type.Offsets);
        aiObject.GetComponent<Movement>().SetStatistics(ship.Type);
        aiObject.GetComponent<ShipHandler>().SetStatistics(ship.Type);

        return aiObject;
    }

    private GameObject convertPlayer (GameObject playerObject)
    {
        playerObject.tag = "Player";
        playerObject.GetComponent<AiManager>().enabled = false;

        #region Camera Stuff

        GameObject shakeMedium = Instantiate<GameObject>(new GameObject(), playerObject.transform);
        shakeMedium.name = "Intermediate";
        shakeMedium.tag = "Intermediate";
        shakeMedium.transform.localPosition = new Vector3(0, 16, -40);
        shakeMedium.AddComponent<CameraZoom>();
        GameObject camera = Instantiate<GameObject>(Resources.Load<GameObject>("prefabs/PlayerCamera"), shakeMedium.transform);
        camera.name = "Player Camera";
        camera.tag = "MainCamera";

        #endregion Camera Stuff

        this.PlayerShip = playerObject;

        return playerObject;
    }
}
