using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject spawn;

    private int spawnInterval = 2;
    private int spawnRange = 20;
    private int numZombies = 1;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnZombies());
    }

    IEnumerator SpawnZombies()
    {
        int i = 0;
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            Spawn();
            i++;
            Debug.Log("Passou aqui " + i + " vezes");
        }
    }

    void Spawn()
    {
        for(int i = 0; i < numZombies; i++)
        {
            Vector3 spawnVector = new Vector3 (transform.position.x + Random.Range(-spawnRange, spawnRange), transform.position.y + Random.Range(-spawnRange, spawnRange), transform.position.z);
            
            Instantiate(spawn, transform.position, Quaternion.identity);
        }
    }
}
