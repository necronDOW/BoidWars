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

    private List<Member> _members;
    private Color _diffuse, _emission;

    public Team(GameObject owner)
    {
        _members.Add(new Member(owner));
        _diffuse = MaterialHelper.GetColor(owner, "_Color");
        _emission = MaterialHelper.GetColor(owner, "_EmissionColor");
    }
    public Team(GameObject owner, float emissionMultiplier)
    {
        _members.Add(new Member(owner));
        _diffuse = MaterialHelper.GetColor(owner, "_Color");
        _emission = _diffuse * emissionMultiplier;
    }
    public Team(GameObject owner, Color diffuse, Color emission)
    {
        _members.Add(new Member(owner));
        _diffuse = diffuse;
        _emission = emission;
    }
    public Team(GameObject owner, Color diffuse, float emissionMultiplier)
    {
        _members.Add(new Member(owner));
        _diffuse = diffuse;
        _emission = diffuse * emissionMultiplier;
    }

    public void Subscribe(GameObject gameObject)
    {
        _members.Add(new Member(gameObject));

        MaterialLibrary.Get().SetRenderColor(gameObject.GetComponent<MeshRenderer>(), _diffuse, _emission);
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
        else if (index > 0 && index < _members.Count)
        {
            RemoveMember(index);
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
        if (index > 1 && index < _members.Count)
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
        if (startIndex > 0 && startIndex < _members.Count)
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
        MaterialLibrary.Get().SetRenderColor(m.gameObject.GetComponent<MeshRenderer>(), m.originalDiffuse, m.originalEmission);

        _members.RemoveAt(index);
    }
}

public class GameManager : MonoBehaviour
{
    public Team[] teams;
}
