using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class AimController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera; // Câmera virtual usada para a mira
    [SerializeField] private float normalSensitivity = 1.0f; // Sensibilidade normal do personagem
    [SerializeField] private float aimSensitivity = 0.5f; // Sensibilidade ao mirar
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask(); // Máscara de camada para detectar colisores ao mirar
    [SerializeField] private Transform debugTransform; // Transform usado para depuração, indicando onde o raio de mira está atingindo
    [SerializeField] private Transform vfxHitGreen; // Efeito visual para acerto em inimigo
    [SerializeField] private Transform vfxHitRed; // Efeito visual para acerto em objeto
    [SerializeField] private Transform spawnBulletPosition; // Posição de spawn do projétil da bala
    [SerializeField] private Transform pfBulletProjectile; // Prefab do projétil da bala
    [SerializeField] private Rig aimRig; // Rig de animação usado para a mira
    [SerializeField] private float playerLife = 100f; // Vida do jogador
    [SerializeField] private MeshRenderer playerGun; // Renderer da arma do jogador
    [SerializeField] private MeshRenderer playerGunTambor; // Renderer do tambor da arma do jogador
    [SerializeField] private MeshRenderer playerAxe; // Renderer do machado do jogador
    [SerializeField] private HealthManager healthManager; // Script para gerenciar a saúde do jogador
    [SerializeField] private AudioClip audioClip; // Som de tiro
    [SerializeField] private AudioSource SourceaudioClip; // Fonte de áudio para tocar o som de tiro
    [SerializeField] private int weaponDamage = 10; // Dano da arma

    private ThirdPersonController thirdPersonController; // Controlador do personagem em terceira pessoa
    private StarterAssetsInputs starterAssetsInputs; // Script para gerenciar entradas do jogador
    private Animator animator; // Componente Animator para animações do personagem
    private AxeManager axe; // Referência ao script AxeManager
    private float timer = 0f; // Temporizador para ataques
    private float hitRate = 1f; // Taxa de acerto para ataques
    private bool isAxeOnHand; // Estado do machado
    private bool isWeaponOnHand = true; // Estado da arma
    private float buttonPressTime = 0f; // Temporizador para pressionar o botão
    private float delayTime = 0.3f; // Tempo de atraso para o botão
    private float delayTimeAxe = 2f;
    private float btnAxePressTime = 0f;

    private GameObject bola; // Referência ao objeto alvo
    private ZombieManager enemyTarget; // Referência ao script ZombieManager do inimigo

    private void Start()
    {
        // Inicializa as referências aos componentes necessários
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        axe = GameObject.FindGameObjectWithTag("axe")?.GetComponent<AxeManager>();

        // Desabilita a física do machado e esconde o machado
        if (axe != null)
        {
            axe.disableRigidBodyAxe();
        }

        playerAxe.enabled = false;
    }

    private void Update()
    {
        HandleAiming(); // Gerencia a lógica de mira
        HandleWeaponSwitch(); // Gerencia a troca de armas
        HandleShooting(); // Gerencia a lógica de tiro
        HandleAxe(); // Gerencia a lógica do machado
    }

    private void HandleAiming()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        // Lança um raio a partir da câmera para detectar o alvo
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            enemyTarget = raycastHit.collider.GetComponent<ZombieManager>();
            bola = raycastHit.collider.gameObject;
        }

        // Se o jogador estiver mirando
        if (starterAssetsInputs.aim && isWeaponOnHand)
        {
            aimVirtualCamera.gameObject.SetActive(true); // Ativa a câmera de mira
            thirdPersonController.SetSensitivity(aimSensitivity); // Ajusta a sensibilidade da mira
            thirdPersonController.SetRotateOnMove(false); // Desabilita a rotação ao mover

            // Ajusta a animação do personagem ao mirar
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

            aimRig.weight = 0.6f; // Ajusta o peso do rig de mira
            buttonPressTime += Time.deltaTime;
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false); // Desativa a câmera de mira
            thirdPersonController.SetSensitivity(normalSensitivity); // Ajusta a sensibilidade normal
            thirdPersonController.SetRotateOnMove(true); // Habilita a rotação ao mover

            // Ajusta a animação do personagem ao não mirar
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
            aimRig.weight = 0f; // Ajusta o peso do rig de mira para zero
            buttonPressTime = 0f;
        }
    }

    private void HandleWeaponSwitch()
    {
        // Se o jogador pegar a arma
        if (starterAssetsInputs.getArm)
        {
            isWeaponOnHand = true;
            if (!playerGun.enabled)
            {
                ToggleMeshRenderer();
            }
        }

        // Se o jogador pegar o machado
        if (starterAssetsInputs.getAxe)
        {
            isWeaponOnHand = false;
            aimVirtualCamera.gameObject.SetActive(false); // Desativa a câmera de mira
            thirdPersonController.SetSensitivity(normalSensitivity); // Ajusta a sensibilidade normal
            thirdPersonController.SetRotateOnMove(true); // Habilita a rotação ao mover
            animator.SetLayerWeight(1, 0); // Ajusta o peso da camada de animação para zero
            aimRig.weight = 0f; // Ajusta o peso do rig de mira para zero
            buttonPressTime = 0f;

            if (playerGun.enabled)
            {
                ToggleMeshRenderer();
            }
        }
    }

    private void HandleShooting()
    {
        // Se o jogador estiver atirando e tiver a arma na mão
        if (starterAssetsInputs.shoot && isWeaponOnHand && buttonPressTime > delayTime)
        {
            animator.SetLayerWeight(2, 1); // Ajusta o peso da camada de animação de tiro
            animator.SetTrigger("Shooting_T"); // Dispara a animação de tiro

            if (enemyTarget != null)
            {
                Transform vfx = Instantiate(vfxHitGreen, debugTransform.position, Quaternion.identity); // Efeito visual de acerto em inimigo
                enemyTarget.TakeDamage(weaponDamage); // Aplica dano ao inimigo
                Destroy(vfx.gameObject, 1f); // Destroi o efeito visual após 1 segundo
            }
            else
            {
                Transform vfx = Instantiate(vfxHitRed, debugTransform.position, Quaternion.identity); // Efeito visual de acerto em objeto
                if (bola != null && bola.GetComponent<bolaBehaviour>() != null)
                {
                    Destroy(bola); // Destroi o objeto
                }
                Destroy(vfx.gameObject, 1f); // Destroi o efeito visual após 1 segundo
            }

            SourceaudioClip.PlayOneShot(audioClip); // Toca o som de tiro
            starterAssetsInputs.shoot = false; // Reseta o estado de tiro
        }
        else
        {
            animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 20f)); // Ajusta o peso da camada de animação de tiro
        }
    }

    private void HandleAxe()
    {
        // Se o jogador estiver atacando com o machado
        if (!isWeaponOnHand && starterAssetsInputs.shoot && btnAxePressTime >= delayTimeAxe)
        {
            int layerIndex = animator.GetLayerIndex("AxeHit");
            animator.SetLayerWeight(layerIndex, 0.8f); // Ajusta o peso da camada de animação de ataque com machado
            animator.SetTrigger("axeHit"); // Dispara a animação de ataque com machado
            axe.disableRigidBodyAxe();
            btnAxePressTime = 0;
            StartCoroutine(AxeHitDelay());

        }
        else
            btnAxePressTime += Time.deltaTime;
    }

    public void getHitZombie(float damage)
    {
        healthManager.TakeDamage(damage); // Aplica dano ao jogador
        playerLife -= damage; // Reduz a vida do jogador
        if (playerLife <= 0)
        {
            PauseGame(); // Pausa o jogo se a vida do jogador chegar a zero
        }
    }

    public void EatFood(float lifeGain)
    {
        playerLife += lifeGain; // Aumenta a vida do jogador
        healthManager.Heal(lifeGain); // Cura o jogador
    }

    private void PauseGame()
    {
        Time.timeScale = 0; // Pausa o tempo do jogo
    }

    private void ToggleMeshRenderer()
    {
        playerGun.enabled = !playerGun.enabled; // Alterna a visibilidade da arma
        playerGunTambor.enabled = !playerGunTambor.enabled; // Alterna a visibilidade do tambor da arma
        playerAxe.enabled = !playerAxe.enabled; // Alterna a visibilidade do machado
    }
  
    private IEnumerator AxeHitDelay()
    {
        yield return new WaitForSeconds(2); // Delay para o ataque com machado (placeholder)
        axe.disableRigidBodyAxe();
    }
}