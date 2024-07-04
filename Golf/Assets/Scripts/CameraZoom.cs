using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    private float zoom;
    [SerializeField] float sensitivity = 200f;
    [SerializeField] float maxViewDistance = 10.6f;
    [SerializeField] float minViewDistance = 2.9f;
    private float velocity = 0f;
    private float smoothTime = 0.15f;

    private void Start()
    {
        if (virtualCamera != null)
        {
            zoom = virtualCamera.m_Lens.OrthographicSize;
        } 
    }

    private void Update()
    {


        float scroll = Input.GetAxis("Mouse ScrollWheel");

        zoom -= scroll * sensitivity;
        zoom = Mathf.Clamp(zoom, minViewDistance, maxViewDistance);
        if (virtualCamera == null)
        {
            return;
        }
        virtualCamera.m_Lens.OrthographicSize = Mathf.SmoothDamp(virtualCamera.m_Lens.OrthographicSize, zoom, ref velocity, smoothTime);

    }
}
