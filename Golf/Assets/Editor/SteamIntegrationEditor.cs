using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SteamIntegration))]
public class SteamIntegrationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SteamIntegration script = (SteamIntegration)target;


        if (GUILayout.Button("Check Achievement Status"))
        {
            SteamIntegration.IsThisSteamAchUnlocked(script.testAchievementID);
        }

        if (GUILayout.Button("Unlock Achievement"))
        {
            SteamIntegration.UnlockSteamAch(script.testAchievementID);
        }

        if (GUILayout.Button("Clear Achievement"))
        {
            SteamIntegration.ClearSteamAchStatus(script.testAchievementID);
        }
    }
}
