using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<GameObject> spawners; // Lista de pontos de spawn
    public List<GameObject> zombies; // Prefabs dos zumbis
    private int spawnInterval = 1; // Intervalo de tempo entre spawns de zumbis
    private int baseZombiesPerWave = 10; // Número base de zumbis por onda
    private int waveNumber = 1; // Número da onda inicial

    // Inicia uma nova onda de spawns de zumbis
    public void StartWave(int wave)
    {
        waveNumber = wave; // Define o número da onda atual
        StartCoroutine(SpawnZombies()); // Inicia a coroutine de spawn de zumbis
    }

    // Coroutine para spawnar zumbis na quantidade da onda atual
    IEnumerator SpawnZombies()
    {
        // Calcula o número de zumbis para a onda atual, adicionando 5 zumbis por onda
        int numZombies = baseZombiesPerWave + (waveNumber - 1) * 5;
        int spawnedZombies = 0; // Contador de zumbis spawnados

        // Loop para spawnar zumbis até alcançar o número desejado
        while (spawnedZombies < numZombies)
        {
            yield return new WaitForSeconds(spawnInterval); // Espera pelo intervalo de spawn
            StartCoroutine(SpawnAndInitializeZombie()); // Spawna e inicializa um zumbi
            spawnedZombies++; // Incrementa o contador de zumbis spawnados
        }
    }

    // Coroutine para spawnar e inicializar um zumbi
    IEnumerator SpawnAndInitializeZombie()
    {
        // Verifica se há spawners e prefabs de zumbis configurados
        if (spawners.Count == 0 || zombies.Count == 0)
        {
            Debug.LogWarning("Spawners ou Zombies não configurados corretamente."); // Log de aviso se não configurados
            yield break; // Sai da coroutine se não configurados
        }

        // Seleciona aleatoriamente um ponto de spawn
        int localNascer = Random.Range(0, spawners.Count);
        // Seleciona aleatoriamente um prefab de zumbi
        GameObject zombiePrefab = SelectZombiePrefab();

        // Define a posição de spawn do zumbi
        Vector3 spawnVector = spawners[localNascer].transform.position;
        // Instancia o zumbi na posição de spawn
        GameObject zombieInstance = Instantiate(zombiePrefab, spawnVector, Quaternion.identity);
        zombieInstance.SetActive(false); // Desativa o zumbi inicialmente

        yield return new WaitForSeconds(1); // Espera 1 segundo antes de inicializar

        // Inicializa o zumbi se o componente ZombieManager estiver presente
        ZombieManager zombieManager = zombieInstance.GetComponent<ZombieManager>();
        if (zombieManager != null)
        {
            ZombieManager.Zumbi tipoZumbi = SelectZombieType(); // Seleciona o tipo de zumbi
            zombieManager.NewStart(tipoZumbi); // Inicializa o zumbi com o tipo selecionado
        }

        zombieInstance.SetActive(true); // Ativa o zumbi
    }

    // Seleciona aleatoriamente um prefab de zumbi da lista
    GameObject SelectZombiePrefab()
    {
        int index = Random.Range(0, zombies.Count);
        return zombies[index];
    }

    // Seleciona o tipo de zumbi com base na onda atual e uma chance aleatória
    ZombieManager.Zumbi SelectZombieType()
    {
        int chance = Random.Range(0, 100);
        if (waveNumber > 4 && chance < 2)
        {
            return ZombieManager.Zumbi.Boss; // 2% de chance de spawnar um Boss se a onda for maior que 4
        }
        if (waveNumber > 3 && chance < 5)
        {
            return ZombieManager.Zumbi.GrandeRapido; // 5% de chance de spawnar um GrandeRapido se a onda for maior que 3
        }
        if (waveNumber > 2 && chance < 10)
        {
            return ZombieManager.Zumbi.Grande; // 10% de chance de spawnar um Grande se a onda for maior que 2
        }
        if (waveNumber > 1 && chance < 20)
        {
            return ZombieManager.Zumbi.Rapido; // 20% de chance de spawnar um Rapido se a onda for maior que 1
        }
        return ZombieManager.Zumbi.Normal; // Normal caso contrário
    }
}