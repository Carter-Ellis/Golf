using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode
{

    public static TYPE current;

    public enum TYPE
    {
        HOLE18,
        FREEPLAY,
        SPEEDRUN,
        CLUBLESS,
        HARDCORE,
        MAX
    }

}
