using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager
{
    private static Manager instance;

    private Manager() { teams = new List<Team>();  }
    public static Manager Instance
    {
        get
        {
            if (instance == null)
                instance = new Manager();
            return instance;
        }
    }

    public struct Team
    {
        private List<GameObject> objects;
        private Color teamColor;

        public Team(GameObject player, Color c)
        {
            objects = new List<GameObject>();
            objects.Add(player);

            teamColor = c;
            SetColor(objects.Count - 1);
        }

        public void Add(GameObject o)
        {
            objects.Add(o);
            SetColor(objects.Count - 1);
        }

        public void Remove(int index)
        {
            if (index > 0 && index < objects.Count)
                objects.RemoveAt(index);
        }
        public void Remove(GameObject o)
        {
            objects.Remove(o);
        }

        public GameObject GetObject(int index)
        {
            if (index >= 0 && index < objects.Count)
                return objects[index];
            else return null;
        }

        public Color GetColor() { return teamColor; }
        public GameObject GetPlayer() { return objects[0]; }
        public int GetSize() { return objects.Count; }

        private void SetColor(int index)
        {
            if (index >= 0 && index < objects.Count)
            {
                Renderer r = objects[index].GetComponent<Renderer>();
                if (r) r.material.SetColor("_EmissionColor", teamColor);
            }
        }
    };

    private List<Team> teams;
    private int teamCap = 2;

    private void Initialize()
    {
        teams = new List<Team>();
    }

    public int NewTeam(GameObject player)
    {
        Renderer r = player.GetComponent<Renderer>();

        if (r)
        {
            Color c = r.material.GetColor("_EmissionColor");
            return NewTeam(player, c);
        }

        return -1;
    }
    public int NewTeam(GameObject player, Color c)
    {
        if (teams.Count < teamCap)
        {
            teams.Add(new Team(player, c));

            Debug.Log("MANAGER: Added team " + (teams.Count - 1) + ".");
            return teams.Count - 1;
        }
        else Debug.Log("MANAGER: Team cap of " + teamCap + " already reached.");

        return -1;
    }

    public bool AddToTeam(int index, GameObject o)
    {
        if (index >= 0 && index < teams.Count)
        {
            teams[index].Add(o);

            Debug.Log("MANAGER: Added '" + o.name + "' to team " 
                + index + " (count=" + teams[index].GetSize() + ").");
            return true;
        }
        else return false;
    }

    public void Destroy(int teamIndex, int objIndex)
    {
        if (teamIndex >= 0 && teamIndex < teams.Count)
        {
            GameObject o = teams[teamIndex].GetObject(objIndex);
            string oName = o.name;

            Object.DestroyImmediate(o);
            teams[teamIndex].Remove(objIndex);

            Debug.Log("MANAGER: Destroyed '" + oName + "' from team "
                + teamIndex + " (count=" + teams[teamIndex].GetSize() + ").");
        }
    }
    public void Destroy(int teamIndex, GameObject obj)
    {
        if (teamIndex >= 0 && teamIndex < teams.Count)
        {
            string objName = obj.name;

            Object.DestroyImmediate(obj);
            teams[teamIndex].Remove(obj);

            Debug.Log("MANAGER: Destroyed '" + objName + "' from team "
                + teamIndex + " (count=" + teams[teamIndex].GetSize() + ").");
        }
    }

    public Color GetTeamColor(int index)
    {
        if (index >= 0 && index < teams.Count)
            return teams[index].GetColor();
        else return Color.black;
    }

    public Team GetOpponent(int yourIndex)
    {
        int opponentIndex = (yourIndex + 1) % 2;
        return teams[opponentIndex];
    }

    public Team GetTeam(int index)
    {
        return teams[index];
    }

    public GameObject GetPlayer(int index)
    {
        if (index >= 0 && index < teams.Count)
            return teams[index].GetPlayer();
        return null;
    }

    public int GetTeamCount()
    {
        return teams.Count;
    }
}
