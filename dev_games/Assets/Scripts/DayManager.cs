using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public GameObject skybox;
    public Texture2D dia;
    public Texture2D noite;
    private bool playerHere = false;
    Renderer render;
    public GameObject luz;
    private Light direcional;
    // Start is called before the first frame update
    void Start()
    {
        render = skybox.GetComponent<Renderer>();
        render.material.EnableKeyword("_NORMALMAP");
        render.material.EnableKeyword("_METALLICGLOSSMAP");
        direcional = luz.GetComponent<Light>();
    }

    void Update()
    {
        if (playerHere && Input.GetKeyDown(KeyCode.E)) //Checa se o player está no local e apertou E
        {
            render.material.mainTexture = noite;
            direcional.enabled = false;
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
}
