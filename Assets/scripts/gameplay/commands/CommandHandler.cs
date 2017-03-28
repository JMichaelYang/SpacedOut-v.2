using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHandler : MonoBehaviour
{
    //singleton instance
    public static CommandHandler Instance = null;
    
    //command stream
    private Queue<Command> commands;
    
    public void AddCommands(params Command[] commands)
    {
        int len = commands.Length;
        for (int i = 0; i < len; i++)
        {
            this.commands.Enqueue(commands[i]);
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

        this.commands = new Queue<Command>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < this.commands.Count; i++)
        {
            this.commands.Dequeue().Execute();
        }
    }
}
