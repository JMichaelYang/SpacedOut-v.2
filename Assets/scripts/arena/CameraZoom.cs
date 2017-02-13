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
    }

	// Use this for initialization
	void Start ()
    {
        this.thisCamera = this.GetComponent<Camera>();
        this.thisCamera.aspect = GameSettings.CameraAspectRatio;
        this.thisCamera.fieldOfView = GameSettings.CameraFOV;
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.zoomAmount *= GameSettings.ZoomSpeed;

        Vector3 newPos = this.thisCamera.transform.localPosition;
        float newZ = newPos.z + zoomAmount;
        float newY = newPos.y + zoomAmount * Mathf.Sin(Mathf.Deg2Rad * -20f);
        newPos.z = Mathf.Lerp(newPos.z, newZ, Time.deltaTime * this.smoothing);
        newPos.y = Mathf.Lerp(newPos.y, newY, Time.deltaTime * this.smoothing);

        if (newPos.z > GameSettings.MinZoom && newPos.z < GameSettings.MaxZoom)
        {
            this.thisCamera.transform.localPosition = newPos;
        }

        this.zoomAmount = 0;
    }
}
