using UnityEngine;
using UnityEngine.Networking;

//use QoS channel 2
[NetworkSettings(channel = 1, sendInterval = 0.03f)]
public class ShipPlayer : NetworkBehaviour
{
    //camera
    public GameObject playerCamera;

    //how many inputs to store per update
    public int inputBufferSize = 5;
    public int interpolationDelay = 10;

    [SyncVar(hook = "OnServerStateChanged")]
    public ShipState serverState;

    //state handler, changes based on whether this is the local player or not
    IShipStateHandler stateHandler;
    ShipPlayerServer server;

    void Awake()
    {
        this.AwakeOnServer();
    }

    //if we are server side, add a ShipPlayerServer component
    [Server]
    void AwakeOnServer()
    {
        this.server = this.gameObject.AddComponent<ShipPlayerServer>();
    }

    void Start()
    {
        //if this is not the client's player, add a ShipPlayerObserved component
        if (!this.isLocalPlayer)
        {
            this.stateHandler = (IShipStateHandler)this.gameObject.AddComponent<ShipPlayerObserved>();
            return;
        }

        //else add a player predicted component
        this.stateHandler = (IShipStateHandler)this.gameObject.AddComponent<ShipPlayerPredicted>();
        this.gameObject.AddComponent<ShipPlayerInput>();
        Instantiate(playerCamera).transform.SetParent(this.transform);
    }
    
    [Command(channel = 1)]
    public void CmdMove(ShipPlayerInput.Inputs[] inputs)
    {
        this.server.Move(inputs);
    }
    
    public void SyncState(ShipState stateToUse)
    {
        this.transform.position = stateToUse.position;
        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, stateToUse.rotation);
    }

    void OnServerStateChanged(ShipState newState)
    {
        this.serverState = newState;

        if (this.stateHandler == null)
        {
            return;
        }

        this.stateHandler.OnStateChanged(this.serverState);
    }
}
