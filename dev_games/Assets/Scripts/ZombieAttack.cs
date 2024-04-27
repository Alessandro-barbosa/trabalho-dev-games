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
    // Start is called before the first frame update
    void Start()
    {
        zombieAnim = GetComponent<Animator>();
        AIpath = GetComponent<AIPath>();
    }

    // Update is called once per frame
    void Update()
    {
        zombieAnim.SetBool("IsMoving", AIpath.canMove);
        //Debug.Log(AIpath.canMove);
        if (target != null)
        {
            if(Vector3.Distance(target.transform.position, transform.position) > 1.8)
            {
                AIpath.canMove = true;
                target = null;  
            }
            Debug.Log(Vector3.Distance(target.transform.position, transform.position));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colidiu");

        if (collision.gameObject.CompareTag("Player"))
        {
            target = collision.gameObject;
            AIpath.canMove = false;
            Debug.Log(Vector3.Distance(collision.gameObject.transform.position, transform.position));
            zombieAnim.SetTrigger("RightAttackTrigger");
        }
    }
}
