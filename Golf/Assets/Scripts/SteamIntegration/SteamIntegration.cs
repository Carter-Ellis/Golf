using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamIntegration : MonoBehaviour
{
    public Achievement.TYPE testAchievementID;
    private static bool steamInitialized = false;

    void Awake()
    {
        var steamManagers = FindObjectsOfType<SteamIntegration>();
        if (steamManagers.Length > 1)
        {
            Destroy(this);
            return;
        }

        if (!steamInitialized)
        {
            try
            {
                Steamworks.SteamClient.Init(3812820); // Use your actual AppID
                steamInitialized = Steamworks.SteamClient.IsValid;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Steam init failed: " + e.Message);
                steamInitialized = false;
            }
        }
    }

    private void Update()
    {
        if (steamInitialized)
        {
            Steamworks.SteamClient.RunCallbacks();
        }
    }

    private void OnApplicationQuit()
    {
        if (steamInitialized)
        {
            Steamworks.SteamClient.Shutdown();
            steamInitialized = false;
        }
    }

    public static bool IsThisSteamAchUnlocked(Achievement.TYPE type)
    {
        if (!steamInitialized) return false;

        string id = type.ToString();
        var ach = new Steamworks.Data.Achievement(id);
        //Debug.Log($"Achievement {id} status: " + ach.State);
        return ach.State;
    }

    public static void UnlockSteamAch(Achievement.TYPE type)
    {
        if (!steamInitialized) return;

        string id = type.ToString();
        var ach = new Steamworks.Data.Achievement(id);
        ach.Trigger();

        Debug.Log($"Achievement {id} unlocked.");
    }

    public static void ClearSteamAchStatus(Achievement.TYPE type)
    {
        if (!steamInitialized) return;

        string id = type.ToString();
        var ach = new Steamworks.Data.Achievement(id);
        ach.Clear();
        Debug.Log($"Achievement {id} cleared.");
    }
}
