using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidBehaviour : MonoBehaviour {

    Vector3 force;
    Vector3 acceleration;
    Vector3 velocity;
    public GameObject focus;
    public bool swarm = true;

    float mass;
    float startY;

    float boidFocusFactor;
    float boidAttractFactor;
    float boidAttractRadius;
    float boidRepelFactor;
    float boidRepelRadius;
    float boidAlignFactor;
    float boidAlignRadius;

    Rigidbody rb;

    public BoidGroup group;

    // Use this for initialization
    void Start ()
    {
        force = new Vector3(0.0f, 0.0f, 0.0f);
        acceleration = new Vector3(0.0f, 0.0f, 0.0f);
        velocity = new Vector3(0.0f, 0.0f, 0.0f);
        mass = 0.5f;

        boidFocusFactor = Random.Range(20.0f, 30.0f);
        boidAttractFactor = Random.Range(5.0f, 10.0f);
        boidAttractRadius = Random.Range(7.5f, 10.0f);
        boidRepelFactor = Random.Range(7.5f, 12.5f);
        boidRepelRadius = Random.Range(2.5f, 5.0f);
        boidAlignFactor = Random.Range(1.0f, 4.0f);
        boidAlignRadius = Random.Range(2.5f, 7.5f);

        rb = GetComponent<Rigidbody>();

        startY = transform.position.y;
    }
	
	// Update is called once per frame
	void Update ()
    {
        acceleration = force * mass;
        velocity = velocity + (acceleration * Time.deltaTime);
        transform.position = transform.position + (velocity * Time.deltaTime);

        // update position of prefab
        rb.velocity = velocity;

        // align object with velocity
        transform.LookAt(velocity * 2.0f);

        transform.position = new Vector3(transform.position.x, startY, transform.position.z);

        if (swarm)
            computeForce();
        else
            transform.position = Vector3.MoveTowards(transform.position, group.transform.position, Mathf.Infinity);
    }

    public void computeForce()
    {
        Vector3 direction = (focus.transform.position - transform.position);
        direction.Normalize();
        direction = (direction * boidFocusFactor);
        force = direction;

        Vector3 CohesionForce = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 SeparationForce = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 AlignmentForce = new Vector3(0.0f, 0.0f, 0.0f);
        for (int i = 0; i < group.boids.Count; i++)
        {
            if (group.boids[i] != this && group.boids[i] != null)
            {
                Vector3 displacement = group.boids[i].transform.position - transform.position;
                float distance = displacement.magnitude;
                displacement.Normalize();

                if (distance < boidAttractRadius)
                {
                    CohesionForce = displacement;
                    force += (CohesionForce * boidAttractFactor);
                }

                if (distance < boidRepelRadius)
                {
                    SeparationForce = displacement;
                    force -= (SeparationForce * boidRepelFactor);
                }

                if (distance < boidAlignRadius)
                {
                    AlignmentForce = group.boids[i].velocity - velocity;
                    AlignmentForce.Normalize();
                    force += (AlignmentForce * boidAlignFactor);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<BoidBehaviour>().group.GetTeam() != group.GetTeam())
                collision.gameObject.GetComponent<MinionInteractions>().Damage(1);
        }
    }
}
