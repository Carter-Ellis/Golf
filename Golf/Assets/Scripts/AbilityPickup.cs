using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    public ABILITIES type;
    public bool isRecharge = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball == null)
        {
            return;
        }
        if (ball.unlockedAbilities.Count > 0)
        {
            ball.unlockedAbilities[ball.indexOfAbility].reset(ball);
        }
        if (isRecharge)
        {
            ball.RechargeAbility(type);
        }
        else
        {
            ball.AddAbility(Ability.Create(type, gameObject.GetComponent<SpriteRenderer>().color));
        }
        
        Destroy(this.gameObject);
    }

}
