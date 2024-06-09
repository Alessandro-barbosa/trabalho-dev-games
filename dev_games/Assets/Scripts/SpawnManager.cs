using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<GameObject> spawners;
    public List<GameObject> zombies; // Prefabs dos zumbis
    private int spawnInterval = 1;
    private int baseZombiesPerWave = 10; // Número base de zumbis por onda
    private int waveNumber = 1; // Onda inicial

    public void StartWave(int wave)
    {
        waveNumber = wave;
        StartCoroutine(SpawnZombies());
    }

    IEnumerator SpawnZombies()
    {
        // Adicionando 5 zumbis por wave:
        int numZombies = baseZombiesPerWave + (waveNumber - 1) * 5;
        int spawnedZombies = 0;

        while (spawnedZombies < numZombies)
        {
            yield return new WaitForSeconds(spawnInterval);
            StartCoroutine(SpawnAndInitializeZombie());
            spawnedZombies++;
        }
    }

    IEnumerator SpawnAndInitializeZombie()
    {
        if (spawners.Count == 0 || zombies.Count == 0)
        {
            Debug.LogWarning("Spawners ou Zombies não configurados corretamente.");
            yield break;
        }

        int localNascer = Random.Range(0, spawners.Count);
        GameObject zombiePrefab = SelectZombiePrefab();

        Vector3 spawnVector = spawners[localNascer].transform.position;
        GameObject zombieInstance = Instantiate(zombiePrefab, spawnVector, Quaternion.identity);
        zombieInstance.SetActive(false);

        yield return new WaitForSeconds(1);

        ZombieManager zombieManager = zombieInstance.GetComponent<ZombieManager>();
        if (zombieManager != null)
        {
            ZombieManager.Zumbi tipoZumbi = SelectZombieType();
            zombieManager.NewStart(tipoZumbi);
        }

        zombieInstance.SetActive(true);
    }

    GameObject SelectZombiePrefab()
    {
        // Seleciona aleatoriamente um prefab de zumbi da lista
        int index = Random.Range(0, zombies.Count);
        return zombies[index];
    }

    ZombieManager.Zumbi SelectZombieType()
    {
        int chance = Random.Range(0, 100);
        if (waveNumber > 4 && chance < 2)
        {
            return ZombieManager.Zumbi.Boss;
        }
        if (waveNumber > 3 && chance < 5)
        {
            return ZombieManager.Zumbi.GrandeRapido;
        }
        if (waveNumber > 2 && chance < 10)
        {
            return ZombieManager.Zumbi.Grande;
        }
        if (waveNumber > 1 && chance < 20)
        {
            return ZombieManager.Zumbi.Rapido;
        }
        return ZombieManager.Zumbi.Normal;
    }
}