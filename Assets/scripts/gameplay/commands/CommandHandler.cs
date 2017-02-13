using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHandler : MonoBehaviour
{
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
    void Start()
    {
        this.commands = new List<Command>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Command command in this.commands)
        {
            command.Execute();
        }

        //clear list of commands executed this update
        this.commands.Clear();
    }
}
