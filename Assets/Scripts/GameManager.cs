using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    struct Member
    {
        public GameObject gameObject;
        public Color originalDiffuse;
        public Color originalEmission;

        public Member(GameObject g)
        {
            gameObject = g;
            originalDiffuse = MaterialHelper.GetColor(g, "_Color");
            originalEmission = MaterialHelper.GetColor(g, "_EmissionColor");
        }
    }

    private List<Member> _members = new List<Member>();
    private Color _diffuse, _emission;
    
    public Team(GameObject owner)
    {
        _diffuse = MaterialHelper.GetColor(owner, "_Color");
        _emission = MaterialHelper.GetColor(owner, "_EmissionColor");
        Subscribe(owner);
    }
    public Team(GameObject owner, float emissionMultiplier)
    {
        _diffuse = MaterialHelper.GetColor(owner, "_Color");
        _emission = _diffuse * emissionMultiplier;
        Subscribe(owner);
    }
    public Team(GameObject owner, Color diffuse, Color emission)
    {
        _diffuse = diffuse;
        _emission = emission;
        Subscribe(owner);
    }
    public Team(GameObject owner, Color diffuse, float emissionMultiplier)
    {
        _diffuse = diffuse;
        _emission = diffuse * emissionMultiplier;
        Subscribe(owner);
    }

    public void Subscribe(GameObject gameObject)
    {
        if (!IsMember(gameObject))
        {
            _members.Add(new Member(gameObject));

            MaterialLibrary.Get().SetRenderColor(gameObject.GetComponent<MeshRenderer>(), _diffuse, _emission);
        }
    }
    public void Unsubscribe(GameObject gameObject)
    {
        if (gameObject == _members[0].gameObject)
        {
            Debug.Log("The object '" + gameObject.name + "' is the team owner and cannot be un-subscribed.");
        }
        else
        {
            for (int i = 0; i < _members.Count; i++)
            {
                if (gameObject == _members[i].gameObject)
                {
                    RemoveMember(i);
                    return;
                }
            }
        }
    }
    public void Unsubscribe(int index)
    {
        if (index == 0)
        {
            Debug.Log("Index 0 is reserved for the team owner who cannot be un-subscribed.");
        }
        else
        {
            RemoveMember(index);
            return;
        }
    }
    public void UnsubscribeAll()
    {
        while (_members.Count > 0)
        {
            RemoveMember(0);
        }
    }
    public void UnsubscribeAllButPlayer()
    {
        while (_members.Count > 1)
        {
            RemoveMember(0);
        }
    }

    public bool IsMember(GameObject gameObject)
    {
        for (int i = 0; i < _members.Count; i++)
        {
            if (gameObject == _members[i].gameObject)
            {
                return true;
            }
        }

        return false;
    }

    public GameObject GetMember(int index)
    {
        if (index > 0 && index < _members.Count)
        {
            return _members[index].gameObject;
        }

        return null;
    }
    public GameObject GetOwner()
    {
        return _members[0].gameObject;
    }

    public void DestroyAll()
    {
        DestroyFrom(0);
    }
    public void DestroyAllButPlayer()
    {
        DestroyFrom(1);
    }
    private void DestroyFrom(int startIndex)
    {
        if (startIndex >= 0 && startIndex < _members.Count)
        {
            for (int i = startIndex; i < _members.Count; i++)
            {
                Object.Destroy(_members[i].gameObject);
            }
        }

        _members.RemoveRange(startIndex, _members.Count - startIndex);
    }

    private void RemoveMember(int index)
    {
        Member m = _members[index];
        if (m.gameObject)
        {
            MaterialLibrary.Get().SetRenderColor(m.gameObject.GetComponent<MeshRenderer>(), m.originalDiffuse, m.originalEmission);
        }

        _members.RemoveAt(index);
    }
}

public class GameManager : MonoBehaviour
{
    private List<Team> _teams = new List<Team>();
    public List<bool> teamFoldouts = new List<bool>();

    public bool AddTeam(GameObject owner)
    {
        if (!IsInAnyTeam(owner))
        {
            _teams.Add(new Team(owner));
            return true;
        }

        return false;
    }
    public bool AddTeam(GameObject owner, Color diffuse, float emissionMultiplier)
    {
        if (!IsInAnyTeam(owner))
        {
            _teams.Add(new Team(owner, diffuse, emissionMultiplier));
            return true;
        }

        return false;
    }
    public bool AddToTeam(GameObject gameObject, int teamIndex)
    {
        if (!IsInAnyTeam(gameObject, teamIndex))
        {
            _teams[teamIndex].Subscribe(gameObject);
            return true;
        }

        return false;
    }
    public bool RemoveTeam(int index)
    {
        if (index >= 0 && index < _teams.Count)
        {
            _teams[index].UnsubscribeAll();
            _teams.RemoveAt(index);
            return true;
        }

        return false;
    }
    public Team GetTeam(int index)
    {
        if (index >= 0 && index < _teams.Count)
        {
            return _teams[index];
        }

        else return null;
    }
    public int TeamCount()
    {
        return _teams.Count;
    }

    private bool IsInAnyTeam(GameObject gameObject, int exclude = -1)
    {
        for (int i = 0; i < _teams.Count; i++)
        {
            if (i != exclude)
            {
                if (_teams[i].IsMember(gameObject))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
