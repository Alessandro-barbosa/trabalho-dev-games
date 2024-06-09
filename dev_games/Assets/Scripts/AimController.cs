using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class AimController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Rig aimRig;
    [SerializeField] private float playerLife;
    [SerializeField] private MeshRenderer playerGun;
    [SerializeField] private MeshRenderer playerGunTambor;
    [SerializeField] private MeshRenderer playerAxe;
    private float timer = 0;
    private float hitRate = 1f;
    private AxeManager axe;


    private Transform a;
    private bool axeOnHand;
    private bool armOnHand = true;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;

    private Transform objetoAtingido;
    private GameObject enemyObject;
    private BulletTarget enemyTarget;

    public float ClipLenght = 1f;
    public AudioClip audioClip;
    public AudioSource SourceaudioClip;
    public HealthManager healthManager;


    private float buttonPressTime = 0;
    private float delayTime = 0.3f;
    private void Start()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        playerLife = 100;
        playerAxe.enabled = false;
        axe = GameObject.FindGameObjectWithTag("axe").GetComponent<AxeManager>();
        axe.disableRigidBodyAxe();
    }
    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        Transform hitTransform = null;

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
            enemyTarget = raycastHit.collider.gameObject.GetComponent<BulletTarget>();
        }

        if (starterAssetsInputs.getAxe)
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            animator.SetLayerWeight(1, 0);

            animator.SetLayerWeight(2, 0);

            starterAssetsInputs.shoot = false;

            aimRig.weight = 0f;
            buttonPressTime = 0;

            armOnHand = false;
            if(playerGun.enabled)
                toggleMeshRenderer();
            
        }
        if (!armOnHand)
        {
            
            int layerIndex = animator.GetLayerIndex("AxeHit");
            if (starterAssetsInputs.shoot)
            {
                
                animator.SetLayerWeight(layerIndex, 0.8f);
                animator.SetTrigger("axeHit");
                axe.disableRigidBodyAxe();

                timer += Time.deltaTime;
                if (timer > hitRate)
                {
                    StartCoroutine(axeCounter());
                    axe.disableRigidBodyAxe();
                    timer = 0;
                }
                
            }
        }
        
        if (starterAssetsInputs.getArm) {
            armOnHand = true;
            if (!playerGun.enabled)
                toggleMeshRenderer();
        }
        if (armOnHand)
        {
            int layerIndex = animator.GetLayerIndex("AxeHit");
            animator.SetLayerWeight(layerIndex, 0f);
            // Player is aiming
            if (starterAssetsInputs.aim)
            {
                armOnHand = true;
                // Configurações de câmera
                aimVirtualCamera.gameObject.SetActive(true);
                thirdPersonController.SetSensitivity(aimSensitivity);
                thirdPersonController.SetRotateOnMove(false);

                // Mudando animação do player segurando a arma
                animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

                Vector3 worldAimTarget = mouseWorldPosition;
                worldAimTarget.y = transform.position.y;
                Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

                aimRig.weight = 0.6f; //Acionando o layer Rig
                animator.SetLayerWeight(2, 0);

                buttonPressTime += Time.deltaTime;

                // Atirando e delay do tiro
                if (starterAssetsInputs.shoot && buttonPressTime > delayTime)
                {
                    animator.SetLayerWeight(2, 1);
                    animator.SetTrigger("Shooting_T");

                    if (hitTransform != null)
                    { 
                        // Acertando alguma coisa 
                        if (hitTransform.GetComponent<BulletTarget>() != null)
                        {

                            a = Instantiate(vfxHitGreen, debugTransform.position, Quaternion.identity); // efeitin da bala no zumbi
                            enemyTarget.bulletHit();
                        }
                        else
                        {
                            a = Instantiate(vfxHitRed, debugTransform.position, Quaternion.identity); // efeitin da bala no resto
                        }
                    }
                    SourceaudioClip.PlayOneShot(audioClip);
                    if (a != null)
                    {
                        Destroy(a.gameObject, 1f);
                    }
                    //Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
                    //Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                    starterAssetsInputs.shoot = false;
                }
                else
                {
                    animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 20f));
                }

            }
            else{
                aimVirtualCamera.gameObject.SetActive(false);
                thirdPersonController.SetSensitivity(normalSensitivity);
                thirdPersonController.SetRotateOnMove(true);
                animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));

                aimRig.weight = 0f;
                buttonPressTime = 0;
            }
        }

        //if (starterAssetsInputs.getAxe){
        //    animator.SetBool("Axe_B", true);
        //}
    }
    public void getHitZombie(float damage)
    {
        healthManager.TakeDamage(damage);
        playerLife -= damage;
        if(playerLife <= 0)
        {
            PauseGame();
        }
    }
    public void EatFood(float lifeGain)
    {
        playerLife += lifeGain;
        healthManager.Heal(lifeGain);
    }
    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void toggleMeshRenderer()
    {
        playerGun.enabled = !playerGun.enabled;
        playerGunTambor.enabled = !playerGunTambor.enabled;
        playerAxe.enabled = !playerAxe.enabled;
    }

    IEnumerator axeCounter()
    {
        yield return new WaitForSeconds(0);
    }
}