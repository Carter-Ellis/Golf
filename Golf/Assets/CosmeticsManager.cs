using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CosmeticsManager : MonoBehaviour
{

    [SerializeField] private Image hatImage;
    [SerializeField] private Image _lock;
    [SerializeField] private Image hatMirror;
    private Hat.TYPE type;
    private Inventory inv;
    private void Start()
    {

        type = Hat.TYPE.NONE + 1;
        inv = FindObjectOfType<Inventory>();

        changeHat(0);

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
        hatMirror.sprite = hatImage.sprite;
        Vector2 spriteSize = hatMirror.sprite.rect.size;
        hatMirror.rectTransform.sizeDelta = spriteSize * 10;
        //Check if locked
        if (inv.unlockedHats.ContainsKey(type) && inv.unlockedHats[type])
        {
            _lock.enabled = false;
            print(_lock.enabled);
        }
        else
        {
            _lock.enabled = true;
        }

    }

    public void GoRight()
    {

        changeHat(1);
        
    }

    public void GoLeft()
    {

        changeHat(-1);

    }

}
