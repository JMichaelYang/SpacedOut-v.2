using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BoundingLine : MonoBehaviour
{
    public enum Position
    {
        LEFT,
        RIGHT,
        TOP,
        BOTTOM
    }

    private LineRenderer lineRenderer;
    public Position LinePosition;

	// Use this for initialization
	void Awake ()
    {
        this.lineRenderer = this.gameObject.GetComponent<LineRenderer>();

        switch (this.LinePosition)
        {
            case Position.LEFT:
                //upper point
                this.lineRenderer.SetPosition(0, new Vector3(-GameSettings.ArenaWidth / 2, GameSettings.ArenaHeight / 2));
                //bottom point
                this.lineRenderer.SetPosition(1, new Vector3(-GameSettings.ArenaWidth / 2, -GameSettings.ArenaHeight / 2));
                break;

            case Position.TOP:
                //left point
                this.lineRenderer.SetPosition(0, new Vector3(-GameSettings.ArenaWidth / 2, GameSettings.ArenaHeight / 2));
                //right point
                this.lineRenderer.SetPosition(1, new Vector3(GameSettings.ArenaWidth / 2, GameSettings.ArenaHeight / 2));
                break;

            case Position.RIGHT:
                //upper point
                this.lineRenderer.SetPosition(0, new Vector3(GameSettings.ArenaWidth / 2, GameSettings.ArenaHeight / 2));
                //bottom point
                this.lineRenderer.SetPosition(1, new Vector3(GameSettings.ArenaWidth / 2, -GameSettings.ArenaHeight / 2));
                break;

            case Position.BOTTOM:
                //left point
                this.lineRenderer.SetPosition(0, new Vector3(-GameSettings.ArenaWidth / 2, -GameSettings.ArenaHeight / 2));
                //right point
                this.lineRenderer.SetPosition(1, new Vector3(GameSettings.ArenaWidth / 2, -GameSettings.ArenaHeight / 2));
                break;
        }
	}
}
