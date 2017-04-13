using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiHandler : MonoBehaviour
{
    // Singleton instance of a GuiHandler
    public static GuiHandler Instance;

    // List containing the various functions to be run
    public delegate void guiDraw();
    private List<guiDraw> drawFunctions;

    void Awake()
    {
        //maintain singleton pattern
        if (GuiHandler.Instance == null)
        {
            GuiHandler.Instance = this;
        }
        else if (GuiHandler.Instance != this)
        {
            Destroy(this.gameObject);
        }

        this.drawFunctions = new List<guiDraw>();
    }

    public void RegisterOnGUI(guiDraw draw)
    {
        this.drawFunctions.Add(draw);
    }

    public void DeregisterOnGUI(guiDraw draw)
    {
        this.drawFunctions.Remove(draw);
    }

    void OnGUI()
    {
        for (int i = 0; i < this.drawFunctions.Count; i++)
        {
            this.drawFunctions[i].Invoke();
        }
    }
}
