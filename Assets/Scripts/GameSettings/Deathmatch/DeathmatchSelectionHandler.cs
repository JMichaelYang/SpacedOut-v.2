using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathmatchSelectionHandler : MonoBehaviour
{
    public static DeathmatchSelectionHandler Instance;

    private float arenaWidth = 500f;
    private float arenaHeight = 500f;

    private List<Team> teams;
    private int focusedTeam;
    private int focusedShip;

    #region UI Controls

    #region Navigation Controls

    public GameObject BackButton;
    private Button backButton;
    public GameObject StartButton;
    private Button startButton;

    #endregion Navigation Controls

    #endregion UI Controls

    // Use this for initialization
    void Awake()
    {
        if (DeathmatchSelectionHandler.Instance == null)
        {
            DeathmatchSelectionHandler.Instance = this;
        }
        else if (DeathmatchSelectionHandler.Instance != this)
        {
            DeathmatchSelectionHandler.Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        #region Defaults

        this.focusedTeam = 0;
        this.focusedShip = 0;

        this.arenaWidth = 500f;
        this.arenaHeight = 500f;

        this.teams = new List<Team>();

        //default values
        for (int i = 0; i < 2; i++)
        {
            this.teams.Add(new Team());
            this.teams[i].Ships = new List<Ship>();
            this.teams[i].Ships.Add(new Ship(ShipTypes.Debug, WeaponTypes.DebugGun1, WeaponTypes.DebugGun1));
            this.teams[i].Ships.Add(new Ship(ShipTypes.Debug, WeaponTypes.DebugGun1, WeaponTypes.DebugGun1));
            this.teams[i].Ships.Add(new Ship(ShipTypes.Debug, WeaponTypes.DebugGun1, WeaponTypes.DebugGun1));
            this.teams[i].Ships.Add(new Ship(ShipTypes.Debug, WeaponTypes.DebugGun1, WeaponTypes.DebugGun1));
        }
        this.teams[0].Name = "Team One";
        this.teams[0].TeamColor = Color.blue;
        this.teams[1].Name = "Team Two";
        this.teams[1].TeamColor = Color.red;

        #endregion Defaults

        #region UI Controls

        #region Navigation Controls

        this.backButton = this.BackButton.GetComponent<Button>();
        //this.backButton
        this.startButton = this.StartButton.GetComponent<Button>();

        #endregion Navigation Controls

        #endregion UI Controls
    }

    #region UI Controls

    #region Navigation Controls
    


    #endregion Navigation Controls

    #endregion UI Controls
}
