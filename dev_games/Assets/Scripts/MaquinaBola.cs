using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaquinaBola : MonoBehaviour
{
    public GameObject button;
    public GameObject luz;
    public GameObject bola;
    public GameObject bolaLocal;
    private MeshRenderer led;


    private bool playerHere = false;
    private bool maquinaON = false;


    void Start()
    {
        led = luz.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (playerHere && Input.GetKeyDown(KeyCode.E)) //Checa se o player está no local e apertou E
        {
            funcional();
        }
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

    void funcional()
    {
        if (maquinaON == false)
        {
            button.transform.Translate(0, 0, 0.043f);
            maquinaON = true;
            led.enabled = false;

            InvokeRepeating("spawnaBola", 0, 1.0f);
        }
        else if (maquinaON == true)
        {
            button.transform.Translate(0, 0, -0.043f);
            maquinaON = false;
            led.enabled = true;

            CancelInvoke();
        }
    }

    void spawnaBola()
    {
        GameObject novaBola = Instantiate(bola, bolaLocal.transform.position, bolaLocal.transform.rotation);
        Rigidbody rb = novaBola.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Vector3.up * 15.0f, ForceMode.Impulse);
            rb.AddForce(Vector3.forward * 5.0f, ForceMode.Impulse);
            rb.AddForce(Vector3.left * 5.5f, ForceMode.Impulse);
            rb.AddTorque(randomTorque(), randomTorque(), randomTorque());
        }
    }

    float randomTorque()
    {
        return Random.Range(-10, 10);
    }
}
