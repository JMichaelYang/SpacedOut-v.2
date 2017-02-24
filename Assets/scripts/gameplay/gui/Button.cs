using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    //the current value of the button
    private bool isPressed;
    public bool IsPressed
    {
        get
        {
            return this.isPressed;
        }
        protected set
        {
            this.isPressed = value;

            //setting textures
            this.gameObject.GetComponent<SpriteRenderer>().sprite = this.isPressed ? this.downTexture : this.upTexture;
        }
    }

    //button position and size
    private float width;
    private float height;

    private float opacity;

    //button sprite
    private Sprite upTexture;
    private Sprite downTexture;

    // Use this for initialization
    void Start()
    {
        this.IsPressed = false;
    }

    /// <summary>
    /// Method to set the values of the button
    /// </summary>
    /// <param name="x">The X position of the button</param>
    /// <param name="y">The Y position of the button</param>
    /// <param name="width">The width of the button</param>
    /// <param name="height">The height of the button</param>
    /// <param name="upTexture">The texture of the button when not pressed</param>
    /// <param name="downTexture">The texture of the button when pressed</param>
    public void Initialize(float width, float height, Sprite upTexture, Sprite downTexture, float opacity)
    {
        this.width = width;
        this.height = height;

        this.upTexture = upTexture;
        this.transform.localScale = new Vector3(this.width / this.upTexture.bounds.size.x, this.height / this.upTexture.bounds.size.y, 1f);
        this.downTexture = downTexture;
        this.IsPressed = false;

        this.opacity = opacity;
        this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, this.opacity);
    }

    // Update is called once per frame
    void Update() { }

    /// <summary>
    /// Updates the button
    /// </summary>
    /// <param name="touch">The current touch that is interacting with the button</param>
    /// <returns>Returns true if the button is pressed in bounds</returns>
    public bool UpdateButton(Touch touch)
    {
        float localTouchX = Utils.ConvertScale(touch.position.x, 0, Screen.width, -6.5f, 6.5f);
        float localTouchY = Utils.ConvertScale(touch.position.y, 0, Screen.height, -11.5f, 11.5f);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                if (localTouchX > this.transform.localPosition.x - (this.width / 2) && localTouchX < this.transform.localPosition.x + (this.width / 2) 
                    && localTouchY > this.transform.localPosition.y - (this.height / 2) && localTouchY < this.transform.localPosition.y + (this.height / 2))
                {
                    this.IsPressed = true;
                    return true;
                }
                else
                    this.IsPressed = false;
                break;

            case TouchPhase.Ended:
                this.IsPressed = false;
                break;

            default:
                if (localTouchX > this.transform.localPosition.x - (this.width / 2) && localTouchX < this.transform.localPosition.x + (this.width / 2)
                    && localTouchY > this.transform.localPosition.y - (this.height / 2) && localTouchY < this.transform.localPosition.y + (this.height / 2))
                    this.IsPressed = true;
                else
                    this.IsPressed = false;
                break;
        }

        return false;
    }
}
