using UnityEngine;
using System.Collections;

/*
 * class to store game settings
 * note scale: 1 game unit to 8m (all assets should be 32 ppm)
 */
public class GameSettings : MonoBehaviour
{
    #region arena

    public static int ArenaWidth = 500;
    public static int ArenaHeight = 500;

    public static float StarDensity = .2f;
    public static int StarMinDepth = 0;
    public static int StarMaxDepth = 100;

    #endregion arena

    #region camera

    public static float CameraFOV = 60f;
    public static float CameraAspectRatio = 9f / 16f;
    public static float MinZoom = -50;
    public static float MaxZoom = -10;
    public static float ZoomSpeed = 20f;

    #endregion camera

    #region controls

    //inertial dampening settings
    public static bool DampenInteria = true;
    public static float DampeningMultiplier = 0.015f;

    #region keyboard

    public static KeyCode MoveForward = KeyCode.W;
    public static KeyCode MoveBack = KeyCode.S;
    public static KeyCode MoveLeft = KeyCode.A;
    public static KeyCode MoveRight = KeyCode.D;

    #endregion keyboard

    #region touch

    public static float JoystickRadius = 2;
    public static float JoystickMinX = -6;
    public static float JoystickMaxX = -1;
    public static float JoystickMinY = -11;
    public static float JoystickMaxY = -6;

    public static bool JoystickDisappear = true;
    public static float JoystickBaseOpacity = 0.6f;
    public static float JoystickKnobOpacity = 0.6f;

    public static float ZoomSpotMinX = 2;
    public static float ZoomSpotMaxX = 6;
    public static float ZoomSpotMinY = 6;
    public static float ZoomSpotMaxY = 11;
    public static float ZoomMaxSwipe = 2;

    public static float ZoomSpotMultiplier = 0.01f;

    #endregion touch

    #endregion controls

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
