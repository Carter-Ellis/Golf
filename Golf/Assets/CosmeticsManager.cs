using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CosmeticsManager : MonoBehaviour
{

    [SerializeField] private Image hatImage;
    [SerializeField] private Image _lock;
    [SerializeField] private Image hatMirror;
    [SerializeField] private Color[] colors;
    [SerializeField] private Image colorCircle;
    [SerializeField] private GameObject equipButton;
    [SerializeField] private GameObject unequipButton;
    public bool isEquipped;
    private Hat.TYPE type;
    private Inventory inv;
    private int colorIndex;
    private Vector2[] mirrorPos = new Vector2[]
    {
        new Vector2(-15,-11),
        new Vector2(0,4),
        new Vector2(0,9),
        new Vector2(0,-1),
        new Vector2(0,9),
        new Vector2(0,19),
        new Vector2(0,-6),
        new Vector2(0,30),
        new Vector2(0,14),
        new Vector2(0,9),
        new Vector2(15,-6),
        new Vector2(0,24),
        new Vector2(15,-1),
        new Vector2(0,9),
        new Vector2(0,9),
        new Vector2(0,4),
        new Vector2(10,4),
    };

    private IEnumerator Start()
    {
        yield return null;
        type = Hat.TYPE.NONE + 1;
        inv = FindObjectOfType<Inventory>();

        isEquipped = false;

        for (int i = 1; i < (int)Hat.TYPE.MAX_HATS; i++)
        {
            if (inv.hat == Hat.GetSprite((Hat.TYPE)i))
            {
                type = (Hat.TYPE)i;
                isEquipped = true;
                break;
            }
        }
        for (int i = 1; i < colors.Length; i++)
        {
            if (inv.hatColor == colors[i])
            {
                colorIndex = i;
                break;
            }
        }

        changeHat(0);
        changeColor(0);
        if (isEquipped)
        {
            Equip();
        }

    }

    private void changeHat(int amount)
    {

        type += amount;
        if ((int)type >= (int)Hat.TYPE.MAX_HATS)
        {
            type = Hat.TYPE.NONE + 1;
        }
        if ((int)type <= (int)Hat.TYPE.NONE)
        {
            type = Hat.TYPE.MAX_HATS - 1;
        }

        hatImage.sprite = Hat.GetSprite(type);
        Vector2 spriteSize = hatImage.sprite.rect.size;
        hatImage.rectTransform.sizeDelta = spriteSize * 10;

        adjustHatPos(type, hatImage.rectTransform);

        //Check if locked
        if (inv.unlockedHats.ContainsKey(type) && inv.unlockedHats[type])
        {
            _lock.enabled = false;
        }
        else
        {
            _lock.enabled = false;
        }

        if (amount == 0) { return; }
        

        if (hatImage.sprite == inv.hat && colors[colorIndex] == inv.hatColor)
        {
            equipButton.SetActive(false);
            unequipButton.SetActive(true);
            isEquipped = true;
        }
        else
        {
            equipButton.SetActive(true);
            unequipButton.SetActive(false);
            isEquipped = false;
        }

        //Check if locked
        if (!_lock.enabled)
        {
            equipButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        else
        {
            equipButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }

    }

    private void changeColor(int amount)
    {
        if (colors.Length <= 0 ) { Debug.Log("No colors found."); return; }

        colorIndex += amount;
        if (colorIndex >= colors.Length)
        {
            colorIndex = 0;
        }
        if (colorIndex < 0)
        {
            colorIndex = colors.Length - 1;
        }

        hatImage.color = colors[colorIndex];
        colorCircle.color = colors[colorIndex];

        if (amount == 0) { return; }

        if (colors[colorIndex] != inv.hatColor)
        {
            equipButton.SetActive(true);
            unequipButton.SetActive(false);
            isEquipped = false;
        }
        else if (hatImage.sprite == inv.hat && colors[colorIndex] == inv.hatColor)
        {
            equipButton.SetActive(false);
            unequipButton.SetActive(true);
            isEquipped = true;
        }

        
    }

    private void adjustHatPos(Hat.TYPE type, RectTransform rect)
    {
        if (type == Hat.TYPE.NINJA)
        {
            rect.anchoredPosition = new Vector2(584, 10.89994f);
        }
        else if (type == Hat.TYPE.FEZ)
        {
            rect.anchoredPosition = new Vector2(600, 10.89994f);
        }
        else
        {
            rect.anchoredPosition = new Vector2(589.7999f, 10.89994f);
        }
    }

    public void GoColorRight()
    {
        changeColor(1);
    }
    public void GoColorLeft()
    {
        changeColor(-1);
    }

    public void GoRight()
    {
        changeHat(1);
    }

    public void GoLeft()
    {
        changeHat(-1);
    }

    public void Equip()
    {
        inv.hat = hatImage.sprite;
        inv.hatColor = colors[colorIndex];
        inv.hatType = type;
        if (hatImage.sprite != null)
        {
            inv.hatName = hatImage.sprite.name;
        }

        hatMirror.sprite = hatImage.sprite;

        Vector2 spriteSize2 = hatMirror.sprite.rect.size;
        hatMirror.rectTransform.sizeDelta = spriteSize2 * 10;
        hatMirror.rectTransform.anchoredPosition = mirrorPos[(int)type - 1];

        hatMirror.color = colors[colorIndex];
        if (FindObjectOfType<BuyButton>() == null) {
            equipButton.SetActive(false);
            unequipButton.SetActive(true);
        }
        

        inv.SavePlayer();   
    }

    public void Unequip()
    {
        inv.hat = null;
        inv.hatColor = Color.white;
        inv.hatName = "";
        inv.hatType = Hat.TYPE.NONE;
        equipButton.SetActive(true);
        unequipButton.SetActive(false);

        hatMirror.sprite = null;
        hatMirror.color = Color.clear;



        inv.SavePlayer();
    }

}
