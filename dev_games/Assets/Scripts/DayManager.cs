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

    public SpawnManager spawner;

    public int dayCounter;
    bool isDay = true;

    void Start()
    {
        spawner = GameObject.Find("GameManager").GetComponent<SpawnManager>();
        render = skybox.GetComponent<Renderer>();
        render.material.EnableKeyword("_NORMALMAP");
        render.material.EnableKeyword("_METALLICGLOSSMAP");
        direcional = luz.GetComponent<Light>();
        n1 = nuvem1.GetComponent<MeshRenderer>();
        n2 = nuvem2.GetComponent<MeshRenderer>();
        n3 = nuvem3.GetComponent<MeshRenderer>();
        n4 = nuvem4.GetComponent<MeshRenderer>();
        luaa = lua.GetComponent<MeshRenderer>();
        dayCounter = 0;
    }

    void Update()
    {
        if (playerHere && Input.GetKeyDown(KeyCode.E) && isDay)
        {
            SetNoite();
            dayCounter++;
            spawner.StartWave(dayCounter);
            isDay = false;
            StartCoroutine(CheckForZombies());
        }
    }

    void SetDia()
    {
        render.material.mainTexture = dia;
        direcional.enabled = true;
        SetNuvens(true);
        luaa.enabled = false;
        ApagarLuz();
        isDay = true;
    }

    void SetNoite()
    {
        render.material.mainTexture = noite;
        direcional.enabled = false;
        SetNuvens(false);
        luaa.enabled = true;
        ligarluz();
    }

    void SetNuvens(bool estado)
    {
        n1.enabled = estado;
        n2.enabled = estado;
        n3.enabled = estado;
        n4.enabled = estado;
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

    void ligarluz()
    {
        GameObject[] luzes = GameObject.FindGameObjectsWithTag("luz");
        foreach (GameObject vela in luzes)
        {
            Light li = vela.GetComponent<Light>();
            if (li != null)
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

    void ApagarLuz()
    {
        GameObject[] luzes = GameObject.FindGameObjectsWithTag("luz");
        foreach (GameObject vela in luzes)
        {
            Light li = vela.GetComponent<Light>();
            if (li != null)
            {
                li.enabled = false;
            }
            ParticleSystem ps = vela.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var emission = ps.emission;
                emission.enabled = false;
            }
        }
    }

    IEnumerator CheckForZombies()
    {
        while (!isDay)
        {
            yield return new WaitForSeconds(5f);
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                SetDia();
            }
        }
    }
}