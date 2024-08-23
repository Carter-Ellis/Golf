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
    public string description;
    public Color color;

    public Ability()
    {
        name = GetName(type);
        description = GetDescription(type);
    }

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

    public static string GetName(ABILITIES type)
    {
        switch(type)
        {
            case ABILITIES.FREEZE: return "Freeze";
            case ABILITIES.WIND: return "Wind";
            case ABILITIES.TELEPORT: return "Teleport";
            case ABILITIES.BURST: return "Burst";
            default: return "";
        }
    }
    public static string GetDescription(ABILITIES type)
    {
        switch (type)
        {
            case ABILITIES.FREEZE: return "Instantly halt the ball's movement, freezing it in place for precise control and strategic positioning";
            case ABILITIES.WIND: return "Harness a powerful gust to significantly boost the ball's speed, propelling it forward with increased velocity";
            case ABILITIES.TELEPORT: return "Instantly transport the ball from one location to another for quick repositioning";
            case ABILITIES.BURST: return "Unleash a rapid volley of high-velocity projectiles to swiftly attack and overwhelm enemies";
            default: return "";
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
