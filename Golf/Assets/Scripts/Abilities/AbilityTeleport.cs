using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class AbilityTeleport : Ability
{
    private GameObject ballMarker;
    private int charges = 100;
    private int maxCharges = 100;
    bool isReady;

    public AbilityTeleport(Color color)
    {
        type = ABILITIES.TELEPORT;
        name = "Teleport";
        chargeName = "Tele Orbs";
        description = "Instantly transport the ball from one location to another for quick repositioning";
        this.color = color;
    }

    public override int getCharges(Ball ball)
    {
        return charges;
    }

    public override int getMaxCharges(Ball ball)
    {
        return maxCharges;
    }

    public override void onFrame(Ball ball)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        ballMarker = GameObject.FindGameObjectWithTag("BallMarker");
        if (isReady && Input.GetMouseButtonUp(1) && hit.collider == null)
        {
            
            if (charges <= 0)
            {
                return;
            }
            ballMarker.GetComponent<SpriteRenderer>().enabled = true; 
            ballMarker.transform.position = ball.GetComponent<Transform>().position;
            ball.GetComponent<Transform>().position = mousePos;
            charges--;
            isReady = false;
            ball.canPutt = true;
            ball.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public override void onPickup(Ball ball)
    {
        charges = maxCharges;
    }

    public override void onRecharge(Ball ball)
    {
        if (charges < maxCharges)
        {
            charges++;
        }
    }

    public override void onUse(Ball ball)
    {
        SpriteRenderer sr = ball.GetComponent<SpriteRenderer>();

        isReady = isReady ? false : true;
        sr.color = isReady ? Color.magenta : Color.white;
        ball.canPutt = ball.canPutt ? false : true;
        
    }
    public override void reset(Ball ball)
    {
        SpriteRenderer sr = ball.GetComponent<SpriteRenderer>();
        isReady = false;
        ball.canPutt = true;
        sr.color = Color.white;
    }

}
