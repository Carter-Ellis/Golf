using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ABILITIES
{
    
    FREEZE,
    WIND,
    TELEPORT,
    BURST,
    MAX_ABILITIES
}
public abstract class Ability
{
    public ABILITIES type;
    public string name;
    public string chargeName;
    public Color color;
    public static Ability Create(ABILITIES type, Color color)
    {
        switch (type)
        {
            case ABILITIES.FREEZE:
                return new AbilityFreeze(color);
            case ABILITIES.WIND:
                return new AbilityWind(color);
            case ABILITIES.TELEPORT:
                return new AbilityTeleport(color);
            case ABILITIES.BURST:
                return new AbilityBurst(color);
            default:
                return null;
        }
    }

    public abstract void onUse(Ball ball);

    public abstract void onRecharge(Ball ball);

    public abstract int getCharges(Ball ball);

    public abstract int getMaxCharges(Ball ball);

    public abstract void onFrame(Ball ball);

    public abstract void reset(Ball ball);

    public abstract void onPickup(Ball ball);
}
