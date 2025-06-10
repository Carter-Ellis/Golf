using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement : MonoBehaviour
{
    public enum TYPE
    {
        HOLEINNONE,
        HOLEINONE,
        BEAT_CAMP_HARDCORE,
        MAX
    }

    private static string[] names =
    {
        "Hole In None",
        "Hole In One",
        "Hardcore Campaign",
    };

    private static string[] descriptions =
    {
        "Complete a hole without ever hitting the ball. Clubless mode does not count.",
        "Complete a hole with only one stroke.",
        "Complete a hardcore campaign run."
    };

    public static string GetName(TYPE type)
    {
        return names[(int)type];
    }

    public static string GetDescription(TYPE type)
    {
        return descriptions[(int)type];
    }

}
