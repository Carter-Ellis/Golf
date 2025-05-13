using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OkButton : MonoBehaviour
{
    private Inventory inv;

    public void closePanel()
    {
        transform.parent.gameObject.SetActive(false);
        FindObjectOfType<Ball>().enabled = true;
    }
}
