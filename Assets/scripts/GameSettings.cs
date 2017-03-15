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

    public static float StarDensity = .05f;
    public static int StarMinDepth = 0;
    public static int StarMaxDepth = 100;

    #endregion arena

    #region camera

    public static float CameraFOV = 60f;
    public static float CameraAspectRatio = 9f / 16f;
    public static float MinZoom = -100;
    public static float MaxZoom = -10;
    public static float ZoomSpeed = 10f;

    public static bool ShouldShake = false;
    public static float ShotShake = 0.2f;
    public static float ShakeDecrease = 4f;

    #endregion camera

    #region controls

    //inertial dampening settings
    public static bool DampenInteria = true;
    public static float DampeningMultiplier = 0.01f;

    #region keyboard

    public static KeyCode MoveForward = KeyCode.W;
    public static KeyCode MoveBack = KeyCode.S;
    public static KeyCode MoveLeft = KeyCode.A;
    public static KeyCode MoveRight = KeyCode.D;

    public static KeyCode Shoot = KeyCode.Space;

    #endregion keyboard

    #region touch

    //joystick
    public static float JoystickRadius = 2f;
    public static float JoystickMinX = -6.5f;
    public static float JoystickMaxX = -1f;
    public static float JoystickMinY = -11f;
    public static float JoystickMaxY = -6.5f;
    public static bool JoystickDisappear = false;
    public static float JoystickBaseOpacity = 0.6f;
    public static float JoystickKnobOpacity = 0.6f;

    //zoom spot
    public static float ZoomSpotMinX = 2f;
    public static float ZoomSpotMaxX = 6.5f;
    public static float ZoomSpotMinY = 6f;
    public static float ZoomSpotMaxY = 11.5f;
    public static float ZoomMaxSwipe = 2f;
    public static float ZoomSpotMultiplier = 0.05f;

    //shoot button
    public static float ShootButtonX = 5f;
    public static float ShootButtonY = -10f;
    public static float ShootButtonWidth = 2f;
    public static float ShootButtonHeight = 2f;
    public static float ShootButtonOpacity = 0.6f;

    #endregion touch

    #endregion controls

    #region resources

    public static readonly string ShipPrefab = "prefabs/ships/Ship";
    public static readonly string ShipExplosion = "prefabs/ships/ExplosionSystem";

    public static readonly string StarPrefab = "prefabs/arena/Star";

    #endregion resources
}
