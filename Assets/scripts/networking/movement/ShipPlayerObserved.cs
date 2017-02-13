using UnityEngine;
using System.Collections.Generic;

public class ShipPlayerObserved : MonoBehaviour, IShipStateHandler
{
    float clientTick;
    ShipState interpolated;
    ShipPlayer player;
    LinkedList<ShipState> stateBuffer;

    void Awake()
    {
        this.player = this.GetComponent<ShipPlayer>();
        this.stateBuffer = new LinkedList<ShipState>();
        this.SetObservedState(this.player.serverState);
        this.AddState(this.player.serverState);
    }

    void FixedUpdate()
    {
        this.clientTick++;
        LinkedListNode<ShipState> fromNode = this.stateBuffer.First;
        LinkedListNode<ShipState> toNode = fromNode.Next;

        while ((toNode != null) && (toNode.Value.timeStamp <= this.clientTick))
        {
            fromNode = toNode;
            toNode = fromNode.Next;
            this.stateBuffer.RemoveFirst();
        }

        this.SetObservedState(toNode != null ? ShipState.Interpolate(fromNode.Value, toNode.Value, this.clientTick) : fromNode.Value);
    }

    public void OnStateChanged(ShipState newState)
    {
        this.AddState(newState);
    }

    void AddState(ShipState state)
    {
        this.stateBuffer.AddLast(state);
        this.clientTick = state.timeStamp - this.player.interpolationDelay;

        while (this.stateBuffer.First.Value.timeStamp <= this.clientTick)
        {
            this.stateBuffer.RemoveFirst();
        }

        this.interpolated.timeStamp = Mathf.Max(this.clientTick, this.stateBuffer.First.Value.timeStamp - this.player.inputBufferSize);
        this.stateBuffer.AddFirst(this.interpolated);

        if (this.interpolated.timeStamp <= this.clientTick)
        {
            return;
        }

        this.interpolated.timeStamp = this.clientTick;
        this.stateBuffer.AddFirst(this.interpolated);
    }

    void SetObservedState(ShipState newState)
    {
        this.interpolated = newState;
        this.player.SyncState(this.interpolated);
    }
}
