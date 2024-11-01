using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    Inventory inv;
    [SerializeField] float sensitivity = 200f;
    [SerializeField] float maxViewDistance = 10.6f;
    [SerializeField] float minViewDistance = 2.9f;
    private float velocity = 0f;
    private float smoothTime = 0.15f;

    private void Start()
    {
        inv = FindObjectOfType<Ball>().GetComponent<Inventory>();
        if (inv != null && virtualCamera != null)
        {
            print("new zoom: " + inv.zoom);
            virtualCamera.m_Lens.OrthographicSize = inv.zoom;
        }
        else if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            Debug.LogError("Missing Virtual Camera or Inventory object in CameraZoom script.");
        }
        
        
    }

    private void LateUpdate()
    {


        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            
            inv.zoom -= scroll * sensitivity;
            inv.zoom = Mathf.Clamp(inv.zoom, minViewDistance, maxViewDistance);
            print("saving: " + inv.zoom);
            SaveSystem.SaveZoom(inv.zoom);
        }
        if (virtualCamera == null)
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                Debug.LogError("Virtual Camera missing in CameraZoom script.");
            } 
            
            return;
        }
        virtualCamera.m_Lens.OrthographicSize = Mathf.SmoothDamp(virtualCamera.m_Lens.OrthographicSize, inv.zoom, ref velocity, smoothTime);

    }
}
