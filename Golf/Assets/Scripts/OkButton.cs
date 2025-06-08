using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OkButton : MonoBehaviour
{
    private Inventory inv;

    private void Start()
    {
        inv = FindObjectOfType<Inventory>();
    }

    public void closePanel()
    {
        transform.parent.gameObject.SetActive(false);
        int level = SceneManager.GetActiveScene().buildIndex;
        inv.levelPopups[level] = true;
        FindObjectOfType<Ball>().enabled = true;
        inv.SavePlayer();
    }
}
