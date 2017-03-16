using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathmatchStart : MonoBehaviour
{
    public void StartDeathmatch()
    {
        SceneManager.LoadScene("Singleplayer");
    }
}
