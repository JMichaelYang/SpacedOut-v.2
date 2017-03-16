using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Deathmatch")]
public class DeathmatchGameObject : ScriptableObject
{
    public List<Team> Teams;
}