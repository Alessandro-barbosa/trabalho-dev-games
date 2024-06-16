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
    public int recursos;
    private WoodCanva woodCanva;
    private int counterNow = 0;

    void Start()
    {
        woodCanva = FindObjectOfType<WoodCanva>();
    }

    void Update()
    {
        if (playerHere && Input.GetKeyDown(KeyCode.E)) //Checa se o player está no local e apertou E
        {
            trocarObjetos();
        }
        GameObject[] tocos = GameObject.FindGameObjectsWithTag("toco");
        recursos = tocos.Length - 1;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //Confere que o player está no local e muda variavel
        {
            playerHere = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) //Confere que o player saiu do local e muda variavel
        {
            playerHere = false; 
        }
    }

    void trocarObjetos()
    {
        GameObject[] cercaParaSerTrocado = GameObject.FindGameObjectsWithTag(tagToReplace); //Salva as cercas com a tag para serem trocadas em um vetor
        foreach (GameObject cercaAntiga in cercaParaSerTrocado)
        {
            string nomeCerca = cercaAntiga.name;
            Vector3 position = cercaAntiga.transform.position;
            Quaternion rotation = cercaAntiga.transform.rotation;

            GameObject newObject = null;

            switch (nomeCerca) //Checa o nivel de upgrade
            {
                case "cerca0":
                    if (woodCanva.woodCount >= 2)
                    {
                        newObject = upgrade1;
                        counterNow = 2;
                    }
                    break;

                case "cerca1":
                    if (woodCanva.woodCount >= 5)
                    {
                        newObject = upgrade2;
                        counterNow = 5;
                    }
                    break;

                case "cerca2":
                    if (woodCanva.woodCount >= 10)
                    {
                        newObject = upgrade3;
                        counterNow = 10;
                    }
                    break;

                case "cerca3":
                    newObject = upgrade3;
                    break;
            }

            if (newObject != null) //Destroi a cerca antiga e coloca a nova
            {
                Destroy(cercaAntiga);
                GameObject newCerca = Instantiate(newObject, position, rotation);
                newCerca.name = GetNextCercaName(nomeCerca);
            }
        }

        switch (counterNow) {
            case (2):
                {
                    woodCanva.minusLogWood(2);
                    break;
                }
            case (5):
                {
                    woodCanva.minusLogWood(5);
                    break;
                }
            case (10):
                {
                    woodCanva.minusLogWood(10);
                    break;
                }
        }
    }

    string GetNextCercaName(string currentName) //Pega o nome do proximo upgrade
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
