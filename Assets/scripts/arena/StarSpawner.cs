using UnityEngine;
using System.Collections;

public class StarSpawner : MonoBehaviour
{
    public GameObject StarPrefab;

	//spawn stars
	void Start ()
    {
        Vector3 min = new Vector3(-GameSettings.ArenaWidth / 2, -GameSettings.ArenaHeight / 2, GameSettings.StarMinDepth);
        Vector3 max = new Vector3(GameSettings.ArenaWidth / 2, GameSettings.ArenaHeight / 2, GameSettings.StarMaxDepth);
        float numStars = GameSettings.StarDensity * (GameSettings.ArenaWidth * GameSettings.ArenaHeight);

        //GameObject tempStar = StarPrefab;

        for (int i = 0; i < numStars; i++)
        {
            //tempStar.GetComponent<SpriteRenderer>().color = Color.blue;
            //Debug.Log(tempStar.GetComponent<SpriteRenderer>().color);
            Instantiate(this.StarPrefab, Utils.RandomVector3(min, max), Quaternion.identity);
        }
	}
}
