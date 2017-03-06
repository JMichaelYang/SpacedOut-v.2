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

    //bounds of the joystick
    private float minX = GameSettings.JoystickMinX;
    private float minY = GameSettings.JoystickMinY;
    private float maxX = GameSettings.JoystickMaxX;
    private float maxY = GameSettings.JoystickMaxY;
    //max radius of the joystick
    private float radius = GameSettings.JoystickRadius;

    //opacity
    private float baseOpacity = GameSettings.JoystickBaseOpacity;
    private float knobOpacity = GameSettings.JoystickKnobOpacity;

    //knob object
    public GameObject knob;
    
    //if this joystick is currently active
    public bool IsActive { get; protected set; }

    /// <summary>
    /// Method to set the bounds of the joystick
    /// </summary>
    /// <param name="minX">Minimum X value of the joystick</param>
    /// <param name="minY">Minimum Y value of the joystick</param>
    /// <param name="maxX">Maximum X value of the joystick</param>
    /// <param name="maxY">Maximum Y value of the joystick</param>
    /// <param name="radius">Maximum radius of the joystick</param>
    public void SetBounds(float minX, float minY, float maxX, float maxY, float radius)
    {
        this.minX = minX;
        this.minY = minY;
        this.maxX = maxX;
        this.maxY = maxY;

        this.radius = radius;
    }
    /// <summary>
    /// Method to set the opacity of the joystick
    /// </summary>
    /// <param name="baseOpacity">The opacity of the base</param>
    /// <param name="knobOpacity">The opacity of the knob</param>
    public void SetOpacity(float baseOpacity, float knobOpacity)
    {
        this.baseOpacity = baseOpacity;
        this.knobOpacity = knobOpacity;
        
        this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, this.baseOpacity);
        this.knob.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, this.knobOpacity);
    }

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

        this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, this.baseOpacity);
        this.knob.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, this.knobOpacity);
    }

    /// <summary>
    /// Updates the joystick
    /// </summary>
    /// <param name="touch">The touch object that is interacting with the joystick</param>
    /// <returns>Returns true if the initial touch lands within bounds, otherwise returns false</returns>
    public bool UpdateJoystick(Touch touch)
    {
        float localTouchX = Utils.ConvertScale(touch.position.x, 0, Screen.width, -6.5f, 6.5f);
        float localTouchY = Utils.ConvertScale(touch.position.y, 0, Screen.height, -11.5f, 11.5f);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                if (localTouchX > this.minX && localTouchX < this.maxX && localTouchY > this.minY && localTouchY < this.maxY)
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
                if (this.knob.transform.localPosition.sqrMagnitude > this.radius * this.radius)
                {
                    this.knob.transform.localPosition *= (this.radius / this.knob.transform.localPosition.magnitude);
                }
                break;
        }

        return false;
    }
}
