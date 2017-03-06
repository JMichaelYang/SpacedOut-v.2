using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHandler : MonoBehaviour
{
    //singleton instance
    public static CommandHandler Instance = null;

    //command stream
    private List<Command> commands;

    public void AddCommands(params Command[] commands)
    {
        foreach (Command command in commands)
        {
            this.commands.Add(command);
        }
    }

    // Use this for initialization
    void Awake()
    {
        //maintain singleton pattern
        if (CommandHandler.Instance == null)
        {
            CommandHandler.Instance = this;
        }
        else if (CommandHandler.Instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        this.commands = new List<Command>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < this.commands.Count; i++)
        {
            this.commands[i].Execute();
        }

        //clear list of commands executed this update
        this.commands.Clear();
    }
}
