using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class AbilityBurst : Ability
{
    private int charges = 0;
    private int maxCharges = 3;
    public bool isBurst = false;

    public AbilityBurst(Color color)
    {
        type = ABILITIES.BURST;
        name = "Burst";
        chargeName = "Shotgun Shells";
        description = "Unleash a rapid volley of high-velocity projectiles to swiftly attack and overwhelm enemies";
        this.color = color;
    }

    public override int getCharges(Ball ball)
    {
        return charges;
    }

    public override int getMaxCharges(Ball ball)
    {
        return maxCharges;
    }

    public override void onFrame(Ball ball)
    {

    }

    public override void onPickup(Ball ball)
    {
        charges = maxCharges;
    }

    public override void onRecharge(Ball ball)
    {
        if (charges < maxCharges)
        {
            charges++;
        }
    }

    public override void onUse(Ball ball)
    {
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        Debug.Log(charges + " MAg: " + rb.velocity.magnitude + "isBusrt: " + isBurst);
        if (charges <= 0 || rb.velocity.magnitude <= 0f)
        {
            
            return;
        }
        Debug.Log(isBurst);
        if (isBurst)
        {
            return;
        }
        ball.Burst();
        charges--;
        
    }

    public override void reset(Ball ball)
    {

    }
}
