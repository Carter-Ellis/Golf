using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    
    public enum Axis
    {
        Horizontal,
        Vertical,
        Fire1,
        Fire2,
        Fire3,
        Fire4,
        ScrollWheel,
        SwapUp,
        SwapDown,
        Cancel,
        Reset,
        MAX_AXIS
    }

    private static string controllerSuffix = "_controller";

    private static string[] axesNames =
    {
        "Horizontal",
        "Vertical",
        "Fire1",
        "Fire2",
        "Fire3",
        "Fire4",
        "ScrollWheel",
        "SwapUp",
        "SwapDown",
        "Cancel",
         "Reset",
    };

    private static float[,] axesValue = new float[(int)Axis.MAX_AXIS, 2];
    private static bool[,] axesFrameDown = new bool[(int)Axis.MAX_AXIS, 2];
    private static bool[,] axesFrameUp = new bool[(int)Axis.MAX_AXIS, 2];

    public static bool isController { get; private set; }
    public static float cursorSpeed = 0.6f;

    private static Vector2 _cursorPos = new Vector2(0.5f, 0.5f);
    private const float defaultCursorSpeed = 0.6f;

    private void OnEnable()
    {
        cursorSpeed = defaultCursorSpeed;
        for (int i = 0; i < (int)Axis.MAX_AXIS; i++)
        {
            axesValue[i, 0] = 0;
            axesValue[i, 1] = 0;
            axesFrameDown[i, 0] = false;
            axesFrameDown[i, 1] = false;
            axesFrameUp[i, 0] = false;
            axesFrameUp[i, 1] = false;
        }
        resetCursor();
    }

    void Update()
    {
        
        bool usedKey = false;
        bool usedController = false;

        for (int i = 0; i < axesValue.GetLength(0); i++)
        {

            float value = Input.GetAxis(axesNames[i]);
            axesFrameDown[i, 0] = Mathf.Approximately(axesValue[i, 0], 0f) && !Mathf.Approximately(value, 0f);
            axesFrameUp[i, 0] = !Mathf.Approximately(axesValue[i, 0], 0f) && Mathf.Approximately(value, 0f);
            axesValue[i, 0] = value;
            if (axesFrameDown[i, 0])
            {
                usedKey = true;
            }

            value = Input.GetAxis(axesNames[i] + controllerSuffix);
            axesFrameDown[i, 1] = Mathf.Approximately(axesValue[i, 1], 0f) && !Mathf.Approximately(value, 0f);
            axesFrameUp[i, 1] = !Mathf.Approximately(axesValue[i, 1], 0f) && Mathf.Approximately(value, 0f);
            axesValue[i, 1] = value;
            if (axesFrameDown[i, 1])
            {
                usedController = true;
            }

        }

        if (!usedKey && usedController)
        {
            if (!isController)
            {
                resetCursor(); //Reset when switching to controller
            }
            isController = true;
        }
        else if (usedKey && !usedController)
        {
            isController = false;
        }

        if (isController)
        {
            Vector2 joystick = new Vector2(get(Axis.Horizontal), get(Axis.Vertical));
            if (joystick.magnitude >= 0.1f)
            {
                _cursorPos += joystick * Time.deltaTime * cursorSpeed;
                _cursorPos = new Vector2(Mathf.Clamp(_cursorPos.x, 0, 1), Mathf.Clamp(_cursorPos.y, 0, 1));
            }
        }

    }

    public static bool isDown(Axis axis)
    {
        return axesFrameDown[(int)axis, isController ? 1 : 0];
    }

    public static bool isUp(Axis axis)
    {
        return axesFrameUp[(int)axis, isController ? 1 : 0];
    }

    public static float get(Axis axis)
    {
        return axesValue[(int)axis, isController ? 1 : 0];
    }

    public static Vector2 cursorPosition { get
        {
            return isController ? (_cursorPos * new Vector2(Screen.width, Screen.height)) : Input.mousePosition;
        }
    }

    public static Vector2 rawCursorPosition { get
        {
            return isController ? _cursorPos : Input.mousePosition;
        }
    }

    public static void resetCursor()
    {
        _cursorPos = new Vector2(0.5f, 0.5f);
    }

}
