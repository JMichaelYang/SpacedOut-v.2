using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    //store an instance of the player object
    private GameObject player;
    private Movement movement;
    private CommandHandler commandHandler;

    //camera object
    private GameObject playerCamera;
    private CameraZoom zoom;

    #region Touch Input Components

    private Joystick joystickComponent;
    private int joystickFingerId = -1;

    private ZoomSpot zoomSpotComponent;
    private int zoomSpotFingerId = -1;

    private Button shootButtonComponent;
    private int shootButtonFingerId = -1;

    #endregion Touch Input Components

    // Use this for initialization
    void Start()
    {
        this.commandHandler = GameObject.FindObjectOfType<CommandHandler>();

        this.player = GameObject.FindGameObjectWithTag("Player");
        this.movement = this.player.GetComponent<Movement>();

        this.playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        this.zoom = this.playerCamera.GetComponent<CameraZoom>();

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WSA// || UNITY_EDITOR

#else
        this.joystickComponent = this.playerCamera.GetComponentInChildren<Joystick>();
        this.zoomSpotComponent = this.playerCamera.GetComponentInChildren<ZoomSpot>();
        this.shootButtonComponent = this.playerCamera.GetComponentInChildren<Button>();
#endif
    }

    // Update is called once per frame
    void Update()
    {
        // preprocessor code
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WSA || UNITY_EDITOR
        this.keyboardUpdate();
#else
        this.touchscreenUpdate();
#endif
    }

    /// <summary>
    /// touchscreen update function
    /// </summary>
    private void touchscreenUpdate()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                int id = touch.fingerId;

                switch (touch.phase)
                {
                    case TouchPhase.Began:

                        //if the touch hits the touch items, update the id
                        #region Joystick Updating

                        if (!this.joystickComponent.IsActive)
                        {
                            if (this.joystickComponent.UpdateJoystick(touch))
                            {
                                this.joystickFingerId = id;
                            }
                        }

                        #endregion Joystick Updating
                        #region ZoomSpot Updating

                        if (!this.zoomSpotComponent.IsActive)
                        {
                            if (this.zoomSpotComponent.UpdateSpot(touch))
                            {
                                this.zoomSpotFingerId = id;
                            }
                        }

                        #endregion ZoomSpot Updating
                        #region ShootButton Updating

                        if (this.shootButtonComponent.UpdateButton(touch))
                        {
                            this.shootButtonFingerId = id;
                        }

                        #endregion ShootButton Updating

                        break;

                    case TouchPhase.Ended:

                        //if IDs match, update touch items
                        #region Joystick Updating

                        if (this.joystickFingerId == id)
                        {
                            this.joystickComponent.UpdateJoystick(touch);
                            this.joystickFingerId = -1;
                        }

                        #endregion Joystick Updating
                        #region ZoomSpot Updating

                        if (this.zoomSpotFingerId == id)
                        {
                            this.zoomSpotComponent.UpdateSpot(touch);
                            this.zoomSpotFingerId = -1;
                        }

                        #endregion ZoomSpot Updating
                        #region ShootButton Updating

                        if (this.shootButtonFingerId == id)
                        {
                            this.shootButtonComponent.UpdateButton(touch);
                            this.shootButtonFingerId = -1;
                        }

                        #endregion ShootButton Updating

                        break;

                    default:

                        //if IDs match, update touch items
                        #region Joystick Updating

                        if (this.joystickFingerId == id)
                        {
                            this.joystickComponent.UpdateJoystick(touch);

                            this.commandHandler.AddCommands(new AccelerateCommand(this.movement, this.joystickComponent.ValueY));
                            this.commandHandler.AddCommands(new RotateCommand(this.movement, -this.joystickComponent.ValueX));
                        }

                        #endregion Joystick Updating
                        #region ZoomSpot Updating

                        if (this.zoomSpotFingerId == id)
                        {
                            this.zoomSpotComponent.UpdateSpot(touch);
                            this.commandHandler.AddCommands(new ZoomCommand(this.zoom, this.zoomSpotComponent.GetValue() * GameSettings.ZoomSpotMultiplier));
                        }

                        #endregion ZoomSpot Updating
                        #region ShootButton Updating

                        if (this.shootButtonFingerId == id)
                        {
                            this.shootButtonComponent.UpdateButton(touch);
                            this.commandHandler.AddCommands(new ShootCommand(this.player.GetComponent<Weapons>(), 0, 1));
                        }

                        #endregion ShootButton Updating

                        break;
                }
            }
        }
    }

    /// <summary>
    /// keyboard update function
    /// </summary>
    private void keyboardUpdate()
    {
        #region Player Movement

        if (Input.GetKey(GameSettings.MoveForward))
        {
            this.commandHandler.AddCommands(new AccelerateCommand(this.player.GetComponent<Movement>(), 1));
        }
        if (Input.GetKey(GameSettings.MoveBack))
        {
            this.commandHandler.AddCommands(new AccelerateCommand(this.player.GetComponent<Movement>(), -1));
        }
        if (Input.GetKey(GameSettings.MoveLeft))
        {
            this.commandHandler.AddCommands(new RotateCommand(this.player.GetComponent<Movement>(), 1));
        }
        if (Input.GetKey(GameSettings.MoveRight))
        {
            this.commandHandler.AddCommands(new RotateCommand(this.player.GetComponent<Movement>(), -1));
        }

        #endregion Player Movement
        #region Player Shooting

        if (Input.GetKey(GameSettings.Shoot))
        {
            this.commandHandler.AddCommands(new ShootCommand(this.player.GetComponent<Weapons>(), 0, 1));
        }

        #endregion Player Shooting

        #region Camera

        //this.zoom.ChangeZoom(Input.GetAxisRaw("Mouse ScrollWheel"));
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
