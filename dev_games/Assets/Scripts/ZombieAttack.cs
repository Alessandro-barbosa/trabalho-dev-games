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
    private ZombieSteps zombieSteps;
    private AIDestinationSetter destinationSet;
    // Start is called before the first frame update
    void Start()
    {
        zombieAnim = GetComponent<Animator>();
        AIpath = GetComponent<AIPath>();
        zombieSteps = GetComponent<ZombieSteps>();
        destinationSet = GetComponent<AIDestinationSetter>();
        destinationSet.target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

        zombieAnim.SetBool("IsMoving", AIpath.canMove);
        //Debug.Log(AIpath.canMove);
        if (target != null) //Se o target não está vazio faz
        {
            if(Vector3.Distance(target.transform.position, transform.position) > 1.8)
            {
                AIpath.canMove = true;
                target = null;
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
        }
    }
}
