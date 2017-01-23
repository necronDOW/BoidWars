using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int team = 0;
    public Object prefab;
    public List<Waypoint> startingPoints = new List<Waypoint>();
    
    Manager manager;

    private void Start()
    {
        manager = Manager.Instance;
    }

    public void Spawn(int count)
    {
        if (prefab)
        {
            GameObject bO = (Instantiate(prefab, transform.position, Quaternion.identity) as GameObject);
            BoidGroup bG = bO.GetComponent<BoidGroup>();

            bG.SetTeam(team);
            bG.waypoint = startingPoints[Random.Range(0, startingPoints.Count)];

            for (int i = 0; i < count; i++)
                bG.AddToGroup(manager.GetTeamColor(team), 5.0f);

            manager.AddToTeam(team, bO);
        }
    }
}
