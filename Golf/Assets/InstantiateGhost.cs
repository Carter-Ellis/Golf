using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateGhost : MonoBehaviour
{
    public GameObject ghost;
    void Awake()
    {
        if (ghost != null)
        {
            Instantiate(ghost);
        }
    }

}
