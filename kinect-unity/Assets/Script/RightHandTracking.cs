using System.Runtime.InteropServices;using UnityEngine;using System.Collections;public class RightHandTracking : MonoBehaviour {    [StructLayout(LayoutKind.Sequential)]    public struct POINT    {        public int X;        public int Y;        public POINT(int x, int y)        {            this.X = x;            this.Y = y;        }    }    private SkeletonWrapper sw;    private HandInputManager him;    private int PlayerID = 0;    private POINT mousePos;    private Vector3 rightHandPos;    private Vector3 preRightHandPos;

    public int scale = 1000;    [DllImport("user32.dll")]    public static extern bool SetCursorPos(int X, int Y);    [DllImport("user32.dll")]    public static extern bool GetCursorPos(out POINT pos);

    void OnEnable() {
        HandInputManager.handMotionDetected += SelectMenu;    }

    void OnDisable()
    {
        HandInputManager.handMotionDetected -= SelectMenu;
    }    void Awake() {        sw = SkeletonWrapper.Instance;        him = HandInputManager.Instance;    }        // Use this for initialization	void Start () {        this.rightHandPos = sw.bonePos[PlayerID, 11];        this.preRightHandPos = this.rightHandPos;        GetCursorPos(out mousePos);	}		// Update is called once per frame	void Update () {        this.rightHandPos = sw.bonePos[PlayerID, 11];
        int dx = (int)((this.rightHandPos.x - this.preRightHandPos.x)*scale);
        int dy = (int)((this.rightHandPos.z - this.preRightHandPos.z)*scale);
        mousePos.X += dx;
        mousePos.Y += dy;
        SetCursorPos(mousePos.X, mousePos.Y);
        this.preRightHandPos = this.rightHandPos;	}


    void SelectMenu(object sender, HandMotionDetectedEventArgs args) {
        if (args.motion == HandMotion.LEFT_HAND_WAVE_OUT) {
            //TODO: Change the code here, implement choosing mene function            Debug.Log("SELECT THIS MENU");        }    }}