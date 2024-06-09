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
    private AimController player; // Referência ao controlador do jogador
    public float damage; // Dano causado pelo zumbi
    private float hitRate = 1f; // Taxa de acerto do zumbi
    private float timer = 0; // Temporizador para ataques
    private string movementLayer; // Nome da camada de movimento
    public Image healthBar; // Barra de vida do zumbi

    // Enumerador para os tipos de zumbis
    public enum Zumbi
    {
        Normal,
        Rapido,
        Grande,
        GrandeRapido,
        Boss
    }

    public int maxHealth; // Vida máxima do zumbi
    private int currentHealth; // Vida atual do zumbi

    // Awake é chamado quando o script é inicializado
    void Awake()
    {
        zombieAnim = GetComponent<Animator>(); // Obtém o componente Animator
        AIpath = GetComponent<AIPath>(); // Obtém o componente AIPath
        destinationSet = GetComponent<AIDestinationSetter>(); // Obtém o componente AIDestinationSetter
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<AimController>(); // Obtém o AimController do jogador

        // Define o alvo do zumbi como o jogador
        if (destinationSet != null && player != null)
        {
            destinationSet.target = player.transform;
        }
    }

    // Inicializa o zumbi com base no tipo
    public void NewStart(Zumbi tipo)
    {
        // Verifica se o componente AIPath está presente
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
        currentHealth = maxHealth; // Define a vida atual como a vida máxima
        UpdateHealthBar(); // Atualiza a barra de vida
    }

    // Update é chamado uma vez por frame
    void Update()
    {
        // Verifica se o zumbi pode se mover
        bool isMoving = AIpath != null && AIpath.canMove;
        if (zombieAnim != null)
        {
            zombieAnim.SetBool("IsMoving", isMoving); // Atualiza a animação de movimento

            if (isMoving)
            {
                // Define a camada de animação de movimento e desativa a camada de combate
                zombieAnim.SetLayerWeight(zombieAnim.GetLayerIndex(movementLayer), 1);
                zombieAnim.SetLayerWeight(zombieAnim.GetLayerIndex("Combat Layer"), 0);
            }
        }

        if (target != null)
        {
            // Verifica a distância do alvo e permite o movimento se estiver longe
            if (Vector3.Distance(target.transform.position, transform.position) > 1.8f)
            {
                if (AIpath != null)
                {
                    AIpath.canMove = true;
                }
                target = null;
                if (zombieAnim != null)
                {
                    // Define a camada de animação de movimento e desativa a camada de combate
                    zombieAnim.SetLayerWeight(zombieAnim.GetLayerIndex(movementLayer), 1);
                    zombieAnim.SetLayerWeight(zombieAnim.GetLayerIndex("Combat Layer"), 0);
                }
            }
            timer += Time.deltaTime;
            // Verifica se o tempo decorrido é maior que a taxa de acerto para causar dano
            if (timer > hitRate)
            {
                StartCoroutine(ZombieDamage());
                timer = 0;
            }
        }
    }

    // Detecta colisões com o jogador
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            target = collision.gameObject; // Define o alvo como o jogador
            if (AIpath != null)
            {
                AIpath.canMove = false; // Para o movimento do zumbi
            }
            if (zombieAnim != null)
            {
                // Inicia a animação de ataque
                zombieAnim.SetTrigger("RightAttackTrigger");
                // Desativa a camada de movimento e ativa a camada de combate
                zombieAnim.SetLayerWeight(zombieAnim.GetLayerIndex(movementLayer), 0);
                zombieAnim.SetLayerWeight(zombieAnim.GetLayerIndex("Combat Layer"), 1);
            }
            StartCoroutine(ZombieDamage()); // Inicia a coroutine para causar dano
        }
    }

    // Coroutine para causar dano ao jogador
    IEnumerator ZombieDamage()
    {
        yield return new WaitForSeconds(0);
        if (target != null)
        {
            player.getHitZombie(damage); // Aplica dano ao jogador
        }
    }

    // Método para o zumbi receber dano
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

    // Método para destruir o zumbi
    void Die()
    {
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