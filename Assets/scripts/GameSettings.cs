using UnityEngine;
using System.Collections;

/*
 * class to store game settings
 * note scale: 1 game unit to 8m (all assets should be 32 ppm)
 */
public class GameSettings : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 30;
    }

    #region arena

    public static int ArenaWidth = 7500;
    public static int ArenaHeight = 7500;

    public static float StarDensity = .05f;
    public static int StarMinDepth = 0;
    public static int StarMaxDepth = 100;

    #endregion arena

    #region camera

    public static float CameraFOV = 60f;
    public static float CameraAspectRatio = 9f / 16f;
    public static float DefaultZoom = -800;
    public static float MinZoom = -1600;
    public static float MaxZoom = -400;
    public static float ZoomSpeed = 100f;

    public static bool ShouldShake = false;
    public static float ShotShake = 0.2f;
    public static float ShakeDecrease = 4f;

    #endregion camera

    #region controls

    //inertial dampening settings
    public static bool DampenInteria = true;
    public static float DampeningMultiplier = 0.03f;

    #region keyboard

    public static KeyCode MoveForward = KeyCode.W;
    public static KeyCode MoveBack = KeyCode.S;
    public static KeyCode MoveLeft = KeyCode.A;
    public static KeyCode MoveRight = KeyCode.D;

    public static KeyCode Shoot = KeyCode.Space;

    #endregion keyboard

    #region touch

    //joystick
    public static float JoystickRadius = .2f;
    public static float JoystickMinX = .05f;
    public static float JoystickMaxX = .45f;
    public static float JoystickMinY = .05f;
    public static float JoystickMaxY = .3f;
    public static float JoystickPixelWidth = 30f;
    public static float JoystickPixelHeight = 30f;
    public static bool JoystickDisappear = false;
    public static float JoystickBaseOpacity = 0.6f;
    public static float JoystickKnobOpacity = 0.6f;

    #endregion touch

    #endregion controls

    #region resources

    public static readonly Color ColorToReplace = Color.white;

    public static readonly string ShipPrefab = "prefabs/ships/Ship";
    public static readonly string ShipExplosion = "prefabs/ships/ExplosionSystem";

    public static readonly string ShipTexPath = "sprites/ships/";

    #endregion resources
}
