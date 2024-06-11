using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fogueiraDano : MonoBehaviour
{
    private AimController player;
    private BoxCollider dano;
    public float damage;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<AimController>();
        dano = GetComponent<BoxCollider>();
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colidiu com player");
        }
    }
}
