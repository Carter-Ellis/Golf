using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public static class SaveSystem
{
    public static void SavePlayer(Inventory inv)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.data";

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            PlayerData data = new PlayerData(inv);
            formatter.Serialize(stream, data);
            stream.Close();
        }
    }

    public static void SaveZoom(float zoomLevel)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/zoom.data";

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, zoomLevel);
        }
    }

    public static float LoadZoom()
    {
        string path = Application.persistentDataPath + "/zoom.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                return (float)formatter.Deserialize(stream);
            }
        }
        else
        {
            //Debug.Log("Zoom save file not found.");
            return 5f; // Default zoom level
        }
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                PlayerData result = formatter.Deserialize(stream) as PlayerData;
                stream.Close();
                return result;
            }
        }
        else
        {
            //Debug.Log("Save file not found in " + path);
            return null;
        }
    }

    public static void ErasePlayerData()
    {
        string path = Application.persistentDataPath + "/player.data";
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                //Debug.Log("Saved data cleared.");
            }
            else
            {
                //Debug.LogWarning("No saved data found to delete.");
            }
        }
        catch (IOException e)
        {
            Debug.LogError("Could not delete player data: " + e.Message);
        }

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetVolumes(1f, 1f, 1f, 1f);
            //Debug.Log("Audio settings reset to default.");
        }
    }

    public static void EraseZoomData()
    {
        string path = Application.persistentDataPath + "/zoom.data";
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                //Debug.Log("Zoom data cleared.");
            }
            else
            {
                //Debug.LogWarning("No zoom data found to delete.");
            }
        }
        catch (IOException e)
        {
            Debug.LogError("Could not delete zoom data: " + e.Message);
        }

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetVolumes(1f, 1f, 1f, 1f);
        }
    }
}
