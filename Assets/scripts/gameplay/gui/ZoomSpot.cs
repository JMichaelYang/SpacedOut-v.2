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
    
    //bounds of the spot
    private float minX = GameSettings.ZoomSpotMinX;
    private float minY = GameSettings.ZoomSpotMinY;
    private float maxX = GameSettings.ZoomSpotMaxX;
    private float maxY = GameSettings.ZoomSpotMaxY;
    //max swiping zoom distance
    private float maxDistance = GameSettings.ZoomMaxSwipe;

    //if this spot is currently active
    public bool IsActive { get; protected set; }

    // Use this for initialization
    void Start()
    {
        this.IsActive = false;
        this.maxDistance = GameSettings.ZoomMaxSwipe;
    }

    /// <summary>
    /// Method to set the bounds of the zoom spot
    /// </summary>
    /// <param name="minX">Minimum X of the zoom spot</param>
    /// <param name="minY">Minimum Y of the zoom spot</param>
    /// <param name="maxX">Maximum X of the zoom spot</param>
    /// <param name="maxY">Maximum Y of the zoom spot</param>
    /// <param name="maxDistance">Maximum swipe distance for the zoom spot</param>
    public void SetBounds(float minX, float minY, float maxX, float maxY, float maxDistance)
    {
        this.minX = minX;
        this.minY = minY;
        this.maxX = maxX;
        this.maxY = maxY;

        this.maxDistance = maxDistance;
    }

    // Update is called once per frame
    void Update() { }

    /// <summary>
    /// Updates the zoom spot
    /// </summary>
    /// <param name="touch">The touch object that is interacting with the zoom spot</param>
    /// <returns>Returns true if the initial touch lands within bounds, otherwise returns false</returns>
    public bool UpdateSpot(Touch touch)
    {
        float localTouchX = Utils.ConvertScale(touch.position.x, 0, Screen.width, -6.5f, 6.5f);
        float localTouchY = Utils.ConvertScale(touch.position.y, 0, Screen.height, -11.5f, 11.5f);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                if (localTouchX > this.minX && localTouchX < this.maxX && localTouchY > this.minY && localTouchY < this.maxY)
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
