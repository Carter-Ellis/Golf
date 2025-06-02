using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class Button : MonoBehaviour
{
    private Inventory inv;

    private SpriteRenderer sr;
    public GameObject target;
    private ButtonTarget buttonTarget;
    public Sprite pushedSprite;
    public Sprite unpushedSprite;
    public AudioClip pushClip;
    public AudioClip unpushClip;

    public bool isRedCoinButton;
    public bool isOnePress;
    public GameObject[] redCoins;
    public GameObject specialCoin;
    private float redCoinTimer = 0f;
    private float redCoinThreshold = 6.5f;
    private bool isRedCoinActive = false;
    private bool redCoinComplete = false;
    private bool isPressed = false;
    internal bool interactable;

    private EventInstance door6Instance;

    private void Start()
    {
        inv = FindObjectOfType<Inventory>();
    }

    private void Update()
    {
        if (isRedCoinActive && !redCoinComplete)
        {
            
            redCoinTimer += Time.deltaTime;
            if (redCoinTimer > redCoinThreshold)
            {
                DespawnRedCoins();
                redCoinTimer = 0f;
                isRedCoinActive = false;
            }
            if (inv == null) { return; }
            if (inv.redCoinCount == redCoins.Length)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.inHoleSound, transform.position);
                specialCoin.SetActive(true);
                redCoinComplete = true;
            }
        }
    }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (target != null)
        {
            Component[] components = target.GetComponents<Component>();
            foreach (Component component in components)
            {
                if (component is ButtonTarget)
                {
                    buttonTarget = (ButtonTarget)component;
                    break;
                }
            }
        }
    }

    void SpawnRedCoins()
    {
        if (redCoinComplete)
        {
            return;
        }

        door6Instance = RuntimeManager.CreateInstance(FMODEvents.instance.door6sec);
        door6Instance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        door6Instance.start();

        isRedCoinActive = true;
        foreach (GameObject redcoin in redCoins)
        {
            if (redcoin == null)
            {
                Debug.Log("redCoin is null");
                return;
            }
            redcoin.SetActive(true);
        }
    }

    void DespawnRedCoins()
    {
        foreach (GameObject redcoin in redCoins)
        {
            if (redcoin == null)
            {
                Debug.Log("redCoin is null");
                return;
            }
            redcoin.SetActive(false);
        }
        if (GameObject.FindObjectOfType<Inventory>() == null)
        {
            return;
        }
        inv.redCoinCount = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPressed)
        {
            return;
        }
        isPressed = true;
        sr.sprite = pushedSprite;
        AudioManager.instance.PlayOneShot(FMODEvents.instance.push, transform.position);
        if (isRedCoinButton && !isRedCoinActive)
        {
            SpawnRedCoins();
        }
        if (buttonTarget == null)
        {
            return;
        }
        buttonTarget.onPress();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isOnePress)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.unpush, transform.position);
            sr.sprite = unpushedSprite;
            isPressed = false;
        }
    }

    private void OnDisable()
    {
        if (door6Instance.isValid())
        {
            door6Instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            door6Instance.release();
        }
    }
}
