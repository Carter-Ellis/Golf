using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Crab")]
    [field: SerializeField] public EventReference crabWalk { get; private set; }
    [field: Header("Seagull")]
    [field: SerializeField] public EventReference squak { get; private set; }
    [field: SerializeField] public EventReference flapWing { get; private set; }
    [field: Header("Ball SFX")]
    [field: SerializeField] public EventReference ballRollSFX { get; private set; }
    [field: SerializeField] public EventReference ballHurtSFX { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }
    [field: SerializeField] public EventReference mainMusic { get; private set; }
    [field: SerializeField] public EventReference beachMusic { get; private set; }

    [field: Header("Ambience")]
    [field: SerializeField] public EventReference ambience { get; private set; }
    [field: SerializeField] public EventReference beachAmbience { get; private set; }

    [field: Header("DoorSFX")]
    [field: SerializeField] public EventReference door6sec { get; private set; }

    [field: Header("SpikeTrap SFX")]
    [field: SerializeField] public EventReference spikeTrapAttack { get; private set; }
    [field: SerializeField] public EventReference spikeTrapSet { get; private set; }
    [field: SerializeField] public EventReference spikeTrapContract { get; private set; }

    [field: Header("InHole SFX")]
    [field: SerializeField] public EventReference inHoleSound { get; private set; }
    [field: SerializeField] public EventReference inHoleBad { get; private set; }
    [field: SerializeField] public EventReference overHole { get; private set; }

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
    [field: SerializeField] public EventReference error { get; private set; }

    [field: SerializeField] public EventReference tick { get; private set; }

    [field: Header("Coin")]
    [field: SerializeField] public EventReference coinCollect { get; private set; }

    [field: Header("Shop")]
    [field: SerializeField] public EventReference shopPurchase { get; private set; }
    [field: SerializeField] public EventReference shopMusic { get; private set; }

    [field: Header("AppleBite")]
    [field: SerializeField] public EventReference appleBite { get; private set; }

    [field: Header("ColinLines")]
    [field: SerializeField] public EventReference birdie { get; private set; }

    [field: SerializeField] public EventReference bogey { get; private set; }

    [field: SerializeField] public EventReference doubleBogey { get; private set; }

    [field: SerializeField] public EventReference parthetic { get; private set; }

    [field: SerializeField] public EventReference eagle { get; private set; }
    [field: SerializeField] public EventReference seagle { get; private set; }

    [field: SerializeField] public EventReference par { get; private set; }
    [field: SerializeField] public EventReference excellent { get; private set; }
    [field: SerializeField] public EventReference parfect { get; private set; }

    [field: Header("Applause")]
    [field: SerializeField] public EventReference applause { get; private set; }

    [field: Header("Bouncer")]
    [field: SerializeField] public EventReference bouncer { get; private set; }

    [field: Header("Map SFX")]
    [field: SerializeField] public EventReference mapOpen { get; private set; }
    
    [field: Header("Geese")]
    [field: SerializeField] public EventReference geese { get; private set; }

    [field: Header("Wood Hit")]
    [field: SerializeField] public EventReference woodHit { get; private set; }

    [field: Header("Windmill")]
    [field: SerializeField] public EventReference windmill { get; private set; }

    [field: Header("Freeze")]
    [field: SerializeField] public EventReference freeze { get; private set; }

    [field: Header("Wall Break")]
    [field: SerializeField] public EventReference wallBreak { get; private set; }

    [field: Header("CobbleHit")]
    [field: SerializeField] public EventReference cobbleHit { get; private set; }

    [field: Header("Mole")]
    [field: SerializeField] public EventReference moleCrawl { get; private set; }
    [field: SerializeField] public EventReference moleHit { get; private set; }

    [field: Header("Wind Ability")]
    [field: SerializeField] public EventReference windAbility { get; private set; }

    [field: Header("Teleport")]
    [field: SerializeField] public EventReference teleport { get; private set; }

    [field: Header("Gate")]
    [field: SerializeField] public EventReference gateOpen { get; private set; }

    [field: Header("Golf Cart")]
    [field: SerializeField] public EventReference golfCart { get; private set; }

    [field: Header("Fan")]
    [field: SerializeField] public EventReference fan { get; private set; }

    [field: Header("Achievement")]
    [field: SerializeField] public EventReference achievement { get; private set; }

    [field: Header("Water")]
    [field: SerializeField] public EventReference watersplash { get; private set; }
    [field: Header("Secret Coin")]
    [field: SerializeField] public EventReference secretCoin { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            //Debug.Log("Found more than one FMOD Events scrupt in the scene.");
        }
        instance = this;
    }
}
