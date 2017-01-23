using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public int team = -1;
    public int health = 0;

    Manager manager;
    Explosion coreExplosion;

    private bool markForDeath = false;

    private void Start()
    {
        manager = Manager.Instance;
        manager.AddToTeam(team, transform.parent.gameObject);
        gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", manager.GetTeamColor(team));
    }

    void Update()
    {
        transform.Rotate(20 * Time.deltaTime, 20 * Time.deltaTime, 0);

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
    
    private void Die()
    {
        Destroy(gameObject);
    }
}
