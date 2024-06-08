using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int pistolDamage = 20;
    public int machineGunDamage = 10;
    public int shotgunDamage = 50;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            ZombieHealth zombieHealth = other.GetComponent<ZombieHealth>();
            if (zombieHealth != null)
            {
                if (gameObject.CompareTag("PistolProjectile"))
                {
                    zombieHealth.TakeDamage(pistolDamage);
                }
                else if (gameObject.CompareTag("MachineGunProjectile"))
                {
                    zombieHealth.TakeDamage(machineGunDamage);
                }
                else if (gameObject.CompareTag("ShotgunProjectile"))
                {
                    zombieHealth.TakeDamage(shotgunDamage);
                }

                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("bola"))
        {
            Destroy(gameObject);
        }
    }
}