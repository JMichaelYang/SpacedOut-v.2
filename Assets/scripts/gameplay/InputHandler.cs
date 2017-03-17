using CnControls;
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

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WSA || UNITY_EDITOR

        GameObject.Find("TouchPanel").SetActive(false);

#endif

        #endregion Touch Components
    }

    // Update is called once per frame
    void Update()
    {
        // preprocessor code
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WSA || UNITY_EDITOR

        this.keyboardUpdate();

#else

        this.touchUpdate();

#endif
    }

    /// <summary>
    /// Keyboard update function
    /// </summary>
    private void keyboardUpdate()
    {
        #region Player Movement

        if (Input.GetKey(GameSettings.MoveForward))
        {
            this.commandHandler.AddCommands(new AccelerateCommand(this.player.GetComponent<Movement>(), this.movement.MaxAcceleration));
        }
        if (Input.GetKey(GameSettings.MoveBack))
        {
            this.commandHandler.AddCommands(new AccelerateCommand(this.player.GetComponent<Movement>(), -this.movement.MaxAcceleration));
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

    /// <summary>
    /// Touchscreen update function
    /// </summary>
    private void touchUpdate()
    {
        #region Player Movement

        float x = -CnInputManager.GetAxis("Horizontal");
        if (x != 0f)
        {
            this.commandHandler.AddCommands(new RotateCommand(this.player.GetComponent<Movement>(), this.movement.MaxRotationalVelocity * x));
        }

        float y = CnInputManager.GetAxis("Vertical");
        if (y != 0f)
        {
            this.commandHandler.AddCommands(new AccelerateCommand(this.player.GetComponent<Movement>(), this.movement.MaxAcceleration * y));
        }

        #endregion Player Movement
        #region Player Shooting

        if (CnInputManager.GetButton("Shoot"))
        {
            this.commandHandler.AddCommands(new ShootCommand(this.weapons, 0, 1));
        }

        #endregion Player Shooting

        #region Camera

        this.zoom.ChangeZoom(CnInputManager.GetAxisRaw("Zoom"));

        #endregion Camera
    }
}
