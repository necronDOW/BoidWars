using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepSpawner : MonoBehaviour
{
    public GameObject prefab;
    public int count = 1;
    public int threshhold = 50;
    public float prefabSpeed = 1.0f;
    public Waypoint[] nextWaypoints;

    private BoidGroup group;
    private int feedValue;

    private void Start()
    {
        group = CreateBoidGroup();

        for (int i = 0; i < count; i++)
        {
            group.AddToGroup(Color.white, 2.5f);
            group.boids[i].enabled = false;
            group.boids[i].GetComponent<Collider>().enabled = false;
        }

        group.enabled = false;
    }

    private BoidGroup CreateBoidGroup()
    {
        GameObject grp = new GameObject("Creep Group");
        grp.transform.position = transform.position;
        grp.layer = LayerMask.NameToLayer("Waypoints");

        BoidGroup bG = grp.AddComponent<BoidGroup>();
        bG.boidPrefab = prefab;
        bG.speed = 1;
        bG.canPanic = false;
        bG.speed = prefabSpeed;

        grp.AddComponent<BoxCollider>().isTrigger = true;

        Rigidbody r = grp.AddComponent<Rigidbody>();
        r.useGravity = false;
        r.constraints = RigidbodyConstraints.FreezePositionY;

        return bG;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            MinionInteractions eI = other.GetComponent<MinionInteractions>();
            BoidBehaviour bB = other.GetComponent<BoidBehaviour>();

            int team = bB.group.GetTeam();

            if (team >= 0)
            {
                int hp = eI ? eI.health : 1;

                if (team == 0) feedValue -= hp;
                else if (team == 1) feedValue += hp;

                eI.Damage(hp);

                if (TransferOwnership(CheckOwnership()))
                    group.waypoint = nextWaypoints[team];
            }
        }
    }

    private int CheckOwnership()
    {
        if (feedValue <= -threshhold)
            return 0;
        else if (feedValue >= threshhold)
            return 1;

        return -1;
    }

    private bool TransferOwnership(int team)
    {
        if (team != -1)
        {
            GetComponent<SphereCollider>().enabled = false;

            group.enabled = true;

            for (int i = 0; i < count; i++)
            {
                group.boids[i].enabled = true;
                group.boids[i].GetComponent<Collider>().enabled = true;
            }

            group.SetTeam(team);
            group.ResetColor();
            Manager.Instance.AddToTeam(team, group.gameObject);

            return true;
        }

        return false;
    }
}
