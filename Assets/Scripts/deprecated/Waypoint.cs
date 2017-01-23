using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint next;
    public bool isFirst = false;
    public int teamIndex = -1;
    
    protected bool active = false;

    private void Start()
    {
        if (isFirst)
        {
            CascadeTeam(teamIndex);
            CalculatePath();
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(transform.position, Vector3.one);
        if (next)
            Gizmos.DrawLine(transform.position, next.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        BoidGroup bG = other.GetComponent<BoidGroup>();

        if (bG && bG.GetTeam() == teamIndex && next)
            bG.waypoint = next;
    }

    public virtual void CascadeTeam(int index)
    {
        teamIndex = index;

        if (next)
            next.CascadeTeam(index);
    }

    public void CascadeEraseLine()
    {
        active = false;

        LineRenderer r = GetComponent<LineRenderer>();

        if (next && r)
        {
            r.SetPosition(0, Vector3.zero);
            r.SetPosition(1, Vector3.zero);

            next.CascadeEraseLine();
        }
    }
    
    protected virtual void CalculatePath()
    {
        active = true;

        LineRenderer r = GetComponent<LineRenderer>();

        if (next && r)
        {
            r.useWorldSpace = true;

            r.SetPosition(0, transform.position + new Vector3(0f, 1f, 0f));
            r.SetPosition(1, next.transform.position + new Vector3(0f, 1f, 0f));

            r.material.SetColor("_EmissionColor", Manager.Instance.GetTeamColor(teamIndex));
            next.CalculatePath();
        }
    }

    protected virtual void SetSwitches()
    {

    }
}
