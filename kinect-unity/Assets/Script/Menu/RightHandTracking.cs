using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;


public class RightHandTracking : MonoBehaviour {

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT {
        public int X;
        public int Y;

        public POINT(int x, int y) {
            this.X = x;
            this.Y = y;
        }
    }

	public enum MouseEventFlags {
		LeftDown	= 0x00000002,
		LeftUp 		= 0x00000004,
		MiddleDown 	= 0x00000020,
		MiddleUp 	= 0x00000040,
		Move 		= 0x00000001,
		Absolute 	= 0x00008000,
		RightDown 	= 0x00000008,
		RightUp 	= 0x00000010
	}

    private SkeletonWrapper sw;
    private HandInputManager him;
    private int PlayerID = 0;

    private POINT mousePos;
    private Vector3 rightHandPos;
    private Vector3 preRightHandPos;

    public int scale = 1000;

    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);
    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT pos);
	[DllImport("user32.dll")]
	private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

    void OnEnable() {
        HandInputManager.handMotionDetected += SelectMenu;
    }

    void OnDisable()
    {
        HandInputManager.handMotionDetected -= SelectMenu;
    }

	void Awake() {
        sw = SkeletonWrapper.Instance;
        him = HandInputManager.Instance;
    }
    
    // Use this for initialization
	void Start () {
        //this.rightHandPos = sw.bonePos[PlayerID, 11];
        this.preRightHandPos = this.rightHandPos;
        GetCursorPos(out mousePos);
	}
	
	// Update is called once per frame
	void Update () {
        this.rightHandPos = sw.bonePos[PlayerID, 11];
        int dx = (int)((this.rightHandPos.x - this.preRightHandPos.x)*scale);
        int dy = (int)((this.rightHandPos.z - this.preRightHandPos.z)*scale);
        mousePos.X += dx;
        mousePos.Y += dy;
        SetCursorPos(mousePos.X, mousePos.Y);
        this.preRightHandPos = this.rightHandPos;
	}

    void SelectMenu(object sender, HandMotionDetectedEventArgs args) {
        if (args.motion == HandMotion.LEFT_HAND_WAVE_OUT) {
            //TODO: Change the code here, implement choosing mene function
			// simulate mouse click
			MouseEvent(RightHandTracking.MouseEventFlags.LeftUp | RightHandTracking.MouseEventFlags.LeftDown);
            Debug.Log("SELECT THIS MENU");
        }
    }

	// TODO static ?
	public void MouseEvent(MouseEventFlags value) {
		Debug.Log("MouseEvent");
		mouse_event(
			(int)value,
			 mousePos.X,
			 mousePos.Y,
			 0,
			 0);
	}
}
