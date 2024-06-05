using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private bool playerHere = false;
    private string tagToReplace = "CercaUp";
    public GameObject upgrade1;
    public GameObject upgrade2;
    public GameObject upgrade3;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerHere && Input.GetKeyDown(KeyCode.E))
        {
            trocarObjetos();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerHere = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerHere = false;
        }
    }

    void trocarObjetos()
    {
        GameObject[] cercaParaSerTrocado = GameObject.FindGameObjectsWithTag(tagToReplace);
        foreach (GameObject cercaAntiga in cercaParaSerTrocado)
        {
            string nomeCerca = cercaAntiga.name;
            Vector3 position = cercaAntiga.transform.position;
            Quaternion rotation = cercaAntiga.transform.rotation;

            GameObject newObject = null;

            switch (nomeCerca)
            {
                case "cerca0":
                    newObject = upgrade1;
                    break;

                case "cerca1":
                    newObject = upgrade2;
                    break;

                case "cerca2":
                    newObject = upgrade3;
                    break;

                case "cerca3":
                    newObject = upgrade3;
                    break;
            }

            if (newObject != null)
            {
                Destroy(cercaAntiga);
                GameObject newCerca = Instantiate(newObject, position, rotation);
                newCerca.name = GetNextCercaName(nomeCerca);
            }

        }
    }

    string GetNextCercaName(string currentName)
    {
        switch (currentName)
        {
            case "cerca0":
                return "cerca1";
            case "cerca1":
                return "cerca2";
            case "cerca2":
                return "cerca3";
            case "cerca3":
                return "cerca3";
            default:
                return currentName;
        }
    }
}
