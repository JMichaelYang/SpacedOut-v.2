using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : Button
{
    //the current x and y values for the joystick (between -1 and 1)
    public float ValueX { get { return this.knobTransform.anchoredPosition.x / this.radius; } }
    public float ValueY { get { return this.knobTransform.anchoredPosition.y / this.radius; } }

    private float centerX { get { return (this.minX + this.maxX) / 2; } }
    private float centerY { get { return (this.minY + this.maxY) / 2; } }

    //if the joystick is currently being pressed
    private bool isPressed = false;

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

    //base and knob object
    public GameObject BaseObject;
    public GameObject KnobObject;
    private Image baseImage;
    private RectTransform baseTransform;
    private Image knobImage;
    private RectTransform knobTransform;

    /// <summary>
    /// Method to set the bounds of the joystick's original touch
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

        Color baseColor = this.baseImage.color;
        baseColor.a = this.baseOpacity;
        this.baseImage.color = baseColor;

        Color knobColor = this.knobImage.color;
        knobColor.a = this.knobOpacity;
        this.knobImage.color = knobColor;
    }

    // Use this for initialization
    protected override void Awake()
    {
        this.isPressed = false;

        //set the real radius of the object
        this.radius = GameSettings.JoystickRadius * Screen.width;

        //find the objects that constitute this joystick
        this.BaseObject = GameObject.Find("JoystickBase");
        this.KnobObject = GameObject.Find("JoystickKnob");

        //get the base and knob components
        this.baseImage = this.BaseObject.GetComponent<Image>();
        this.baseTransform = this.BaseObject.GetComponent<RectTransform>();
        this.knobImage = this.KnobObject.GetComponent<Image>();
        this.knobTransform = this.KnobObject.GetComponent<RectTransform>();

        //set opacity of the joystick
        this.SetOpacity(GameSettings.JoystickBaseOpacity, GameSettings.JoystickKnobOpacity);

        //set the positions of the base and knob
        this.baseTransform.anchorMin = new Vector2((this.minX + this.maxX) / 2, (this.minY + this.maxY) / 2);
        this.baseTransform.anchorMax = new Vector2((this.minX + this.maxX) / 2, (this.minY + this.maxY) / 2);
        this.knobTransform.anchorMin = new Vector2((this.minX + this.maxX) / 2, (this.minY + this.maxY) / 2);
        this.knobTransform.anchorMax = new Vector2((this.minX + this.maxX) / 2, (this.minY + this.maxY) / 2);
        //set the size of the base and knob
        this.baseTransform.sizeDelta = new Vector2(GameSettings.JoystickPixelWidth, GameSettings.JoystickPixelHeight);
        this.knobTransform.sizeDelta = new Vector2(GameSettings.JoystickPixelWidth / 2, GameSettings.JoystickPixelHeight / 2);
        //set the positions to match the anchors
        this.baseTransform.anchoredPosition = Vector2.zero;
        this.knobTransform.anchoredPosition = Vector2.zero;

        //hide joystick if it can dissapear
        if (GameSettings.JoystickDisappear)
        {
            this.baseTransform.localScale = Vector3.zero;
            this.knobTransform.localScale = Vector3.zero;
        }
    }

    //initiate knob update
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        Vector2 newPress = eventData.pressPosition;
        float sWidth = Screen.width;
        float sHeight = Screen.height;

        //check whether press is within our bounds
        if (newPress.x / sWidth > this.minX &&
            newPress.x / sWidth < this.maxX &&
            newPress.y / sHeight > this.minY &&
            newPress.y / sHeight < this.maxY)
        {
            this.isPressed = true;

            if (GameSettings.JoystickDisappear)
            {
                this.baseTransform.localScale = Vector3.one;
                this.knobTransform.localScale = Vector3.one;
            }

            float pressX = newPress.x - (this.centerX * sWidth);
            float pressY = newPress.y - (this.centerY * sHeight);

            this.knobTransform.anchoredPosition = new Vector2(pressX, pressY);
        }
    }

    //reset knob position
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        this.isPressed = true;

        this.knobTransform.anchoredPosition = Vector2.zero;

        if (GameSettings.JoystickDisappear)
        {
            this.baseTransform.localScale = Vector3.zero;
            this.knobTransform.localScale = Vector3.zero;
        }
    }
}
