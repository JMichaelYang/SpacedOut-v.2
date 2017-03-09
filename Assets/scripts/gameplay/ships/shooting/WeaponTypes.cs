using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTypes : MonoBehaviour
{
    #region Gun Types

    public static readonly GunType DebugGun1 = new GunType(10f, 30f, 6f, 6f, 0.05f, 0.5f, 2f, 5, 100, "bullet");
    public static readonly GunType DebugGun2 = new GunType(20f, 25f, 8f, 2f, 0.2f, 1f, 4f, 5, 20, "rocket");

    #endregion Gun Types

    #region Weapon Types

    static GunType[] guns = { DebugGun1, DebugGun2 };
    static Vector2[] vectors = { new Vector2(-0.2f, 0.2f), new Vector2(0.2f, 0.2f) };
    public static readonly WeaponType DebugWeapon = new WeaponType(guns, vectors);

    #endregion Weapon Types

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
