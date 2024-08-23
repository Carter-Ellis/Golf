using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Ball ball;
    private float currHealth;
    public Scrollbar bar;
    public Image barColor;

    public Color green;
    public Color yellow;
    public Color red;
    private void Start()
    {
        ball = FindObjectOfType<Ball>();
    }

    private void Update()
    {
        if (ball == null) return;
        currHealth = (float)(ball.health / 100f);
        bar.size = currHealth;
        UpdateColor(currHealth);
    }

    void UpdateColor(float value)
    {
        if (value <= 0.5f)
        {      
            barColor.color = Color.Lerp(red, yellow, value * 2f);
        }
        else
        {
            barColor.color = Color.Lerp(yellow, green, (value - 0.5f) * 2f);
        }


    }

}
