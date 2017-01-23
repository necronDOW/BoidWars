using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionInteractions : MonoBehaviour {

    public int health;
    public int exp = 5;
    BoxCollider hitBox;

	// Use this for initialization
	void Start ()
    {
        hitBox = GetComponent<BoxCollider>();
        gameObject.AddComponent<Explosion>();
	}

    public void Damage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            health = 0;
            death();
        }
    }

    public void Drain(ref int targetHP)
    {
        int damage = targetHP;
        targetHP -= health;

        Damage(damage);
    }

    private void death ()
    {
        StartCoroutine(gameObject.GetComponent<Explosion>().meshSplit(true, 5));
    }
}
