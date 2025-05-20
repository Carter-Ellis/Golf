using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private GameObject goose;
    [SerializeField] private BoxCollider2D gooseSpawn;
    [SerializeField] private int minTime = 1;
    [SerializeField] private int maxTime = 3;
    private int numGeese;
    private float timer;
    void Start()
    {
        numGeese = Random.Range(4, 12);
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
    }

    private void spawnGeese()
    {
        if (goose == null) { return; }
        for (int i = 0; i <  numGeese; i++)
        {
            GameObject obj = Instantiate(goose, gooseSpawn.transform.position, Quaternion.identity);
            obj.transform.position = GetRandomPointInBox();
            obj.GetComponent<Rigidbody2D>().velocity = new Vector2(goose.GetComponent<Goose>().flySpeed, 0);
            Destroy(obj, 30f);
        }
        
        
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
