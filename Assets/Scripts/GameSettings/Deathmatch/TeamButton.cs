using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamButton : MonoBehaviour
{
    public int TeamIndex = 0;

    void Awake()
    {
        if (DeathmatchSettings.Instance.Teams.Count > TeamIndex)
        {
            this.GetComponent<Image>().color = DeathmatchSettings.Instance.Teams[this.TeamIndex].TeamColor;
        }
        else
        {
            this.GetComponent<Image>().color = Color.gray;
        }
    }
}
