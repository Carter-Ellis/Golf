using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    public ABILITIES type;
    public bool isRecharge = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        Inventory inv = collision.gameObject.GetComponent<Inventory>();
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (inv == null || ball == null || inv.abilityCount >= inv.maxAbilities)
        {
            return;
        }
        if (inv.unlockedAbilities.Count > 0)
        {
            inv.unlockedAbilities[inv.indexOfAbility].reset(ball);
        }
        if (isRecharge)
        {
            inv.RechargeAbility(type);
        }
        else
        {
            inv.AddAbility(Ability.Create(type, gameObject.GetComponent<SpriteRenderer>().color));
        }
        
        Destroy(this.gameObject);
    }

}
