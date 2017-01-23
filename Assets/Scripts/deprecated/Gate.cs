using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {

    public int team = -1;
    public int health = 0;

    Manager manager;
    GameObject[] towers;

    private bool markForDeath = false;

    private void Start()
    {
        manager = Manager.Instance;

        if (team >= 0)
        {
            manager.AddToTeam(team, gameObject);

            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            towers = new GameObject[renderers.Length];

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.SetColor("_EmissionColor", manager.GetTeamColor(team));
                renderers[i].gameObject.AddComponent<Explosion>();

                towers[i] = renderers[i].gameObject;
            }
        }
    }
    
    private void Update()
    {
        if (markForDeath)
            Die();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            BoidBehaviour bb = other.GetComponent<BoidBehaviour>();

            if (bb.group.GetTeam() != team)
            {
                PlayerStats playerLvl = Manager.Instance.GetPlayer(team).GetComponent<PlayerStats>();
                other.gameObject.GetComponent<MinionInteractions>().Drain(ref health);

                if (health <= 0)
                    markForDeath = true;
            }
        }
    }

    float timer = 0.0f;
    float timerTarget = -1.0f;
    private void Die()
    {
        if (timerTarget < 0.0f)
        {
            for (int i = 0; i < towers.Length; i++)
                StartCoroutine(towers[i].GetComponent<Explosion>().meshSplit(true, 5));

            GetComponent<BoxCollider>().enabled = false;

            timerTarget = towers[0].GetComponent<Explosion>().waitTime;
        }

        timer += Time.deltaTime;

        if (timer >= timerTarget)
            manager.Destroy(team, gameObject);
    }
}
