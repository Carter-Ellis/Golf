using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AbilityFreeze : Ability
{
    private int charges = 0;

    public AbilityFreeze(Color color)
    {
        type = ABILITIES.FREEZE;
        name = "Freeze";
        chargeName = "Frost Locks";
        description = "Instantly stop the ball's movement.";
        upgradeDescription = "Adds extra Freeze use.";
        this.color = color;
    }

    public override int getCharges(Ball ball)
    {
        return charges;
    }

    public override int getMaxCharges(Ball ball)
    {
        return Ability.maxChargesByType[ABILITIES.FREEZE];
    }

    public override void onPickup(Ball ball)
    {
        charges = getMaxCharges(ball);
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
        if (ball.closeToSpike && !ball.GetComponent<Inventory>().achievements[(int)Achievement.TYPE.CLOSE_CALL])
        {
            Achievement.Give(Achievement.TYPE.CLOSE_CALL);
            ball.GetComponent<Inventory>().SavePlayer();
        }
        ball.DisplayFreezeParticles();
        Audio.playSFX(FMODEvents.instance.freeze, GameObject.FindObjectOfType<Ball>().transform.position);
        rb.velocity = Vector2.zero;
        charges -= 1;
    }

    public override void onFrame(Ball ball) { }

    public override void reset(Ball ball) { }

    public override void onBallDisabled(Ball ball)
    {
        
    }
}
