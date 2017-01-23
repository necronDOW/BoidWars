using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junction : Waypoint
{
    public Object switchPrefab;
    public Waypoint[] options;
    public int _optionIndex = 0;
    public Junction previous;

    private GameObject[] switches;

    private void Start()
    {
        if (isFirst)
        {
            CascadeTeam(teamIndex);

            if (options[_optionIndex])
                next = options[_optionIndex];
            CalculatePath();
        }
    }

    bool switched = false;
    private void Update()
    {
        if (Vector3.Distance(Manager.Instance.GetPlayer(teamIndex).transform.position, transform.position) < 5.0f)
        {
            if (!switched && Input.GetAxis("Submit") == 1f)
            {
                Switch();
                switched = true;
            }
            else if (switched && Input.GetAxis("Submit") == 0f)
                switched = false;
        }
    }

    public void Switch()
    {
        int tmp = (_optionIndex + 1) % 2;

        switches[_optionIndex].GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
        switches[tmp].GetComponent<Renderer>().material.SetColor("_EmissionColor", Manager.Instance.GetTeamColor(teamIndex) * 3.0f);

        bool wasActive = active;
        CascadeEraseLine();

        if (options[tmp])
        {
            next = options[tmp];
            _optionIndex = tmp;
        }

        if (wasActive)
            CalculatePath();

        if (previous)
            previous.CascadeCalculatePath();
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, Vector3.one);

        if (options[_optionIndex])
        {
            Gizmos.DrawLine(transform.position, options[_optionIndex].transform.position);
        }

        int tmp = (_optionIndex + 1) % 2;

        if (options[tmp])
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, options[tmp].transform.position);
        }
    }

    protected override void CalculatePath()
    {
        next = options[_optionIndex];

        base.CalculatePath();
    }

    private void CascadeCalculatePath()
    {
        if (!previous)
            CalculatePath();
        else previous.CascadeCalculatePath();
    }

    public override void CascadeTeam(int index)
    {
        teamIndex = index;
        
        SetSwitches();

        for (int i = 0; i < options.Length; i++)
        {
            if (options[i])
            {
                options[i].CascadeTeam(index);

                if (options[i] is Junction && this is Junction)
                    (options[i] as Junction).previous = this;
            }
        }
    }

    protected override void SetSwitches()
    {
        if (switchPrefab)
        {
            switches = new GameObject[options.Length];

            Vector3 option0Normalized = (options[0].transform.position - transform.position).normalized;
            Vector3 option1Normalized = (options[1].transform.position - transform.position).normalized;
            Vector3 angleOffset = (option0Normalized - option1Normalized).normalized;

            switches[0] = (GameObject)Instantiate(switchPrefab, transform.position + angleOffset, Quaternion.Euler(-90.0f, 0f, 0f));
            switches[0].hideFlags = HideFlags.HideInHierarchy;
            switches[1] = (GameObject)Instantiate(switchPrefab, transform.position - angleOffset, Quaternion.Euler(-90.0f, 0f, 0f));
            switches[1].hideFlags = HideFlags.HideInHierarchy;

            switches[_optionIndex].GetComponent<Renderer>().material.SetColor("_EmissionColor", Manager.Instance.GetTeamColor(teamIndex) * 3.0f);
        }
    }
}
