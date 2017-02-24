using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiHandler : MonoBehaviour
{
    //joystick prefab
    private GameObject joystick;
    //zoomspot prefab
    private GameObject zoomSpot;
    //button prefab
    private GameObject button;

	// Use this for initialization
	void Start ()
    {
        this.transform.localPosition = new Vector3(0, 0, 20);

        //preprocessor code for loading UI
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WSA// || UNITY_EDITOR

#else
        this.LoadTouchElements();
#endif
    }

    private void LoadTouchElements()
    {
        //joystick
        this.joystick = Instantiate(Resources.Load<GameObject>("prefabs/gui/Joystick"));
        this.joystick.transform.SetParent(this.gameObject.transform);
        this.joystick.transform.localPosition = new Vector3((GameSettings.JoystickMinX + GameSettings.JoystickMaxX) / 2, (GameSettings.JoystickMinY + GameSettings.JoystickMaxY) / 2, 0);
        this.joystick.GetComponent<Joystick>().SetBounds(GameSettings.JoystickMinX, GameSettings.JoystickMinY, GameSettings.JoystickMaxX, GameSettings.JoystickMaxY, GameSettings.JoystickRadius);
        this.joystick.GetComponent<Joystick>().SetOpacity(GameSettings.JoystickBaseOpacity, GameSettings.JoystickKnobOpacity);

        //zoom spot
        this.zoomSpot = Instantiate(Resources.Load<GameObject>("prefabs/gui/ZoomSpot"));
        this.zoomSpot.transform.SetParent(this.gameObject.transform);
        this.zoomSpot.transform.localPosition = new Vector3((GameSettings.ZoomSpotMinX + GameSettings.ZoomSpotMaxX) / 2, (GameSettings.ZoomSpotMinY + GameSettings.ZoomSpotMaxY) / 2, 0);
        this.zoomSpot.GetComponent<ZoomSpot>().SetBounds(GameSettings.ZoomSpotMinX, GameSettings.ZoomSpotMinY, GameSettings.ZoomSpotMaxX, GameSettings.ZoomSpotMaxY, GameSettings.ZoomMaxSwipe);
        
        //button
        this.button = Instantiate(Resources.Load<GameObject>("prefabs/gui/Button"));
        this.button.transform.SetParent(this.gameObject.transform);
        this.button.transform.localPosition = new Vector3(GameSettings.ShootButtonX, GameSettings.ShootButtonY, 0);
        this.button.GetComponent<Button>().Initialize(GameSettings.ShootButtonWidth, GameSettings.ShootButtonHeight,
            Resources.Load<Sprite>("sprites/gui/button/ButtonUp"), Resources.Load<Sprite>("sprites/gui/button/ButtonDown"), GameSettings.ShootButtonOpacity);
    }

    // Update is called once per frame
    void Update ()
    {
        
	}
}
