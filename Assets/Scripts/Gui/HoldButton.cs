using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldButton : Button
{
    private bool isHeld = false;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        this.isHeld = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        this.isHeld = false;
    }

    void Update()
    {
        if (this.isHeld)
        {
            this.onClick.Invoke();
        }
    }
}
