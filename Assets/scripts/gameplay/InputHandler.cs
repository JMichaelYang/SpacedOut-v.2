using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    //singleton pattern
    public static InputHandler Instance = null;

    //store an instance of the player object
    private GameObject player;
    private Weapons weapons;
    private Movement movement;
    private CommandHandler commandHandler;

    //camera object
    private GameObject playerCamera;
    private CameraZoom zoom;
    private CameraShake shake;

    #region Touch Input Components

    private Joystick joystickComponent;
    private int joystickFingerId = -1;

    private Button zoomOutButton;
    private Button zoomInButton;

    private Button shootButton;

    #endregion Touch Input Components

    void Awake()
    {
        //maintain singleton pattern
        if (InputHandler.Instance == null)
        {
            InputHandler.Instance = this;
        }
        else if (InputHandler.Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        this.commandHandler = GameObject.FindObjectOfType<CommandHandler>();

        this.player = GameObject.FindGameObjectWithTag("Player");
        this.weapons = this.player.GetComponent<Weapons>();
        this.movement = this.player.GetComponent<Movement>();

        this.playerCamera = GameObject.FindGameObjectWithTag("MainCamera"); ;
        this.zoom = GameObject.FindObjectOfType<CameraZoom>();
        this.shake = GameObject.FindObjectOfType<CameraShake>();

        #region Touch Components

#if !UNITY_STANDALONE && !UNITY_WEBPLAYER && !UNITY_WSA //&& !UNITY_EDITOR

        this.zoomInButton = GameObject.Find("ZoomIn").GetComponent<Button>();
        this.zoomInButton.onClick.AddListener(zoomIn);
        this.zoomOutButton = GameObject.Find("ZoomOut").GetComponent<Button>();
        this.zoomOutButton.onClick.AddListener(zoomOut);

        this.shootButton = GameObject.Find("ShootButton").GetComponent<Button>();
        this.shootButton.onClick.AddListener(shoot);

#else

        GameObject.Find("TouchPanel").SetActive(false);

#endif

        #endregion Touch Components
    }

    #region Touchscreen Functions

    void zoomIn() { this.zoom.ChangeZoom(.5f); }
    void zoomOut() { this.zoom.ChangeZoom(-.5f); }

    void shoot() { this.commandHandler.AddCommands(new ShootCommand(this.weapons, 0, 1)); }

    #endregion Touchscreen Functions

    // Update is called once per frame
    void Update()
    {
        // preprocessor code
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WSA || UNITY_EDITOR
        this.keyboardUpdate();
#endif
    }

    /// <summary>
    /// keyboard update function
    /// </summary>
    private void keyboardUpdate()
    {
        #region Player Movement

        if (Input.GetKey(GameSettings.MoveForward))
        {
            this.commandHandler.AddCommands(new AccelerateCommand(this.player.GetComponent<Movement>(), this.movement.MaxAcceleration, true));
        }
        if (Input.GetKey(GameSettings.MoveBack))
        {
            this.commandHandler.AddCommands(new AccelerateCommand(this.player.GetComponent<Movement>(), -this.movement.MaxAcceleration, true));
        }
        if (Input.GetKey(GameSettings.MoveLeft))
        {
            this.commandHandler.AddCommands(new RotateCommand(this.player.GetComponent<Movement>(), this.movement.MaxRotationalVelocity));
        }
        if (Input.GetKey(GameSettings.MoveRight))
        {
            this.commandHandler.AddCommands(new RotateCommand(this.player.GetComponent<Movement>(), -this.movement.MaxRotationalVelocity));
        }

        #endregion Player Movement
        #region Player Shooting

        if (Input.GetKey(GameSettings.Shoot))
        {
            this.commandHandler.AddCommands(new ShootCommand(this.weapons, 0, 1));
        }

        #endregion Player Shooting

        #region Camera

        this.zoom.ChangeZoom(Input.GetAxisRaw("Mouse ScrollWheel"));
        if (Input.GetKey(KeyCode.I))
        {
            this.zoom.ChangeZoom(1f);
        }
        if (Input.GetKey(KeyCode.O))
        {
            this.zoom.ChangeZoom(-1f);
        }

        #endregion Camera
    }
}
