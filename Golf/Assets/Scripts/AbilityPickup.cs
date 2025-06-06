using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
[System.Serializable]
public class AbilityPickup : MonoBehaviour
{
    public ABILITIES type;
    public bool isRecharge = false;
    public bool playPickupAudio = false;
    public bool isRefill = false;
    float timer = 0f;
    float refillTime = 3f;
    bool used = false;

    private void Update()
    {
        if (used)
        {
            timer += Time.deltaTime;
            if (timer >= refillTime)
            {
                used = false;
                GetComponent<SpriteRenderer>().enabled = true;
                timer = 0f;
            }
        }
    }
        

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (used)
        {
            return;
        }

        Inventory inv = collision.gameObject.GetComponent<Inventory>();
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (inv == null || ball == null || inv.abilityCount >= inv.maxAbilities)
        {
            return;
        }

        Ability ability = inv.getCurrentAbility();
        if (ability != null)
        {
            ability.onBallDisabled(ball);
        }

        if (playPickupAudio)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.freeze, transform.position);
        }
        
        if (inv.unlockedAbilities != null && inv.unlockedAbilities.Count > 0)
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
        
        if (isRefill)
        {
            used = true;
            GetComponent<SpriteRenderer>().enabled = false;
            timer = 0f;

        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }

}
