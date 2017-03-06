using UnityEngine;
using System.Collections;

public class StarSpawner : MonoBehaviour
{
    private GameObject template;
    private SpriteRenderer templateRenderer;

    void Awake()
    {
        this.template = Resources.Load<GameObject>(GameSettings.StarPrefab);
        this.templateRenderer = this.template.GetComponent<SpriteRenderer>();
    }

	//spawn stars
	void Start ()
    {
        Vector3 min = new Vector3(-GameSettings.ArenaWidth / 2, -GameSettings.ArenaHeight / 2, GameSettings.StarMinDepth);
        Vector3 max = new Vector3(GameSettings.ArenaWidth / 2, GameSettings.ArenaHeight / 2, GameSettings.StarMaxDepth);
        float numStars = GameSettings.StarDensity * (GameSettings.ArenaWidth * GameSettings.ArenaHeight);

        for (int i = 0; i < numStars; i++)
        {
            Color color = this.templateRenderer.color;
            color.r = Random.Range(0f, 1f);
            color.b = Random.Range(0f, 1f);
            color.g = Random.Range(0f, 1f);
            this.templateRenderer.color = color;
            Instantiate(this.template, Utils.RandomVector3(min, max), Quaternion.identity);
        }
	}
}
