using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 10;

    public void Damage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
