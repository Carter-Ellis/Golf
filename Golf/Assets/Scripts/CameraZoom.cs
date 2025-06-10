using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    CameraController camController;
    Inventory inv;
    private float sensitivity = 3000f;
    private float maxViewDistance = 11.5f;
    [SerializeField] float minViewDistance = 2.9f;
    private float velocity = 0f;
    private float smoothTime = 0.15f;

    private void Start()
    {
        inv = FindObjectOfType<Ball>().GetComponent<Inventory>();
        camController = FindObjectOfType<CameraController>();
        if (inv != null && virtualCamera != null)
        {
            virtualCamera.m_Lens.OrthographicSize = inv.zoom;
        }
        
        
    }

    private void LateUpdate()
    {


        float scroll = PlayerInput.get(PlayerInput.Axis.ScrollWheel) * Time.deltaTime;

        if (scroll != 0)
        {
            
            inv.zoom -= scroll * sensitivity;
            inv.zoom = Mathf.Clamp(inv.zoom, minViewDistance, maxViewDistance);
            SaveSystem.SaveZoom(inv.zoom);
        }
        if (virtualCamera == null)
        {
            return;
        }
        if (camController.isViewMode || camController.isWinScreen)
        {
            return;
        }
        virtualCamera.m_Lens.OrthographicSize = Mathf.SmoothDamp(virtualCamera.m_Lens.OrthographicSize, inv.zoom, ref velocity, smoothTime);

    }
}
