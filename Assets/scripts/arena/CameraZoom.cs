using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour
{
    private Camera thisCamera;
    private float smoothing = 10f;

    //amount to be zoomed
    private float zoomAmount = 0;
    public void ChangeZoom(float value)
    {
        this.zoomAmount = value;
        this.Zoom();
    }

	// Use this for initialization
	void Start ()
    {
        this.thisCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        this.thisCamera.aspect = GameSettings.CameraAspectRatio;
        this.thisCamera.fieldOfView = GameSettings.CameraFOV;
	}
	
	//function to zoom
	void Zoom ()
    {
        this.zoomAmount *= GameSettings.ZoomSpeed;

        Vector3 newPos = this.transform.localPosition;
        float newZ = newPos.z + zoomAmount;
        float newY = newPos.y + zoomAmount * Mathf.Sin(-25f * Mathf.Deg2Rad);
        newPos.z = Mathf.Lerp(newPos.z, newZ, Time.deltaTime * this.smoothing);
        newPos.y = Mathf.Lerp(newPos.y, newY, Time.deltaTime * this.smoothing);

        if (newPos.z > GameSettings.MinZoom && newPos.z < GameSettings.MaxZoom)
        {
            this.transform.localPosition = newPos;
        }

        this.zoomAmount = 0;
    }
}
