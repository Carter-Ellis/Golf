using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    
    
    Ball ball;
    public List<Ability> unlockedAbilities = new List<Ability>();
    public int indexOfAbility = 0;

    public int coins;
    public int maxAbilities = 3;
    public int abilityCount = 0;

    [Header("TextDisplay")]
    [SerializeField] public TextMeshProUGUI selectedAbilityTxt;
    [SerializeField] public TextMeshProUGUI abilityChargesTxt;
    [SerializeField] private TextMeshProUGUI coinTxt;

    private void Start()
    {
        ball = GetComponent<Ball>();
    }

    private void Update()
    {
        AbilityManager();
        DisplayAbility();
        coinTxt.text = "Coins: " + coins;
    }
    private void AbilityManager()
    {
        if (Input.GetKeyDown(KeyCode.E) && unlockedAbilities.Count > 0)
        {
            unlockedAbilities[indexOfAbility].reset(ball);
            indexOfAbility = (indexOfAbility + 1) % unlockedAbilities.Count;
        }
        if (Input.GetKeyDown(KeyCode.Q) && unlockedAbilities.Count > 0)
        {
            unlockedAbilities[indexOfAbility].reset(ball);
            indexOfAbility = (unlockedAbilities.Count - 1 + indexOfAbility) % unlockedAbilities.Count;
        }

        if (Input.GetKeyDown(KeyCode.Space) && unlockedAbilities.Count > 0)
        {
            unlockedAbilities[indexOfAbility].onUse(ball);
        }
        if (unlockedAbilities.Count > 0)
        {
            unlockedAbilities[indexOfAbility].onFrame(ball);
        }
    }

    private void DisplayAbility()
    {
        if (unlockedAbilities.Count > 0)
        {
            selectedAbilityTxt.text = "Ability: " + unlockedAbilities[indexOfAbility].name;
            selectedAbilityTxt.color = unlockedAbilities[indexOfAbility].color;
            abilityChargesTxt.text = unlockedAbilities[indexOfAbility].chargeName + ": " + unlockedAbilities[indexOfAbility].getCharges(ball) + " of " + unlockedAbilities[indexOfAbility].getMaxCharges(ball);
            abilityChargesTxt.color = unlockedAbilities[indexOfAbility].color;
        }
    }

    public void AddAbility(Ability ability)
    {
        if (ability == null)
        {
            return;
        }

        foreach (Ability abil in unlockedAbilities)
        {
            if (abil.type == ability.type)
            {
                return;
            }
        }
        unlockedAbilities.Add(ability);
        indexOfAbility = unlockedAbilities.Count - 1;
        abilityCount++;
        ability.onPickup(ball);

    }

    public void RechargeAbility(ABILITIES type)
    {
        foreach (Ability ability in unlockedAbilities)
        {
            if (ability.type == type)
            {
                ability.onRecharge(ball);
                break;
            }
        }
    }
}
