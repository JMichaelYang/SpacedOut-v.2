using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTypes : MonoBehaviour
{
    #region Gun Types

    public static GunType DebugGun = new GunType(10f, 30f, 4f, .05f, 5, .5f, 2f, 50, 3f, "bullet");

    #endregion Gun Types

    #region Weapon Types

    static GunType[] guns = { DebugGun, DebugGun };
    static Vector2[] vectors = { new Vector2(-0.1f, 0.1f), new Vector2(0.1f, 0.1f) };
    public static WeaponType DebugWeapon = new WeaponType(guns, vectors);

    #endregion Weapon Types

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
