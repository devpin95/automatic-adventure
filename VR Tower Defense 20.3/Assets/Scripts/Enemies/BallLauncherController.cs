using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncherController : MonoBehaviour
{

    public EnemyAttributes attributes;
    private float health;
    
    // Start is called before the first frame update
    void Start()
    {
        health = attributes.startingHealth;
        Debug.Log("Ramp has " + health + " health.");
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    public void TakeDirectHit(float damage)
    {
        health -= damage;
        Debug.Log("Ramp has " + health + " health (-" + damage + ")");
    }

    public void TakeIndirectHit(float damage)
    {
        health -= damage;
        Debug.Log("Ramp has " + health + " health (-" + damage + ")");
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
