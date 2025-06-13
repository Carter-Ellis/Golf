using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class AbilityTeleport : Ability
{
    public bool isReady;
    public float maxTeleportRange = 3f;
    private int charges = 1;

    public AbilityTeleport(Color color)
    {
        type = ABILITIES.TELEPORT;
        name = "Teleport";
        chargeName = "Tele Orbs";
        description = "Instantly transport the ball from one location to another for quick repositioning";
        upgradeDescription = "Increase teleport range.";
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

        if (isReady)
        {
            DrawCircle(ball.transform.position, maxTeleportRange, 64);
        }
        else
        {
            return;
        }

        if (charges <= 0)
        {
            return;
        }

        Vector3 mousePos;
        if (PlayerInput.isController)
        {
            //Convert cursor position from 0:1 to -1:1 range and invert
            Vector3 cursorDir = -(PlayerInput.rawCursorPosition * 2f - new Vector2(1, 1));
            //Normalize if too large to contain in circle
            if (cursorDir.magnitude > 1f)
            {
                cursorDir.Normalize();
            }
            mousePos = ball.transform.position + cursorDir * maxTeleportRange;
            ball.cursor.transform.position = new Vector3(mousePos.x, mousePos.y, -9.7f);
        }
        else
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (!PlayerInput.isUp(PlayerInput.Axis.Fire1))
        {
            return;
        }

        if (GameObject.Find("Pause Screen") != null && GameObject.Find("Pause Screen").activeSelf)
        {
            return;
        }

        float distance = Vector2.Distance(mousePos, ball.transform.position);
        if (distance > maxTeleportRange)
        {
            return;
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(mousePos);
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);

        if (hits == null)
        {
            return;
        }

        bool hitBackground = false;
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider == null) continue;

            if (hit.collider.gameObject.CompareTag("Background"))
            {
                hitBackground = true;
                break;
            }

        }

        if (!hitBackground)
        {
            return;
        }

        LayerMask blockingLayers = LayerMask.GetMask("Foreground");
        Collider2D overlap = Physics2D.OverlapCircle(mousePos, 0.2f, blockingLayers);

        if (overlap != null)
        {
            return;
        }

        AudioManager.instance.PlayOneShot(FMODEvents.instance.teleport, GameObject.FindObjectOfType<Ball>().transform.position);
        ball.transform.position = mousePos;
        PlayerInput.cursorSpeed /= 2;
        ball.DisplayTeleportParticles();
        charges--;
        isReady = false;
        ball.isTeleportReady = false;
        ball.isBallLocked = false;
        if (GameObject.Find("Pause Screen") != null && GameObject.Find("Pause Screen").activeSelf)
        {
            ball.isBallLocked = false;
        }

        ball.cursor.GetComponent<SpriteRenderer>().enabled = false;
        ball.GetComponent<SpriteRenderer>().color = ball.GetComponent<Inventory>().ballColor;
        GameObject existing = GameObject.Find("TeleportCircle");
        if (existing != null)
        {
            GameObject.Destroy(existing);
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

    public override void onUse(Ball ball)
    {
        if (GameObject.Find("Pause Screen") != null && GameObject.Find("Pause Screen").activeSelf)
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
            ball.cursor.GetComponent<SpriteRenderer>().enabled = PlayerInput.isController;
            PlayerInput.resetCursor();
            PlayerInput.cursorSpeed *= 2;
        }
        else
        {
            ball.cursor.GetComponent<SpriteRenderer>().enabled = false;
            GameObject tpCircle = GameObject.Find("TeleportCircle");
            if (tpCircle != null) GameObject.Destroy(tpCircle);
            PlayerInput.cursorSpeed /= 2;
        }

    }

    public override void reset(Ball ball)
    {
        SpriteRenderer sr = ball.GetComponent<SpriteRenderer>();
        isReady = false;
        ball.canPutt = true;
        sr.color = ball.GetComponent<Inventory>().ballColor;
        GameObject existing = GameObject.Find("TeleportCircle");
        if (existing != null)
        {
            GameObject.Destroy(existing);
        }
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

    public override void onBallDisabled(Ball ball)
    {
        GameObject existing = GameObject.Find("TeleportCircle");
        if (existing != null)
        {
            GameObject.Destroy(existing);
        }
    }
}
