using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public static class SaveSystem
{
    public static void SavePlayer (Inventory inv)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/player.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(inv);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveZoom(float zoomLevel)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/zoom.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, zoomLevel);
        stream.Close();
    }

    public static float LoadZoom()
    {
        string path = Application.persistentDataPath + "/zoom.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            float zoomLevel = (float)formatter.Deserialize(stream);
            stream.Close();
            return zoomLevel;
        }
        else
        {
            Debug.Log("Zoom save file not found.");
            return 5f; // Default zoom level
        }
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            stream.Close();
            return data;
        } 
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
    public static void ErasePlayerData()
    {
        string path = Application.persistentDataPath + "/player.data";
        if (File.Exists(path))
        {
            File.Delete(path);
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
            Debug.Log("Saved data cleared.");

        }
        else
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
            Debug.LogWarning("No saved data found to delete.");
        }

        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetVolumes(1f, 1f, 1f, 1f);
            Debug.Log("Audio settings reset to default.");
        }

    }

    public static void EraseZoomData()
    {
        string path = Application.persistentDataPath + "/zoom.data";
        if (File.Exists(path))
        {
            File.Delete(path);
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
            Debug.Log("Saved data cleared.");

        }
        else
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
            Debug.LogWarning("No saved data found to delete.");
        }

        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetVolumes(1f, 1f, 1f, 1f);
            Debug.Log("Audio settings reset to default.");
        }

    }

}
