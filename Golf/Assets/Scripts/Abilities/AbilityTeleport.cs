using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class AbilityTeleport : Ability
{
    private GameObject ballMarker;
    private int charges = 1;
    public bool isReady;
    public float maxTeleportRange = 3f;

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
        return Ability.maxChargesByType[ABILITIES.TELEPORT];
    }

    public override void onFrame(Ball ball)
    {
        if (GameObject.FindObjectOfType<PauseManager>() == null || GameObject.FindObjectOfType<PauseManager>().GetComponent<Canvas>().enabled)
        {
            return;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (isReady)
        {
            DrawCircle(ball.transform.position, maxTeleportRange, 64);
        }
        else
        {
            GameObject existing = GameObject.Find("TeleportCircle");
            if (existing != null) GameObject.Destroy(existing);
        }

        float distance = Vector2.Distance(mousePos, ball.transform.position);
        if (distance > maxTeleportRange)
        {
            return;
        }

        if (isReady && Input.GetMouseButtonUp(0) &&
            hit.collider != null && hit.collider.gameObject.CompareTag("Background"))
        {
            if (charges <= 0)
            {
                return;
            }

            LayerMask blockingLayers = LayerMask.GetMask("Foreground");
            Collider2D overlap = Physics2D.OverlapCircle(mousePos, 0.2f, blockingLayers);

            if (overlap != null)
            {
                return;
            }

            ball.transform.position = mousePos;
            charges--;
            isReady = false;
            ball.isTeleportReady = false;
            ball.isBallLocked = false;
            if (GameObject.Find("Pause Screen").gameObject.activeSelf)
            {
                ball.isBallLocked = false;
            }

            ball.GetComponent<SpriteRenderer>().color = ball.GetComponent<Inventory>().ballColor;
            GameObject existing = GameObject.Find("TeleportCircle");
            if (existing != null) GameObject.Destroy(existing);
        }
    }

    public override void onPickup(Ball ball)
    {
        charges = getMaxCharges(ball);
        float range = ball.GetComponent<Inventory>().teleportRange;
        maxTeleportRange = range <= 0 ? 3 : range;
    }

    public override void onRecharge(Ball ball)
    {
        if (charges < getMaxCharges(ball))
        {
            charges++;
        }
    }

    public override void setCharges(int amount)
    {
        charges = amount;
    }
    /*public override void setRange(int amount)
    {
        maxTeleportRange = amount;
    }*/

    public override void onUse(Ball ball)
    {
        if (GameObject.FindObjectOfType<PauseManager>() == null || GameObject.FindObjectOfType<PauseManager>().GetComponent<Canvas>().enabled)
        {
            return;
        }
        if (charges <= 0)
        {
            return;
        }

        SpriteRenderer sr = ball.GetComponent<SpriteRenderer>();

        isReady = !isReady;
        ball.isTeleportReady = isReady;
        sr.color = isReady ? Color.magenta : ball.GetComponent<Inventory>().ballColor;
        ball.isBallLocked = isReady;

        GameObject existing = GameObject.Find("TeleportCircle");
        if (existing != null) GameObject.Destroy(existing);

        if (isReady)
        {
            ball.hasClickedBall = false;
            ball.cursor.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public override void reset(Ball ball)
    {
        SpriteRenderer sr = ball.GetComponent<SpriteRenderer>();
        isReady = false;
        ball.canPutt = true;
        sr.color = ball.GetComponent<Inventory>().ballColor;
    }

    private void DrawCircle(Vector3 center, float radius, int segments)
    {
        LineRenderer line = GameObject.Find("TeleportCircle")?.GetComponent<LineRenderer>();
        if (line == null)
        {
            GameObject go = new GameObject("TeleportCircle");
            line = go.AddComponent<LineRenderer>();
            line.useWorldSpace = true;
            line.loop = true;
            line.startWidth = 0.05f;
            line.endWidth = 0.05f;
            line.positionCount = segments;
            line.material = new Material(Shader.Find("Sprites/Default"));
            line.startColor = Color.magenta;
            line.endColor = Color.magenta;
        }

        Vector3[] points = new Vector3[segments];
        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            points[i] = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
        }

        line.positionCount = segments;
        line.SetPositions(points);
    }
}
