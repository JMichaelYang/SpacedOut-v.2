using UnityEngine;
using System.Collections.Generic;

//ship script that runs on server
public class ShipPlayerServer : MonoBehaviour
{
    Queue<ShipPlayerInput.Inputs> inputBuffer;
    int movesMade;
    ShipPlayer player;
    float serverTick;

    void Awake()
    {
        this.inputBuffer = new Queue<ShipPlayerInput.Inputs>();
        this.player = this.GetComponent<ShipPlayer>();
        this.player.serverState = ShipState.CreateStartingState();
    }

    //apply all inputs
    void FixedUpdate()
    {
        this.serverTick++;

        if (this.movesMade > 0)
        {
            this.movesMade--;
        }

        if (this.movesMade == 0)
        {
            ShipState serverState = this.player.serverState;

            while ((this.movesMade < player.inputBufferSize) && (inputBuffer.Count > 0))
            {
                serverState = ShipState.Move(serverState, this.inputBuffer.Dequeue(), this.serverTick);
                this.movesMade++;
            }

            if (movesMade > 0)
            {
                this.player.serverState = serverState;
            }
        }
    }

    //add inputs to queue
    public void Move(ShipPlayerInput.Inputs[] inputs)
    {
        foreach (ShipPlayerInput.Inputs input in inputs)
        {
            this.inputBuffer.Enqueue(input);
        }
    }
}
