using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathmatchSelectionHandler : MonoBehaviour
{/*
    //reset player preferences in editor
    [MenuItem("Edit/Reset Playerprefs")]
    public static void DeletePlayerPrefs() { PlayerPrefs.DeleteAll(); }
    */
    public static DeathmatchSelectionHandler Instance;

    private float arenaWidth = 500f;
    private float arenaHeight = 500f;

    private List<Team> teams;
    private int focusedTeam = 0;
    private int focusedShip = 0;

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

        Color[] colors = { Color.blue, Color.red };
        //default values
        for (int i = 0; i < 2; i++)
        {
            this.teams.Add(new Team("Team One", colors[i]));
            this.teams[i].Ships.Add(new Ship(this.teams[i], ShipTypes.Debug, EngineTypes.Debug, WeaponTypes.DebugGun1, WeaponTypes.DebugGun1));
            this.teams[i].Ships.Add(new Ship(this.teams[i], ShipTypes.Debug, EngineTypes.Debug, WeaponTypes.DebugGun1, WeaponTypes.DebugGun1));
            this.teams[i].Ships.Add(new Ship(this.teams[i], ShipTypes.Debug, EngineTypes.Debug, WeaponTypes.DebugGun1, WeaponTypes.DebugGun1));
            this.teams[i].Ships.Add(new Ship(this.teams[i], ShipTypes.Debug, EngineTypes.Debug, WeaponTypes.DebugGun1, WeaponTypes.DebugGun1));
        }

        #endregion Defaults

        #region UI Controls

        #region Navigation Controls

        this.backButton = this.BackButton.GetComponent<Button>();
        this.backButton.onClick.AddListener(backButtonClick);
        this.startButton = this.StartButton.GetComponent<Button>();
        this.startButton.onClick.AddListener(startButtonClick);

        #endregion Navigation Controls

        #endregion UI Controls
    }

    #region UI Controls

    #region Navigation Controls

    private void backButtonClick()
    {

    }
    private void startButtonClick()
    {
        SceneManager.LoadScene("Singleplayer");
    }

    #endregion Navigation Controls

    #endregion UI Controls
}
