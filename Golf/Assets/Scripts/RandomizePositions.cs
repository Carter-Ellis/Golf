using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizePositions : MonoBehaviour
{
    [SerializeField]
    private GameObject[] gameObjects;

    private void Start()
    {
        
        List<Vector2> positions = new List<Vector2>();

        foreach (GameObject obj in  gameObjects)
        {
            positions.Add(obj.transform.position);
        }

        foreach (GameObject obj in gameObjects)
        {
            int index = Random.Range(0, positions.Count);
            obj.transform.position = positions[index];
            positions.RemoveAt(index);
        }

    }


}
