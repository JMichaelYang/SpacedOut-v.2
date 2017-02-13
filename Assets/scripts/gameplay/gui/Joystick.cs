using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    //the current x and y values for the joystick (between -1 and 1)
    public float ValueX
    {
        get { return this.knob.transform.localPosition.x / this.radius; }
    }
    public float ValueY
    {
        get { return this.knob.transform.localPosition.y / this.radius; }
    }

    //max radius of the joystick
    public float radius;

    //opacity
    public float baseOpacity;
    public float knobOpacity;

    //knob object
    public GameObject knob;
    
    //if this joystick is currently active
    public bool IsActive { get; protected set; }

    // Use this for initialization
    void Start()
    {
        this.IsActive = false;

        //hide joystick if it can dissapear
        if (GameSettings.JoystickDisappear)
        {
            this.transform.localScale = Vector3.zero;
            this.knob.transform.localScale = Vector3.zero;
        }

        this.knob.transform.localPosition = Vector3.zero;
        this.radius = GameSettings.JoystickRadius;
        this.baseOpacity = GameSettings.JoystickBaseOpacity;
        this.knobOpacity = GameSettings.JoystickKnobOpacity;

        this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, this.baseOpacity);
        this.knob.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, this.knobOpacity);
    }

    // Update is called once per frame
    void Update() { }

    public bool UpdateJoystick(Touch touch)
    {
        float localTouchX = Utils.ConvertScale(touch.position.x, 0, Screen.width, -6.5f, 6.5f);
        float localTouchY = Utils.ConvertScale(touch.position.y, 0, Screen.height, -11.5f, 11.5f);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                if (localTouchX > GameSettings.JoystickMinX && localTouchX < GameSettings.JoystickMaxX
                    && localTouchY > GameSettings.JoystickMinY && localTouchY < GameSettings.JoystickMaxY)
                {
                    this.IsActive = true;
                    if (GameSettings.JoystickDisappear)
                    {
                        this.transform.localScale = Vector3.one;
                        this.knob.transform.localScale = Vector3.one;
                    }
                    this.transform.localPosition = new Vector3(localTouchX, localTouchY);
                    this.knob.transform.localPosition = Vector3.zero;
                    return true;
                }
                break;

            case TouchPhase.Ended:
                this.IsActive = false;
                if (GameSettings.JoystickDisappear)
                {
                    this.transform.localScale = Vector3.zero;
                    this.knob.transform.localScale = Vector3.zero;
                }
                this.knob.transform.localPosition = Vector3.zero;
                break;

            default:

                this.knob.transform.localPosition = new Vector3(localTouchX, localTouchY, this.transform.localPosition.z) - this.transform.localPosition;

                //limit magnitude of knob distance to joystick radius
                if (this.knob.transform.localPosition.sqrMagnitude > Mathf.Pow(this.radius, 2))
                {
                    this.knob.transform.localPosition *= (this.radius / this.knob.transform.localPosition.magnitude);
                }
                break;
        }

        return false;
    }
}
