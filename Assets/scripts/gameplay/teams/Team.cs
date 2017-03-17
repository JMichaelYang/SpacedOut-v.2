using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    public string Name;
    public List<Ship> Ships;
    public Color TeamColor;

    public GameObject[] FriendlyShips { get; protected set; }
    public GameObject[] EnemyShips { get; protected set; }
}
