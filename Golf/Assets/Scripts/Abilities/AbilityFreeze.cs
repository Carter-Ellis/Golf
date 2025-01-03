using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
[System.Serializable]

public class AbilityFreeze : Ability
{
    private int charges = 0;
    private int maxCharges = 10;
    public AbilityFreeze(Color color)
    {
        type = ABILITIES.FREEZE;
        name = "Freeze";
        chargeName = "Frost Locks";
        description = "Instantly halt the ball's movement, freezing it in place for precise control and strategic positioning";
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
        if (charges <= 0 || rb.velocity.magnitude <= 0f)
        {
            return;
        }
        rb.velocity = new Vector2(0, 0);
    }

    public override void reset(Ball ball)
    {

    }
}
