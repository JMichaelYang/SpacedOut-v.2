using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathmatchSettings : MonoBehaviour
{
    public static DeathmatchSettings Instance;

    public List<Team> Teams;
    public float ArenaWidth = 500f;
    public float ArenaHeight = 500f;

	// Use this for initialization
	void Awake ()
    {
        if (DeathmatchSettings.Instance == null)
        {
            DeathmatchSettings.Instance = this;
        }
        else if (DeathmatchSettings.Instance != this)
        {
            DeathmatchSettings.Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        this.Teams = new List<Team>();

        //default values
        for (int i = 0; i < 2; i++)
        {
            this.Teams.Add(new Team());
            this.Teams[i].Ships = new List<Ship>();
            this.Teams[i].Ships.Add(new Ship(ShipTypes.Debug, WeaponTypes.DebugGun1, WeaponTypes.DebugGun1));
            this.Teams[i].Ships.Add(new Ship(ShipTypes.Debug, WeaponTypes.DebugGun1, WeaponTypes.DebugGun1));
            this.Teams[i].Ships.Add(new Ship(ShipTypes.Debug, WeaponTypes.DebugGun1, WeaponTypes.DebugGun1));
            this.Teams[i].Ships.Add(new Ship(ShipTypes.Debug, WeaponTypes.DebugGun1, WeaponTypes.DebugGun1));
        }
        this.Teams[0].Name = "Team One";
        this.Teams[0].TeamColor = Color.blue;
        this.Teams[1].Name = "Team Two";
        this.Teams[1].TeamColor = Color.red;
    }
}
