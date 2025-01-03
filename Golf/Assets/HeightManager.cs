using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HeightManager : MonoBehaviour
{
    private Inventory inv;
    private FallOffLevel[] fallOffLevels;


    void Start()
    {
        inv = FindObjectOfType<Ball>().GetComponent<Inventory>();
        fallOffLevels = FindObjectsOfType<FallOffLevel>();
    }

    void Update()
    {
        GameObject tilemap = GameObject.Find("Height 1 Protector");
        GameObject tilemap2 = GameObject.Find("Height 2 Protector");
        if (tilemap == null)
        {
            Debug.LogError("Tilemap not found.");
            return;
        }

        foreach (FallOffLevel fallOffLevel in fallOffLevels)
        {
            if (inv.currentHeight != fallOffLevel.level)
            {
                fallOffLevel.gameObject.SetActive(false);
            }
            else
            {
                fallOffLevel.gameObject.SetActive(true);
            }

        }

        if (inv.currentHeight == 1)
        {
            
            tilemap.GetComponent<Collider2D>().enabled = false;
            tilemap2.GetComponent<Collider2D>().enabled = true;
        }
        else if(inv.currentHeight == 2)
        {
            tilemap2.GetComponent<Collider2D>().enabled = false;
            tilemap.GetComponent<Collider2D>().enabled = true;
        }
        else
        {
            tilemap.GetComponent<Collider2D>().enabled = true;
            tilemap2.GetComponent<Collider2D>().enabled = true;
        }
        
        
    }
}
