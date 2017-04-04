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

        //set up lists
        this.teams = new List<Team>();
        this.ships = new List<GameObject>();

        //TODO: Replace this test code with code that loads from a "level select" screen

        this.teams.Add(new Team("Team One", Color.blue));
        for (int i = 0; i < 1; i++)
        {
            this.teams[0].Ships.Add(new Ship(this.teams[0], ShipTypes.C20H, EngineTypes.F1140, ArmorTypes.PA56, GunTypes.HLM10, GunTypes.HLM10));
        }
        for (int i = 0; i < 2; i++)
        {
            this.teams[0].Ships.Add(new Ship(this.teams[0], ShipTypes.B12M, EngineTypes.F1150, ArmorTypes.PA52, GunTypes.LM20, GunTypes.LM30));
        }
        for (int i = 0; i < 2; i++)
        {
            this.teams[0].Ships.Add(new Ship(this.teams[0], ShipTypes.A23F, EngineTypes.F1160, ArmorTypes.PA50, GunTypes.LM20, GunTypes.LM20));
        }

        this.teams.Add(new Team("Team Two", Color.red));
        for (int i = 0; i < 1; i++)
        {
            this.teams[1].Ships.Add(new Ship(this.teams[0], ShipTypes.C20H, EngineTypes.F1140, ArmorTypes.PA56, GunTypes.HLM10, GunTypes.HLM10));
        }
        for (int i = 0; i < 2; i++)
        {
            this.teams[1].Ships.Add(new Ship(this.teams[0], ShipTypes.B12M, EngineTypes.F1150, ArmorTypes.PA52, GunTypes.LM20, GunTypes.LM30));
        }
        for (int i = 0; i < 2; i++)
        {
            this.teams[1].Ships.Add(new Ship(this.teams[0], ShipTypes.A23F, EngineTypes.F1160, ArmorTypes.PA50, GunTypes.LM20, GunTypes.LM20));
        }

        //spawn ships
        for (int j = 0; j < this.teams.Count; j++)
        {
            for (int i = 0; i < this.teams[j].Ships.Count; i++)
            {
                //find spawn point
                Vector3 nextSpawn = this.getSpawnPoint(j, i);
                GameObject shipSpawn = spawnAi(this.teams[j].Ships[i], this.teams[j].TeamColor);

                //TODO: replace this code, which changes the first player on the first team into the player
                if (i == 0 && j == 0) { this.convertPlayer(shipSpawn); }

                shipSpawn.transform.position = (Vector2)nextSpawn;
                shipSpawn.transform.Rotate(0f, 0f, nextSpawn.z);

                //add ship to teams' lists of friends and enemies
                for (int t = 0; t < this.teams.Count; t++)
                {
                    if (t == j) { this.teams[t].FriendlyShips.Add(shipSpawn); }
                    else { this.teams[t].EnemyShips.Add(shipSpawn); }
                }

                //add the spawned ship to this list of ships
                this.ships.Add(shipSpawn);
            }
        }

        //set weapon and ai lists
        //iterating through each team, using the friendly ships array in order to cover every ship
        for (int i = 0; i < this.teams.Count; i++)
        {
            for (int j = 0; j < this.teams[i].FriendlyShips.Count; j++)
            {
                this.teams[i].FriendlyShips[j].GetComponent<Weapons>().SetTeam(this.teams[i]);
                this.teams[i].FriendlyShips[j].GetComponent<AiManager>().SetTeam(this.teams[i]);
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

    private GameObject spawnAi(Ship ship, Color team)
    {
        GameObject aiObject = ship.GetShipObject(Instantiate<GameObject>(Resources.Load<GameObject>(GameSettings.ShipPrefab)));

        aiObject.tag = "AI";

        SpriteRenderer spriteRenderer = aiObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>(GameSettings.ShipTexPath + ship.Type.SpritePath);
        Utils.UpdateSpriteColor(spriteRenderer, GameSettings.ColorToReplace, team);
        spriteRenderer = aiObject.transform.FindChild("Shield").GetComponent<SpriteRenderer>();
        spriteRenderer.color = team;

        return aiObject;
    }

    private GameObject convertPlayer(GameObject playerObject)
    {
        playerObject.tag = "Player";
        playerObject.GetComponent<AiManager>().enabled = false;

        #region Camera Stuff

        GameObject shakeMedium = Instantiate<GameObject>(new GameObject(), playerObject.transform);
        shakeMedium.name = "Intermediate";
        shakeMedium.tag = "Intermediate";
        shakeMedium.transform.localPosition = new Vector3(0, GameSettings.DefaultZoom * -0.4f, GameSettings.DefaultZoom);
        shakeMedium.AddComponent<CameraZoom>();
        GameObject camera = Instantiate<GameObject>(Resources.Load<GameObject>("prefabs/PlayerCamera"), shakeMedium.transform);
        camera.name = "Player Camera";
        camera.tag = "MainCamera";

        #endregion Camera Stuff

        this.PlayerShip = playerObject;

        return playerObject;
    }
}
