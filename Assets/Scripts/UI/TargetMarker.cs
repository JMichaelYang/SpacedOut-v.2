using UnityEngine;

/// <summary>
/// A marker for each ship that points to its location if it is offscreen
/// </summary>
public class TargetMarker : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform origin;
    private new Camera camera;
    [SerializeField]
    private Vector2 position;
    private float iconWidth;
    private Rect drawTarget { get { return new Rect(this.position.x - this.iconWidth / 2, this.position.y - this.iconWidth / 2, this.iconWidth, this.iconWidth); } }

    private float screenWidth;
    private float screenHeight;

    private float offset;

    private Rect screenBox;

    [SerializeField]
    private Texture texture;
    private bool isVisible;

    void Awake()
    {
        this.screenWidth = Screen.width;
        this.screenHeight = Screen.height;
        this.offset = this.screenWidth * GameSettings.MarkerOffset;
        this.screenBox = new Rect(this.offset, this.offset, this.screenWidth - this.offset, this.screenHeight - this.offset);
        this.iconWidth = this.screenWidth * GameSettings.MarkerSize;

        this.position = Vector2.zero;
        this.isVisible = true;
    }

    // Use this for initialization
    void Start()
    {
        this.camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void SetTarget(Transform target, Transform origin, Texture texture)
    {
        this.target = target;
        this.origin = origin;
        this.texture = texture;
    }

    void OnEnable()
    {
        GuiHandler.Instance.RegisterOnGUI(drawMarker);
    }

    void OnDisable()
    {
        GuiHandler.Instance.DeregisterOnGUI(drawMarker);
    }

    // Drawing GUI elements
    private void drawMarker()
    {
        // Find the position of the origin and the position of the target based on the camera's coordinate system
        Vector2 origPos = camera.WorldToScreenPoint(origin.position);
        //origPos.y = this.screenHeight - origPos.y;
        Vector2 targPos = camera.WorldToScreenPoint(target.position);
        //targPos.y = this.screenHeight - targPos.y;

        // If the screen does not contain the target, draw the marker
        // TODO: lerp visibility of the indicator
        if (!this.screenBox.Contains(targPos))
        {
            // Find the difference between the target position and the origin position
            Vector2 diff = targPos - origPos;
            float xOffLeft = origPos.x - this.screenBox.x;
            float xOffRight = this.screenBox.width - origPos.x;
            float yOffBot = origPos.y - this.screenBox.y;
            float yOffTop = this.screenBox.height - origPos.y;

            Vector2 origDiff = diff;

            // First test the marker on the left/right
            if (diff.x < 0)
            {
                diff *= xOffLeft / -diff.x;
            }
            else
            {
                diff *= xOffRight / diff.x;
            }

            // If it is off of the screen to the left/right, use top/bottom
            if (diff.y > yOffTop || diff.y < -yOffBot)
            {
                if (origDiff.y < 0)
                {
                    diff = origDiff * yOffBot / -origDiff.y;
                }
                else
                {
                    diff = origDiff * yOffTop / origDiff.y;
                }
            }

            // Set the position of the target marker
            this.position = origPos + diff;
            this.position.y = this.screenHeight - this.position.y;
            GUI.DrawTexture(this.drawTarget, this.texture);
        }
        else
        {

        }
    }

    private Vector2 findEdgePosition()
    {
        return Vector2.zero;
    }
}
