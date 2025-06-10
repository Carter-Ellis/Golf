using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private GameObject goose;
    [SerializeField] private BoxCollider2D gooseSpawn;
    [SerializeField] private int minTime = 20;
    [SerializeField] private int maxTime = 60;
    [SerializeField] private float flySpeed = 5;
    private int numGeese;
    private float timer;

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            spawnGeese();
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        timer = Random.Range(minTime, maxTime);
        numGeese = Random.Range(4, 12);
    }

    private void spawnGeese()
    {
        if (goose == null) { return; }
        GameObject geeseGroup = new GameObject();
        geeseGroup.transform.position = gooseSpawn.bounds.center;
        Rigidbody2D rbody = geeseGroup.AddComponent<Rigidbody2D>();
        rbody.isKinematic = true;

        for (int i = 0; i <  numGeese; i++)
        {
            GameObject obj = Instantiate(goose, GetRandomPointInBox(), Quaternion.identity);
            obj.transform.SetParent(geeseGroup.transform, true);
        }

        StudioEventEmitter emitter = geeseGroup.AddComponent<StudioEventEmitter>();
        emitter.EventReference = FMODEvents.instance.geese;
        emitter.PlayEvent = EmitterGameEvent.ObjectStart;
        emitter.StopEvent = EmitterGameEvent.ObjectDestroy;
        emitter.AllowFadeout = true;
        emitter.EventInstance.setVolume(.9f);
        emitter.Play();
        

        rbody.velocity = new Vector2(flySpeed, 0);
        Destroy(geeseGroup, 30f);
        
    }
    Vector3 GetRandomPointInBox()
    {
        Vector3 center = gooseSpawn.bounds.center;
        Vector3 size = gooseSpawn.bounds.size;

        float x = Random.Range(center.x - size.x / 2f, center.x + size.x / 2f);
        float y = Random.Range(center.y - size.y / 2f, center.y + size.y / 2f);

        return new Vector3(x, y, -9.5f);
    }
}
