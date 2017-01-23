using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnTimer : MonoBehaviour
{
    private Spawner spawner;

    private void Start()
    {
        spawner = GetComponent<Spawner>();
    }

    float timer = 0.0f;
    private void Update()
    {
        timer += Time.deltaTime;

        if (spawner && timer > 2.0f)
        {
            spawner.Spawn(10);
            timer = 0.0f;
        }
    }
}
