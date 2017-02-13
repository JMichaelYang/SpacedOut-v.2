using UnityEngine;

//struct to store ship state
public struct ShipState
{
    public int moveNum;
    public Vector2 position;
    public float rotation;
    public float timeStamp;

    public static ShipState CreateStartingState()
    {
        return new ShipState
        {
            moveNum = 0,
            position = Vector2.zero,
            rotation = 0f,
            timeStamp = 0f
        };
    }

    public static ShipState Interpolate(ShipState from, ShipState to, float clientTick)
    {
        float t = (clientTick - from.timeStamp) / (to.timeStamp - from.timeStamp);

        return new ShipState
        {
            moveNum = 0,
            position = Vector2.Lerp(from.position, to.position, t),
            rotation = Mathf.Lerp(from.rotation, to.rotation, t),
            timeStamp = 0f
        };
    }

    public static ShipState Move(ShipState previous, ShipPlayerInput.Inputs input, float timeStamp)
    {
        return new ShipState
        {
            moveNum = previous.moveNum + 1,
            //moves at a speed of 0.125f, move then rotation
            position = 0.125f * GetPositionChange(previous.rotation, input.forward) + previous.position,
            rotation = previous.rotation + input.rotation,
            timeStamp = timeStamp
        };
    }

    public static Vector2 GetPositionChange(float rotation, float forward)
    {
        return new Vector2(Mathf.Cos((rotation + 90f) * Mathf.Deg2Rad) * forward, Mathf.Sin((rotation + 90f) * Mathf.Deg2Rad) * forward);
    }
}
