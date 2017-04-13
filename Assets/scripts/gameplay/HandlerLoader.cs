using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlerLoader : MonoBehaviour
{
    public GameObject gameHandler;
    public GameObject inputHandler;
    public GameObject commandHandler;
    public GameObject guiHandler;

    // Use this for initialization
    void Awake()
    {
        if (GuiHandler.Instance == null) { Instantiate(this.guiHandler); }
        if (DeathmatchGameHandler.Instance == null) { Instantiate(this.gameHandler); }
        if (InputHandler.Instance == null) { Instantiate(this.inputHandler); }
        if (CommandHandler.Instance == null) { Instantiate(this.commandHandler); }
    }
}
