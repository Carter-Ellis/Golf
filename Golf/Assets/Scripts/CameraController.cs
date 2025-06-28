using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    public Canvas userInterface;
    public Canvas mapViewUI;
    Transform followedObject;
    Ball ball;
    public bool isViewMode;
    public bool isIdleMode;
    public bool isWinScreen;

    public Transform mapViewPos;
    public float mapViewSize;

    public float normalViewSize = 15f;
    private float timer = 0;
    private float reloadSceneTime = 1f;

    private bool isAdjustingDamp;
    private float dampTimer = 0f;
    private float dampTimerThreshold = .05f;

    void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        ball = GameObject.FindObjectOfType<Ball>();
        cam.m_Lens.OrthographicSize = normalViewSize;
        isViewMode = false;
    }
    private void Update()
    {
        if (isAdjustingDamp)
        {
            dampTimer += Time.deltaTime;
            if (dampTimer >= dampTimerThreshold)
            {
                var transposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
                if (transposer != null)
                {
                    transposer.m_XDamping = 1;
                    transposer.m_YDamping = 1;
                    transposer.m_ZDamping = 1;
                }
                isAdjustingDamp = false;
            }
        }
    }
    void LateUpdate()
    {
        if ((mapViewPos == null || cam == null) && SceneManager.GetActiveScene().name != "Main Menu")
        {
            Debug.LogError("GolfMapViewpoint or VirtualCamera is not assigned!");
            return;
        }
        else if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            return;
        }

        if (ball == null && !isIdleMode)
        {
            timer += Time.deltaTime;
            if (timer > reloadSceneTime)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (isWinScreen)
        {
            return;
        }

        if (ball != null && PlayerInput.isDown(PlayerInput.Axis.Fire4) && !isViewMode) 
        {
            
            AudioManager.instance.PlayOneShot(FMODEvents.instance.mapOpen, transform.position);

            ball.hasClickedBall = false;
            ball.cursor.GetComponent<SpriteRenderer>().enabled = false;
            ball.ClearDots();
            ball.swingPowerSlider.gameObject.SetActive(false);
            ball.powerTxt.gameObject.SetActive(false);
            ball.cancelImage.SetActive(false);

            userInterface.enabled = false;
            mapViewUI.enabled = true;
            if (ball.isSelectFan)
            {
                Selectable selected = ball.objectSelected;

                // Convert Selectable to MonoBehaviour to access gameObject
                followedObject = ((MonoBehaviour)selected).gameObject.transform;
            }
            else
            {
                followedObject = ball.transform;
            }         
            cam.Follow = null;
            cam.m_Lens.OrthographicSize = mapViewSize;
            cam.transform.position = mapViewPos.position;
            
            isViewMode = true;
        }
        else if (ball != null && PlayerInput.isDown(PlayerInput.Axis.Fire4))
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.menuClose, transform.position);
            var transposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (transposer != null)
            {
                transposer.m_XDamping = 0;
                transposer.m_YDamping = 0;
                transposer.m_ZDamping = 0;
            }
            cam.m_Lens.OrthographicSize = ball.GetComponent<Inventory>().zoom;
            cam.transform.position = new Vector2(followedObject.transform.position.x, followedObject.transform.position.y);
            cam.Follow = followedObject;

            userInterface.enabled = true;
            mapViewUI.enabled = false;
            isViewMode = false;
            isAdjustingDamp = true;

        }
        

    }
}
