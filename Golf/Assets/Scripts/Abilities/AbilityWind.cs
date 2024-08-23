using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityWind : Ability
{
    private int charges = 0;
    private int maxCharges = 1;
    private float gustSpeed = 7f;
    public AbilityWind(Color color)
    {
        type = ABILITIES.WIND;
        name = "Wind";
        chargeName = "Air Bags";
        description = "Harness a powerful gust to significantly boost the ball's speed, propelling it forward with increased velocity";
        this.color = color;
    }
    public override void onPickup(Ball ball)
    {
        charges = maxCharges;
    }

    public override int getCharges(Ball ball)
    {
        return charges;
    }

    public override int getMaxCharges(Ball ball)
    {
        return maxCharges;
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
        if (charges <= 0 || rb.velocity.magnitude <= 0f)
        {
            return;
        }
        charges--;
        
        rb.AddForce(rb.velocity.normalized * gustSpeed, ForceMode2D.Impulse);

    }

    public override void onFrame(Ball ball)
    {

    }

    public override void reset(Ball ball)
    {
        
    }
}
