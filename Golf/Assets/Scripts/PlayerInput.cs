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

    private static Sprite[] sprites = null;
    private static string spritePath = "ControlTipsUI";

    private static int[] spriteIndices =
    {
        7,
        6,
        8,
        9,
        0,
        1,
        10,
        4,
        5,
        2,
        3,
    };

    private static float[] axesValue = new float[(int)Axis.MAX_AXIS];
    private static bool[] axesFrameDown = new bool[(int)Axis.MAX_AXIS];
    private static bool[] axesFrameUp = new bool[(int)Axis.MAX_AXIS];

    private void OnEnable()
    {
        clearInput();
    }

    private void clearInput()
    {
        for (int i = 0; i < (int)Axis.MAX_AXIS; i++)
        {
            axesValue[i] = 0;
            axesValue[i] = 0;
            axesFrameDown[i] = false;
            axesFrameDown[i] = false;
            axesFrameUp[i] = false;
            axesFrameUp[i] = false;
        }
    }

    private static void loadSprites()
    {

        if (sprites != null)
        {
            return;
        }

        sprites = Resources.LoadAll<Sprite>(spritePath);

    }

    public static Sprite getSprite(Axis axis)
    {
        loadSprites();
        return sprites[spriteIndices[(int)axis]];
    }

    public static Axis getType(string axis)
    {
        Axis type = Axis.Fire1;
        for (int i = 0; i < axesNames.Length; i++)
        {
            if (axesNames[i].Equals(axis))
            {
                type = (Axis)i;
                break;
            }
        }
        return type;
    }

    void Update()
    {

        switch(Input.touchCount)
        {
            case 0:
                updateValue(Axis.Fire1, 0);
                updateValue(Axis.Fire2, 0);
                updateValue(Axis.ScrollWheel, 0);
                break;
            case 1:
                updateValue(Axis.Fire1, 1);
                updateValue(Axis.Fire2, 0);
                updateValue(Axis.ScrollWheel, 0);
                break;
            case 2:
                handleZoom();
                break;
        }

    }

    private void handleZoom()
    {
        
        Touch touch0 = Input.GetTouch(0);
        Touch touch1 = Input.GetTouch(1);
        Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
        Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
        float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
        float touchDeltaMag = (touch0.position - touch1.position).magnitude;
        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
        if (Mathf.Abs(deltaMagnitudeDiff) < 10)
        {
            updateValue(Axis.Fire2, 1);
            updateValue(Axis.ScrollWheel, 0);
        }
        else
        {
            float strength = Mathf.Clamp(Mathf.Abs(deltaMagnitudeDiff) / 2000f, 0f, 1f);
            updateValue(Axis.ScrollWheel, deltaMagnitudeDiff > 0f ? -strength : strength);
            updateValue(Axis.Fire1, 0);
            updateValue(Axis.Fire2, 0);
        }

    }

    private void updateValue(Axis axis, float value)
    {
        int index = (int)axis;
        axesFrameDown[index] = Mathf.Approximately(axesValue[index], 0f) && !Mathf.Approximately(value, 0f);
        axesFrameUp[index] = !Mathf.Approximately(axesValue[index], 0f) && Mathf.Approximately(value, 0f);
        axesValue[index] = value;
    }

    public static bool isDown(Axis axis)
    {
        return axesFrameDown[(int)axis];
    }

    public static bool isUp(Axis axis)
    {
        return axesFrameUp[(int)axis];
    }

    public static float get(Axis axis)
    {
        return axesValue[(int)axis];
    }

    public static Vector2 cursorPosition { get
        {
            return Input.mousePosition;
        }
    }

}
