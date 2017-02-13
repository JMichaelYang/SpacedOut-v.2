using UnityEngine;
using System.Collections.Generic;

public class ShipPlayerInput : MonoBehaviour
{
    List<Inputs> inputBuffer;
    ShipPlayer player;
    ShipPlayerPredicted predicted;

    public struct Inputs
    {
        public float forward;
        public float strafe;
        public float rotation;
    }

    void Awake()
    {
        this.inputBuffer = new List<Inputs>();
        this.player = this.GetComponent<ShipPlayer>();
        this.predicted = this.GetComponent<ShipPlayerPredicted>();
    }

    void FixedUpdate()
    {
        Inputs input = new Inputs { forward = Input.GetAxis("Vertical"), rotation = -Input.GetAxis("Horizontal") };

        //if no input
        if ((inputBuffer.Count == 0) && (input.forward == 0) && (input.rotation == 0))
        {
            return;
        }

        this.predicted.AddInput(input);
        this.inputBuffer.Add(input);

        if (this.inputBuffer.Count < this.player.inputBufferSize)
        {
            return;
        }

        this.player.CmdMove(this.inputBuffer.ToArray());
        this.inputBuffer.Clear();
    }
}
