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
        "ScrollWheel"
    };

    private static float[,] axesValue = new float[(int)Axis.MAX_AXIS, 2];
    private static bool[,] axesFrameDown = new bool[(int)Axis.MAX_AXIS, 2];

    public static bool isController { get; private set; }

    void Update()
    {
        
        bool usedKey = false;
        bool usedController = false;

        for (int i = 0; i < axesValue.GetLength(0); i++)
        {

            float value = Input.GetAxis(axesNames[i]);
            axesFrameDown[i, 0] = (axesValue[i, 0] == 0f) && (value != 0);
            axesValue[i, 0] = value;
            if (axesFrameDown[i, 0])
            {
                usedKey = true;
            }

            value = Input.GetAxis(axesNames[i] + controllerSuffix);
            axesFrameDown[i, 1] = (axesValue[i, 1] == 0f) && (value != 0);
            axesValue[i, 1] = value;
            if (axesFrameDown[i, 1])
            {
                usedController = true;
            }

        }

        if (!usedKey && usedController)
        {
            isController = true;
        }
        else if (usedKey && !usedController)
        {
            isController = false;
        }
    }

    public static bool isDown(Axis axis)
    {
        return axesFrameDown[(int)axis, isController ? 1 : 0];
    }

    public static float get(Axis axis)
    {
        return axesValue[(int)axis, isController ? 1 : 0];
    }

    public static Vector2 cursorPosition { get
        {
            if (isController)
            {
                return new Vector2((get(Axis.Horizontal) + 1) * Screen.width * 0.5f, (get(Axis.Vertical) + 1) * Screen.height * 0.5f);
            }
            else
            {
                return Input.mousePosition;
            }
        }
    }

}
