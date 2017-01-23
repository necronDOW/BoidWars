using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Shot : MonoBehaviour {

    // Use this for initialization
    public float speed;
    private Rigidbody Bullet;
    public float lifetime = 2.0f;

    public int exp = 5;
    public int team = -1;

    private void Awake()
    {
        Destroy(gameObject, lifetime);
        Destroy(gameObject.transform.parent.gameObject, lifetime);
        Bullet = GetComponent<Rigidbody>();
    }
    void Start()
    {
        transform.Rotate(Random.Range(-20.0f, 20.0f), Random.Range(-20.0f, 20.0f), 0.0f);
        Bullet.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update () {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            BoidBehaviour bb = other.GetComponent<BoidBehaviour>();

            if (bb.group.GetTeam() != team)
            {
                PlayerStats playerLvl = Manager.Instance.GetPlayer(team).GetComponent<PlayerStats>();

                playerLvl.exp += exp;
                playerLvl.checkLevel();
                other.gameObject.GetComponent<MinionInteractions>().Damage(1);

                Destroy(gameObject);
            }
        }
        if (other.tag == "Wall")
        { 
            if (other.gameObject.GetComponent<Gate>().team != team)
                Destroy(gameObject);
        }

        if (other.tag == "Player")
        {
            if (other.gameObject.GetComponent<PlayerController>().index != team)
            {
                other.gameObject.GetComponent<PlayerController>().health -= 1;

                Destroy(gameObject);
            }
               
        }
    }
}
