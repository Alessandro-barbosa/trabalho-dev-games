using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ZombieManager : MonoBehaviour
{
    private GameObject target;
    private Animator zombieAnim;
    private AIPath AIpath;
    private AIDestinationSetter destinationSet;
    private AimController player;
    public float damage;
    private float hitRate = 1f;
    private float timer = 0;
    private string movementLayer;
    public Image healthBar;

    public enum Zumbi
    {
        Normal,
        Rapido,
        Grande,
        GrandeRapido,
        Boss
    }

    public int maxHealth;
    private int currentHealth;

    void Awake()
    {
        zombieAnim = GetComponent<Animator>();
        AIpath = GetComponent<AIPath>();
        destinationSet = GetComponent<AIDestinationSetter>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<AimController>();
        if (destinationSet != null && player != null)
        {
            destinationSet.target = player.transform;
        }
    }

    public void NewStart(Zumbi tipo)
    {
        if (AIpath == null)
        {
            Debug.LogError("AIPath component is not found on the zombie!");
            return;
        }

        switch (tipo)
        {
            case Zumbi.Normal:
                movementLayer = "Walking Layer";
                AIpath.maxSpeed = 2;
                damage = 10f;
                maxHealth = 100;
                break;
            case Zumbi.Rapido:
                movementLayer = "Running Layer";
                AIpath.maxSpeed = 4;
                damage = 8f;
                maxHealth = 80;
                break;
            case Zumbi.Grande:
                movementLayer = "Walking Layer";
                AIpath.maxSpeed = 2;
                transform.localScale *= 1.3f;
                damage = 20f;
                maxHealth = 200;
                break;
            case Zumbi.GrandeRapido:
                movementLayer = "Running Layer";
                AIpath.maxSpeed = 4;
                transform.localScale *= 1.3f;
                damage = 16f;
                maxHealth = 160;
                break;
            case Zumbi.Boss:
                movementLayer = "Walking Layer";
                AIpath.maxSpeed = 2;
                transform.localScale *= 1.8f;
                damage = 40f;
                maxHealth = 500;
                break;
            default:
                Debug.LogError("ERROR, nao tem esse tipo de zumbi");
                break;
        }
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    void Update()
    {
        bool isMoving = AIpath != null && AIpath.canMove;
        if (zombieAnim != null)
        {
            zombieAnim.SetBool("IsMoving", isMoving);

            if (isMoving)
            {
                zombieAnim.SetLayerWeight(zombieAnim.GetLayerIndex(movementLayer), 1);
                zombieAnim.SetLayerWeight(zombieAnim.GetLayerIndex("Combat Layer"), 0);
            }
        }

        if (target != null)
        {
            if (Vector3.Distance(target.transform.position, transform.position) > 1.8f)
            {
                if (AIpath != null)
                {
                    AIpath.canMove = true;
                }
                target = null;
                if (zombieAnim != null)
                {
                    zombieAnim.SetLayerWeight(zombieAnim.GetLayerIndex(movementLayer), 1);
                    zombieAnim.SetLayerWeight(zombieAnim.GetLayerIndex("Combat Layer"), 0);
                }
            }
            timer += Time.deltaTime;
            if (timer > hitRate)
            {
                StartCoroutine(ZombieDamage());
                timer = 0;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            target = collision.gameObject;
            if (AIpath != null)
            {
                AIpath.canMove = false;
            }
            if (zombieAnim != null)
            {
                zombieAnim.SetTrigger("RightAttackTrigger");
                zombieAnim.SetLayerWeight(zombieAnim.GetLayerIndex(movementLayer), 0);
                zombieAnim.SetLayerWeight(zombieAnim.GetLayerIndex("Combat Layer"), 1);
            }
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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} tomou {damage} de dano. Vida restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        UpdateHealthBar();
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float healthPercentage = (float)currentHealth / maxHealth;
            healthBar.fillAmount = healthPercentage;
        }
    }
}
