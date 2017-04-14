using UnityEngine;
using System.Collections;

/// <summary>
/// Class for all game settings
/// Note that default scale is 1 unit : 1 meter
/// </summary>
public class GameSettings : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 30;
    }

    #region Arena

    public static int ArenaWidth = 8000;
    public static int ArenaHeight = 8000;

    public static float StarDensity = .05f;
    public static int StarMinDepth = 0;
    public static int StarMaxDepth = 100;

    #endregion Arena

    #region UI

    public static float CameraFOV = 60f;
    public static float CameraAspectRatio = 9f / 16f;
    public static float DefaultZoom = -800;
    public static float MinZoom = -1600;
    public static float MaxZoom = -400;
    public static float ZoomSpeed = 100f;

    public static bool ShouldShake = false;
    public static float ShotShake = 0.2f;
    public static float ShakeDecrease = 4f;

    public static float MarkerOffset = 1f / 20f;
    public static float MarkerSize = 1f / 80f;

    #endregion UI

    #region Controls

    #region Keyboard

    public static KeyCode MoveForward = KeyCode.W;
    public static KeyCode MoveBack = KeyCode.S;
    public static KeyCode MoveLeft = KeyCode.A;
    public static KeyCode MoveRight = KeyCode.D;

    public static KeyCode Shoot = KeyCode.Space;

    #endregion Keyboard

    #region Touch

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

    #endregion Touch

    #endregion Controls

    #region Resources

    public static readonly Color ColorToReplace = Color.white;

    public static readonly string ShipPrefab = "Prefabs/Ships/Ship";
    public static readonly string ShipExplosion = "Prefabs/Ships/ExplosionSystem";

    public static readonly string ShipTexPath = "Sprites/Ships/";

    #endregion Resources

    #region Ships

    public static readonly float ShieldRegenDelay = 0.05f;

    //inertial dampening settings
    public static bool DampenInteria = true;
    public static float DampeningMultiplier = 0.005f;

    #endregion Ships

    #region Layers

    public static readonly int TeamOneBulletLayer = 8;
    public static readonly int TeamTwoBulletLayer = 9;
    public static readonly int TeamThreeBulletLayer = 10;
    public static readonly int TeamFourBulletLayer = 11;

    public static readonly int TeamOneShipLayer = 12;
    public static readonly int TeamTwoShipLayer = 13;
    public static readonly int TeamThreeShipLayer = 14;
    public static readonly int TeamFourShipLayer = 15;

    #endregion Layers
}

public enum TeamIndex
{
    ONE,
    TWO,
    THREE,
    FOUR
};