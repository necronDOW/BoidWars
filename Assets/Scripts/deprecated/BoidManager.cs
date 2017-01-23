using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour {
    //
    // this will be added to a game manager class!!!
    //

    Vector3 focus;
    int timer;
    int NextFocusPoint;

    List<BoidBehaviour> Boids;
	// Use this for initialization
	void Start ()
    {
        Boids = new List<BoidBehaviour>();
        //BoidsBehaviour[] tempBoids = FindObjectsOfType(typeof(BoidsBehaviour)) as BoidsBehaviour[];
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < objs.Length; i++)
        {
            Boids.Add(objs[i].GetComponent<BoidBehaviour>());
        }

        timer = 0;
        NextFocusPoint = 0;
        focus = new Vector3(0.0f, 0.0f, 0.0f);

    }

    // Update is called once per frame
    void Update ()
    {
        for (int i = 0; i < Boids.Count; i++)
        {
            Boids[i].computeForce();
        }

        //timer++;
        //if (timer > 29)
        //{
        //    timer = 0;
        //    focus.x = Random.Range(-50.0f, 50.0f);          
        //    focus.z = Random.Range(-50.0f, 50.0f);
        //    focus.y = 0.0f;
        //}
    }
}
