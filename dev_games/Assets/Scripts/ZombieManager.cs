using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ZombieManager : MonoBehaviour
{
    private GameObject target; // Alvo do zumbi (geralmente o jogador)
    private Animator zombieAnim; // Componente Animator do zumbi
    private AIPath AIpath; // Componente AIPath para controle do movimento do zumbi
    private AIDestinationSetter destinationSet; // Componente para definir o destino do zumbi
    private AimController player; // Refer�ncia ao controlador do jogador
    public float damage; // Dano causado pelo zumbi
    private string movementLayer; // Nome da camada de movimento
    public Image healthBar; // Barra de vida do zumbi
    private bool hasAttacked = false;
    private float timer = 0;
    private HortaManager horta;


    // Enumerador para os tipos de zumbis
    public enum Zumbi
    {
        Normal,
        Rapido,
        Grande,
        GrandeRapido,
        Boss
    }

    public int maxHealth; // Vida m�xima do zumbi
    private int currentHealth; // Vida atual do zumbi

    // Awake � chamado quando o script � inicializado
    void Awake()
    {
        zombieAnim = GetComponent<Animator>(); // Obt�m o componente Animator
        AIpath = GetComponent<AIPath>(); // Obt�m o componente AIPath
        destinationSet = GetComponent<AIDestinationSetter>(); // Obt�m o componente AIDestinationSetter
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<AimController>(); // Obt�m o AimController do jogador
        horta = GameObject.FindGameObjectWithTag("Horta").GetComponent<HortaManager>();
        
        // Define o alvo do zumbi como o jogador
        if (destinationSet != null && player != null)
        {
            destinationSet.target = player.transform;
        }
    }

    // Inicializa o zumbi com base no tipo
    public void NewStart(Zumbi tipo)
    {
        // Verifica se o componente AIPath est� presente
        if (AIpath == null)
        {
            Debug.LogError("AIPath component is not found on the zombie!");
            return;
        }

        // Define as propriedades do zumbi com base no tipo
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
        currentHealth = maxHealth; // Define a vida atual como a vida m�xima
        UpdateHealthBar(); // Atualiza a barra de vida
    }

    // Update � chamado uma vez por frame
    void Update()
    {
        if (zombieAnim != null)
        {
            bool isMoving = AIpath != null && AIpath.canMove;
            zombieAnim.SetBool("IsMoving", isMoving);
            UpdateAnimationLayers(isMoving);
        }

        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
            if (distanceToTarget > 1.8f)
            {
                ResetTargetAndMovement();
            }
            else if (!hasAttacked)
            {
                StartCoroutine(ZombieDamage());
                hasAttacked = true;
            }
        }
        timer += Time.deltaTime;
        if (timer > 1)
        {
            AstarPath.active.UpdateGraphs(this.GetComponent<Collider>().bounds);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colidiu com player");
            target = collision.gameObject;
            if (AIpath != null) AIpath.canMove = false;
            TriggerAttackAnimation();
        }
    }

    IEnumerator ZombieDamage()
    {
        Debug.Log("Iniciando ataque ao player...");
        yield return new WaitForSeconds(zombieAnim.GetCurrentAnimatorStateInfo(0).length / 2);
        
        if (target != null && Vector3.Distance(target.transform.position, transform.position) <= 1.8f)
        {
            Debug.Log("Causando " + damage + " de dano no player!");
            player.getHitZombie(damage);
        }
        else
        {
            Debug.Log("Player fora de alcance após a animação de ataque.");
            ResetTargetAndMovement();
        }
        hasAttacked = false;
    }

    private void UpdateAnimationLayers(bool isMoving)
    {
        int movementLayerIndex = zombieAnim.GetLayerIndex("Walking Layer");
        int combatLayerIndex = zombieAnim.GetLayerIndex("Combat Layer");
        
        if (movementLayerIndex != -1 && combatLayerIndex != -1)
        {
            zombieAnim.SetLayerWeight(movementLayerIndex, isMoving ? 1 : 0);
            zombieAnim.SetLayerWeight(combatLayerIndex, isMoving ? 0 : 1);
        }
        else
        {
            Debug.LogWarning("Invalid layer index detected.");
        }
    }

    private void ResetTargetAndMovement()
    {
        Debug.Log("Resetando target e movimento...");
        if (AIpath != null) AIpath.canMove = true;
        target = null;
        hasAttacked = false;
        UpdateAnimationLayers(true);
    }

    private void TriggerAttackAnimation()
    {
        Debug.Log("Iniciando animação de ataque...");
        int movementLayerIndex = zombieAnim.GetLayerIndex("Walking Layer");
        int combatLayerIndex = zombieAnim.GetLayerIndex("Combat Layer");
        
        if (movementLayerIndex != -1 && combatLayerIndex != -1)
        {
            zombieAnim.SetTrigger("RightAttackTrigger");
            zombieAnim.SetLayerWeight(movementLayerIndex, 0);
            zombieAnim.SetLayerWeight(combatLayerIndex, 1);
        }
        else
        {
            Debug.LogWarning("Invalid layer index detected.");
        }
    }
    // M�todo para o zumbi receber dano
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduz a vida atual do zumbi
        Debug.Log($"{gameObject.name} tomou {damage} de dano. Vida restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die(); // Mata o zumbi se a vida for menor ou igual a 0
        }
        UpdateHealthBar(); // Atualiza a barra de vida
    }

    // M�todo para destruir o zumbi
    void Die()
    {
        horta.contadorBuff += 10f;
        Destroy(gameObject);
    }

    // Atualiza a barra de vida do zumbi
    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float healthPercentage = (float)currentHealth / maxHealth;
            healthBar.fillAmount = healthPercentage;
        }
    }
}