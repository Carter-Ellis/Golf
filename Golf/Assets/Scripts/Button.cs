using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private SpriteRenderer sr;
    public GameObject target;
    private ButtonTarget buttonTarget;
    public Sprite pushedSprite;
    public Sprite unpushedSprite;
    public AudioClip pushClip;
    public AudioClip unpushClip;

    public bool isRedCoinButton;
    public GameObject[] redCoins;
    public GameObject specialCoin;
    private float redCoinTimer = 0f;
    private float redCoinThreshold = 6.5f;
    private bool isRedCoinActive = false;
    private bool redCoinComplete = false;
    internal bool interactable;

    private void Update()
    {
        if (isRedCoinActive && !redCoinComplete)
        {
            if (GameObject.FindObjectOfType<Inventory>().redCoinCount == redCoins.Length)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.inHoleSound, transform.position);
                specialCoin.SetActive(true);
                redCoinComplete = true;
            }
            redCoinTimer += Time.deltaTime;
            if (redCoinTimer > redCoinThreshold)
            {
                DespawnRedCoins();
                redCoinTimer = 0f;
                isRedCoinActive = false;
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
        AudioManager.instance.PlayOneShot(FMODEvents.instance.door6sec, transform.position);
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
        GameObject.FindObjectOfType<Inventory>().redCoinCount = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
        AudioManager.instance.PlayOneShot(FMODEvents.instance.unpush, transform.position);
        sr.sprite = unpushedSprite;
    }
}
