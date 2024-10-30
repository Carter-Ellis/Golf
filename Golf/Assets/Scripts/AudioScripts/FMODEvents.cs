using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Ball SFX")]
    [field: SerializeField] public EventReference ballRollSFX { get; private set; }
    [field: SerializeField] public EventReference ballHurtSFX { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }

    [field: Header("Ambience")]
    [field: SerializeField] public EventReference ambience { get; private set; }

    [field: Header("DoorSFX")]
    [field: SerializeField] public EventReference door6sec { get; private set; }

    [field: Header("SpikeTrap SFX")]
    [field: SerializeField] public EventReference spikeTrapAttack { get; private set; }
    [field: SerializeField] public EventReference spikeTrapSet { get; private set; }
    [field: SerializeField] public EventReference spikeTrapContract { get; private set; }

    [field: Header("InHole SFX")]
    [field: SerializeField] public EventReference inHoleSound { get; private set; }

    [field: Header("Tunnel Enter")]
    [field: SerializeField] public EventReference tunnelEnter { get; private set; }

    [field: Header("Tunnel Exit")]
    [field: SerializeField] public EventReference tunnelExit { get; private set; }

    [field: Header("Tunnel Bounce")]
    [field: SerializeField] public EventReference tunnelBounce { get; private set; }

    [field: Header("Wall Hits")]
    [field: SerializeField] public EventReference wallHit { get; private set; }
    
    [field: Header("Ball Swings")]
    [field: Header("Hard")]
    [field: SerializeField] public EventReference hardSwing { get; private set; }
    [field: Header("Medium")]
    [field: SerializeField] public EventReference mediumSwing { get; private set; }
    [field: Header("Soft")]
    [field: SerializeField] public EventReference softSwing { get; private set; }

    [field: Header("Button")]
    [field: SerializeField] public EventReference push { get; private set; }
    [field: SerializeField] public EventReference unpush { get; private set; }

    [field: Header("Menu")]
    [field: SerializeField] public EventReference menuBlip { get; private set; }
    [field: SerializeField] public EventReference menuOpen { get; private set; }
    [field: SerializeField] public EventReference menuClose { get; private set; }
    
    [field: Header("Coin")]
    [field: SerializeField] public EventReference coinCollect { get; private set; }

    [field: Header("Shop")]
    [field: SerializeField] public EventReference shopPurchase { get; private set; }

    [field: Header("AppleBite")]
    [field: SerializeField] public EventReference appleBite { get; private set; }
    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one FMOD Events scrupt in the scene.");
        }
        instance = this;
    }
}
