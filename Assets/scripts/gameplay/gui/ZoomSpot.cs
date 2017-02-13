using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomSpot : MonoBehaviour
{
    //the current value of the swipe
    private float currentValue = 0;
    private float oldValue = 0;
    private float startingY = 0;
    public float GetValue() { return this.currentValue - this.startingY; }
    public float GetUnitValue()
    {
        if (this.currentValue > this.startingY)
            return 1;
        else if (this.currentValue < this.startingY)
            return -1;
        else
            return 0;
    }
    public float GetDelta() { return (this.currentValue - this.oldValue) * GameSettings.ZoomSpotMultiplier; }

    //max swiping zoom distance
    public float maxDistance;

    //if this spot is currently active
    public bool IsActive { get; protected set; }

    // Use this for initialization
    void Start()
    {
        this.IsActive = false;
        this.maxDistance = GameSettings.ZoomMaxSwipe;
    }

    // Update is called once per frame
    void Update() { }

    public bool UpdateSpot(Touch touch)
    {
        float localTouchX = Utils.ConvertScale(touch.position.x, 0, Screen.width, -6.5f, 6.5f);
        float localTouchY = Utils.ConvertScale(touch.position.y, 0, Screen.height, -11.5f, 11.5f);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                if (localTouchX > GameSettings.ZoomSpotMinX && localTouchX < GameSettings.ZoomSpotMaxX
                    && localTouchY > GameSettings.ZoomSpotMinY && localTouchY < GameSettings.ZoomSpotMaxY)
                {
                    this.IsActive = true;
                    this.currentValue = localTouchY;
                    this.startingY = localTouchY;
                    this.oldValue = localTouchY;
                    return true;
                }
                break;

            case TouchPhase.Ended:
                this.IsActive = false;
                this.currentValue = 0;
                this.startingY = 0;
                this.oldValue = 0;
                break;

            default:
                this.oldValue = this.currentValue;
                this.currentValue = localTouchY;
                break;
        }

        return false;
    }
}
