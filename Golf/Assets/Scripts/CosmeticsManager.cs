using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static Hat;

public class CosmeticsManager : MonoBehaviour
{
    [SerializeField] private GameObject RTPos;
    [SerializeField] private RenderTexture hatPreviewRT;
    [SerializeField] private RawImage hatAnimImage;
    [SerializeField] private RawImage hatAnimMirrorImage;
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

    private GameObject animHat;

    public bool isEquipped;

    private bool isAnimSection;
    private Hat.TYPE type;
    private Hat.ANIM_TYPE animType;
    private Inventory inv;
    private int colorIndex;
    private int colorBallIndex;

    private Vector2 animImagePos;

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
        new Vector2(0,-6),
    };

    private Vector2[] animMirrorPos = new Vector2[]
    {
        new Vector2(-5,18),
        new Vector2(-5, -10),
        new Vector2(-5, -20),
    };

    private void Start()
    {
        type = Hat.TYPE.NONE;
        animType = ANIM_TYPE.MAX_ANIM_HATS;
        inv = FindObjectOfType<Inventory>();
        inv.unlockedAnimHats[Hat.ANIM_TYPE.FIRE] = true;
        animImagePos = hatAnimMirrorImage.rectTransform.anchoredPosition;
        if (inv.animHat == null)
        {
            hatAnimMirrorImage.color = new Color(1f, 1f, 1f, 0f);
        }
        
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

        for (int i = 1; i < (int)Hat.ANIM_TYPE.MAX_ANIM_HATS; i++)
        {
            if (inv.animHat == Hat.GetAnimHat((Hat.ANIM_TYPE)i))
            {
                animType = (Hat.ANIM_TYPE)i;
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
        changeAnimHat(0);
        changeColor(0);
        changeBallColor(0);
        if (isEquipped)
        {
            Equip();
        }

    }

    public void changeAnimHat(int amount)
    {
        if (animHat == null || animHat != inv.animHat)
        {
            hatAnimMirrorImage.color = new Color(1f, 1f, 1f, 0f);
        }

        if (!isAnimSection)
        {
            return;
        }

        hatAnimImage.color = new Color(1f, 1f, 1f, 1f);
        animType += amount;
        

        if ((int)animType >= (int)Hat.ANIM_TYPE.MAX_ANIM_HATS)
        {
            animType = Hat.ANIM_TYPE.MAX_ANIM_HATS;
            type = Hat.TYPE.NONE;
            isAnimSection = false;
        }
        if ((int)animType <= (int)Hat.ANIM_TYPE.NONE)
        {
            animType = Hat.ANIM_TYPE.NONE;
            type = Hat.TYPE.MAX_HATS - 1;
            isAnimSection = false;
        }
        
        if (animType == Hat.ANIM_TYPE.NONE || animType == Hat.ANIM_TYPE.MAX_ANIM_HATS)
        {
            hatDescription.SetActive(false);
            changeHat(0);
            return;
        }
        else
        {
            hatDescription.SetActive(true);
            holeNumTxt.text = "Hole " + ((int)animType).ToString();
            hatNameTxt.text = ToTitleCase(animType.ToString().ToLower());
        }
        if (animHat != null)
        {
            Destroy(animHat);
        }

        GameObject animHatAsset = Hat.GetAnimHat(animType);
        animHat = Instantiate(animHatAsset);
        animHat.transform.position = RTPos.transform.position;
        hatAnimImage.texture = hatPreviewRT;

        if (animHat == null || animHat != inv.animHat)
        {
            hatAnimMirrorImage.color = new Color(1f, 1f, 1f, 0f);
        }

        //Check if locked
        if (animType == Hat.ANIM_TYPE.NONE || (inv.unlockedAnimHats.ContainsKey(animType) && inv.unlockedAnimHats[animType]))
        {
            _lock.enabled = false;
        }
        else
        {
            _lock.enabled = true;
        }

        if (amount == 0) { return; }
        

        if (animHat == inv.animHat)
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

    public void changeHat(int amount)
    {

        if (isAnimSection)
        {
            return;
        }

        hatAnimImage.color = new Color(1f, 1f, 1f, 0f);

        type += amount;
        print("type: " + type);
        if ((int)type >= (int)Hat.TYPE.MAX_HATS)
        {
            type = Hat.TYPE.NONE;
            animType = ANIM_TYPE.NONE;
            isAnimSection = true;
        }
        if ((int)type < (int)Hat.TYPE.NONE)
        {
            type = Hat.TYPE.NONE;
            animType = ANIM_TYPE.MAX_ANIM_HATS;
            isAnimSection = true;
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
        changeAnimHat(1);
    }

    public void GoLeft()
    {
        changeHat(-1);
        changeAnimHat(-1);
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

        if (animType != Hat.ANIM_TYPE.NONE && animType != Hat.ANIM_TYPE.MAX_ANIM_HATS)
        {
            print("INSIDE EQUIP: " + animType);
            print("isAnimSection " + isAnimSection);
            hatAnimMirrorImage.texture = hatPreviewRT;
            hatAnimMirrorImage.rectTransform.anchoredPosition = animImagePos + animMirrorPos[(int)animType - 1];
        
            if (isAnimSection)
            {
                inv.animHat = animHat;
                hatAnimMirrorImage.color = new Color(1f, 1f, 1f, 1f);
                print("YO");
            }
            else
            {
                hatAnimMirrorImage.color = new Color(1f, 1f, 1f, 0f);
            }
        
        }
        else
        {
            hatAnimMirrorImage.color = new Color(1f, 1f, 1f, 0f);
        }


        if (FindObjectOfType<BuyButton>() == null) {
            equipButton.SetActive(false);
            unequipButton.SetActive(true);
        }

        if (colorBallIndex == 5 && (colorIndex == 3 || colorIndex == 4))
        {
            
            if (!inv.achievements[(int)Achievement.TYPE.ANNOYING_ORANGE])
            {
                Achievement.Give(Achievement.TYPE.ANNOYING_ORANGE);
            }
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
