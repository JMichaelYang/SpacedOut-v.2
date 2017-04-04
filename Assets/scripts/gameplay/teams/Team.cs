using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    public string Name;
    public TeamIndex Index;
    public Color TeamColor;
    public List<Ship> Ships;

    public List<GameObject> FriendlyShips { get; protected set; }
    public List<GameObject> EnemyShips { get; protected set; }

    public Team(string name, TeamIndex index, Color teamColor)
    {
        this.Name = name;
        this.Index = index;
        this.TeamColor = teamColor;
        this.Ships = new List<Ship>();

        this.FriendlyShips = new List<GameObject>();
        this.EnemyShips = new List<GameObject>();
    }
}
