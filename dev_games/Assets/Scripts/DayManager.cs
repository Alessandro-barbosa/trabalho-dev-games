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
    public GameObject nuvem1;
    public GameObject nuvem2;
    public GameObject nuvem3;
    public GameObject nuvem4;
    public GameObject lua;
    private MeshRenderer n1;
    private MeshRenderer n2;
    private MeshRenderer n3;
    private MeshRenderer n4;
    private MeshRenderer luaa;

    // Start is called before the first frame update
    void Start()
    {
        render = skybox.GetComponent<Renderer>();
        render.material.EnableKeyword("_NORMALMAP");
        render.material.EnableKeyword("_METALLICGLOSSMAP");
        direcional = luz.GetComponent<Light>();
        n1 = nuvem1.GetComponent<MeshRenderer>();
        n2 = nuvem2.GetComponent<MeshRenderer>();
        n3 = nuvem3.GetComponent<MeshRenderer>();
        n4 = nuvem4.GetComponent<MeshRenderer>();
        luaa = lua.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (playerHere && Input.GetKeyDown(KeyCode.E)) //Checa se o player está no local e apertou E
        {
            render.material.mainTexture = noite;
            direcional.enabled = false;
            n1.enabled = false;
            n2.enabled = false;
            n3.enabled = false;
            n4.enabled = false;
            luaa.enabled = true;

            ligarluz();
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

    void ligarluz()
    {
        GameObject[] luzes = GameObject.FindGameObjectsWithTag("luz");
        foreach (GameObject vela in luzes)
        {
            Light li = vela.GetComponent<Light>();
            if (li != null) // Verifica se o objeto tem um componente Light
            {
                li.enabled = true;
            }
            ParticleSystem ps = vela.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var emission = ps.emission;
                emission.enabled = true;
            }
        }
    }
}
