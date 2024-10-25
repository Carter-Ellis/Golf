using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    Transform followedObject;
    Ball ball;
    public bool isViewMode;
    public bool isIdleMode;
    Vector3 mapPos = new Vector3(4, 7, -10);

    public float mapViewSize = 15f;
    public float normalViewSize = 6.5f;
    private float timer = 0;
    private float reloadSceneTime = 3f;
    KeyCode mapKey = KeyCode.Tab;
    void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        ball = GameObject.FindObjectOfType<Ball>();
        cam.m_Lens.OrthographicSize = normalViewSize;
        isViewMode = false;
    }

    void LateUpdate()
    {
        if (ball == null && !isIdleMode)
        {
            timer += Time.deltaTime;
            if (timer > reloadSceneTime)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        if (ball != null && Input.GetKeyDown(mapKey) && !isViewMode) 
        {
            cam.m_Lens.OrthographicSize = mapViewSize;
            followedObject = cam.Follow;
            cam.Follow = null;
            cam.transform.position = mapPos;
            isViewMode = true;
        }
        else if (ball != null && Input.GetKeyDown(mapKey))
        {
            cam.m_Lens.OrthographicSize = normalViewSize;
            cam.Follow = followedObject;
            isViewMode = false;
        }
        

    }
}
