using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocoManager : MonoBehaviour
{
    private HortaManager horta;
    private bool playerInPoco = false;
    
    // Start is called before the first frame update
    void Start()
    {
        horta = GameObject.FindGameObjectWithTag("Horta").GetComponent<HortaManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInPoco && Input.GetKeyDown(KeyCode.E))
        {
            horta.RegarHorta();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInPoco = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInPoco = false;
        }
    }
}
