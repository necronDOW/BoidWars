using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class Environment : MonoBehaviour
{
    public float baseHeight = 0.1f;
    public GameObject[] effectedObjects;
    public GameObject[] bases;

    private AvgFrequency _avgFreq;

    private void Start()
    {
        _avgFreq = GetComponent<AvgFrequency>();
        bases = GameObject.FindGameObjectsWithTag("Base");
    }

    float timer = 0.0f;
    private void Update()
    {
        timer += Time.deltaTime;

        if (_avgFreq)
        {
            float modifierValue = _avgFreq.GetValue(6) + _avgFreq.GetValue(1);
            float tmp = 0f;

            for (int i = 0; i < effectedObjects.Length; i++)
            {
                Transform t = effectedObjects[i].transform;

                t.localScale = new Vector3(1f, Mathf.SmoothDamp(t.localScale.y, baseHeight + (modifierValue * 2f), ref tmp, 0.1f), 1f);
            }

            Camera.main.GetComponent<Bloom>().bloomIntensity = 1f + modifierValue;

            if ((timer >= 7.5f) || (timer >= 5.0f && modifierValue >= 0.5f))
            {
                foreach (GameObject g in bases)
                {
                    Spawner s = g.GetComponent<Spawner>();

                    if (s)
                        s.Spawn(15);
                }

                timer = 0.0f;
            }
        }
    }
}
