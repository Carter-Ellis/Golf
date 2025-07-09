using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[System.Serializable]
public class AbilityWind : Ability
{
    private int charges = 0;
    private float gustSpeed = 7f;

    public AbilityWind(Color color)
    {
        type = ABILITIES.WIND;
        name = "Wind";
        chargeName = "Air Bags";
        description = "Harness a powerful gust to significantly boost the ball's speed, propelling it forward with increased velocity";
        upgradeDescription = "Increases Wind count by 1";
        this.color = color;
    }

    public override void onPickup(Ball ball)
    {
        
        charges = getMaxCharges(ball);
    }

    public override int getCharges(Ball ball)
    {
        return charges;
    }

    public override int getMaxCharges(Ball ball)
    {
        return Ability.maxChargesByType[ABILITIES.WIND];
    }

    public override void onRecharge(Ball ball)
    {
        if (charges < getMaxCharges(ball))
        {
            charges++;
        }
    }

    public override void setCharges(int amount)
    {
        charges = amount;
    }

    public override void onUse(Ball ball)
    {
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        if (charges <= 0 || rb.velocity.magnitude <= 0f)
        {
            return;
        }

        Audio.playSFX(FMODEvents.instance.windAbility, GameObject.FindObjectOfType<Ball>().transform.position);
        charges--;

        rb.AddForce(rb.velocity.normalized * gustSpeed, ForceMode2D.Impulse);
        ball.DisplayWindParticles();
    }

    public override void onFrame(Ball ball) { }

    public override void reset(Ball ball) { }

    public override void onBallDisabled(Ball ball)
    {
    }
}
