using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    private GameObject target;
    private Animator zombieAnim;
    private AIPath AIpath;
    private AIDestinationSetter destinationSet;
    private AimController player;
    public float damage;
    private float hitRate = 1f;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        zombieAnim = GetComponent<Animator>();
        AIpath = GetComponent<AIPath>();
        destinationSet = GetComponent<AIDestinationSetter>();
        destinationSet.target = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<AimController>();
        damage = 10;
    }

    // Update is called once per frame
    void Update()
    {

        zombieAnim.SetBool("IsMoving", AIpath.canMove);
        //Debug.Log(AIpath.canMove);
        if (target != null) //Se o target não estEvazio faz
        {
            if(Vector3.Distance(target.transform.position, transform.position) > 1.8)
            {
                AIpath.canMove = true;
                target = null;
            }
            timer += Time.deltaTime;
            if (timer > hitRate) { 
                StartCoroutine(ZombieDamage());
                timer = 0;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colidiu com player");
            target = collision.gameObject;
            AIpath.canMove = false;
            Debug.Log(Vector3.Distance(collision.gameObject.transform.position, transform.position));
            zombieAnim.SetTrigger("RightAttackTrigger");
            StartCoroutine(ZombieDamage());
        }
    }
    IEnumerator ZombieDamage()
    {
        yield return new WaitForSeconds(0);
        if (target != null)
        {
            player.getHitZombie(damage);
        }
    }
}
