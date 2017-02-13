using UnityEngine;
using System.Collections.Generic;

public class ShipPlayerPredicted : MonoBehaviour, IShipStateHandler
{
    Queue<ShipPlayerInput.Inputs> pendingMoves;
    ShipPlayer player;
    ShipState predictedState;

    void Awake()
    {
        this.pendingMoves = new Queue<ShipPlayerInput.Inputs>();
        this.player = this.GetComponent<ShipPlayer>();
        this.UpdatePredictedState();
    }

    public void AddInput(ShipPlayerInput.Inputs input)
    {
        this.pendingMoves.Enqueue(input);
        this.UpdatePredictedState();
    }

    public void OnStateChanged(ShipState newState)
    {
        //get rid of moves that server had caught up to
        while (this.pendingMoves.Count > (this.predictedState.moveNum - this.player.serverState.moveNum))
        {
            this.pendingMoves.Dequeue();
        }

        this.UpdatePredictedState();
    }

    //reconcile with server then apply updates
    void UpdatePredictedState()
    {
        this.predictedState = this.player.serverState;
        foreach (ShipPlayerInput.Inputs input in this.pendingMoves)
        {
            this.predictedState = ShipState.Move(this.predictedState, input, 0);
        }

        this.player.SyncState(this.predictedState);
    }
}
