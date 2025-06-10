using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CosmeticsManager : MonoBehaviour
{

    [SerializeField] private Image hatImage;
    [SerializeField] private Image ballImage;
    [SerializeField] private Image _lock;
    [SerializeField] private Image ballLock;
    [SerializeField] private Image hatMirror;
    [SerializeField] private Color[] colors;
    [SerializeField] private Image colorCircle;
    [SerializeField] private Image colorBallCircle;
    [SerializeField] private GameObject equipButton;
    [SerializeField] private GameObject unequipButton;

    public bool isEquipped;
    private Hat.TYPE type;
    private Inventory inv;
    private int colorIndex;
    private int colorBallIndex;

    [SerializeField] private GameObject hatDescription;
    [SerializeField] private TextMeshProUGUI holeNumTxt;
    [SerializeField] private TextMeshProUGUI hatNameTxt;

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

    private void Start()
    {
        type = Hat.TYPE.NONE;
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
        for (int i = 1; i < colors.Length; i++)
        {
            if (inv.ballColor == colors[i])
            {
                colorBallIndex = i;
                break;
            }
        }

        changeHat(0);
        changeColor(0);
        changeBallColor(0);
        if (isEquipped)
        {
            Equip();
        }

    }

    public void changeHat(int amount)
    {

        type += amount;
        if ((int)type >= (int)Hat.TYPE.MAX_HATS)
        {
            type = Hat.TYPE.NONE;
        }
        if ((int)type < (int)Hat.TYPE.NONE)
        {
            type = Hat.TYPE.MAX_HATS - 1;
        }

        if (type == Hat.TYPE.NONE)
        {
            hatDescription.SetActive(false);
        }
        else
        {
            hatDescription.SetActive(true);
            holeNumTxt.text = "Hole " + ((int)type).ToString();
            hatNameTxt.text = ToTitleCase(type.ToString().ToLower());
        }

        hatImage.sprite = Hat.GetSprite(type);
        if (hatImage.sprite != null)
        {
            hatImage.color = colors[colorIndex];
            Vector2 spriteSize = hatImage.sprite.rect.size;
            hatImage.rectTransform.sizeDelta = spriteSize * 10;

            adjustHatPos(type, hatImage.rectTransform);
        }
        else
        {
            hatImage.color = Color.clear;
        }

        //Check if locked
        if (type == Hat.TYPE.NONE || (inv.unlockedHats.ContainsKey(type) && inv.unlockedHats[type]))
        {
            _lock.enabled = false;
        }
        else
        {
            _lock.enabled = true;
        }

        if (amount == 0) { return; }
        

        if (hatImage.sprite == inv.hat && colors[colorIndex] == inv.hatColor && colors[colorBallIndex] == inv.ballColor)
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
        if (!ballLock.enabled && !_lock.enabled)
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

        if (hatImage.sprite != null)
        {
            hatImage.color = colors[colorIndex];
        }
        colorCircle.color = colors[colorIndex];

        if (amount == 0) { return; }

        if (colors[colorIndex] != inv.hatColor)
        {
            equipButton.SetActive(true);
            unequipButton.SetActive(false);
            isEquipped = false;
        }
        else if (hatImage.sprite == inv.hat && colors[colorIndex] == inv.hatColor && colors[colorBallIndex] == inv.ballColor)
        {
            equipButton.SetActive(false);
            unequipButton.SetActive(true);
            isEquipped = true;
        }
        
    }

    public void changeBallColor(int amount)
    {
        if (colors.Length <= 0) { Debug.Log("No colors found."); return; }

        colorBallIndex += amount;
        if (colorBallIndex >= colors.Length)
        {
            colorBallIndex = 0;
        }
        if (colorBallIndex < 0)
        {
            colorBallIndex = colors.Length - 1;
        }

        colorBallCircle.color = colors[colorBallIndex];

        if (inv.isColorUnlocked || colors[colorBallIndex] == Color.white)
        {
            ballLock.enabled = false;
        }
        else
        {
            ballLock.enabled = true;
        }

        //if (amount == 0) { return; }

        if (colors[colorBallIndex] != inv.ballColor)
        {
            equipButton.SetActive(true);
            unequipButton.SetActive(false);
            isEquipped = false;
        }
        else if (hatImage.sprite == inv.hat && colors[colorIndex] == inv.hatColor && colors[colorBallIndex] == inv.ballColor)
        {
            equipButton.SetActive(false);
            unequipButton.SetActive(true);
            isEquipped = true;
        }

        if (!ballLock.enabled && !_lock.enabled)
        {
            equipButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        else
        {
            equipButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }

    }

    private void adjustHatPos(Hat.TYPE type, RectTransform rect)
    {
        if (type == Hat.TYPE.NINJA)
        {
            rect.anchoredPosition = new Vector2(584, rect.anchoredPosition.y);
        }
        else if (type == Hat.TYPE.FEZ)
        {
            rect.anchoredPosition = new Vector2(600, rect.anchoredPosition.y);
        }
        else
        {
            rect.anchoredPosition = new Vector2(589.7999f, rect.anchoredPosition.y);
        }
    }

    public void BallColorRight()
    {
        changeBallColor(1);
    }
    public void BallColorLeft()
    {
        changeBallColor(-1);
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

        inv.ballColor = colors[colorBallIndex];
        ballImage.color = colors[colorBallIndex];
        inv.hatType = type;

        hatMirror.sprite = hatImage.sprite;

        if (hatImage.sprite != null)
        {
            inv.hatName = hatImage.sprite.name;
            Vector2 spriteSize2 = hatMirror.sprite.rect.size;
            hatMirror.rectTransform.sizeDelta = spriteSize2 * 10;
            hatMirror.rectTransform.anchoredPosition = mirrorPos[(int)type - 1];
            hatMirror.color = colors[colorIndex];
        }
        else
        {
            hatMirror.color = Color.clear;
        }

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
        inv.ballColor = Color.white;
        inv.hatName = "";
        inv.hatType = Hat.TYPE.NONE;
        equipButton.SetActive(true);
        unequipButton.SetActive(false);

        hatMirror.sprite = null;
        hatMirror.color = Color.clear;
        ballImage.color = Color.white;


        inv.SavePlayer();
    }

    string ToTitleCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return char.ToUpper(input[0]) + input.Substring(1).ToLower();
    }

}
