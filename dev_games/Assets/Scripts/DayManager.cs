using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayManager : MonoBehaviour
{
    // Variáveis públicas para texturas do céu e luzes
    public GameObject skybox; // Objeto do céu para mudar texturas
    public Texture2D dia; // Textura do dia
    public Texture2D noite; // Textura da noite
    private bool playerHere = false; // Para verificar se o jogador está na área de gatilho
    Renderer render; // Renderer para o skybox
    public GameObject luz; // Luz direcional principal
    private Light direcional; // Componente de luz da luz direcional principal
    public GameObject nuvem1; // Objeto de nuvem 1
    public GameObject nuvem2; // Objeto de nuvem 2
    public GameObject nuvem3; // Objeto de nuvem 3
    public GameObject nuvem4; // Objeto de nuvem 4
    public GameObject lua; // Objeto da lua
    private MeshRenderer n1; // MeshRenderer para a nuvem 1
    private MeshRenderer n2; // MeshRenderer para a nuvem 2
    private MeshRenderer n3; // MeshRenderer para a nuvem 3
    private MeshRenderer n4; // MeshRenderer para a nuvem 4
    private MeshRenderer luaa; // MeshRenderer para a lua
    private float timer = 0;
    private float timerText = 0;
    private float counter = 0;
    private CanvaTextDanger canvaTextDanger;
    private bool timerTextNight = false;
    private HortaManager horta;


    public SpawnManager spawner; // Referência ao script SpawnManager

    public int dayCounter; // Contador para o número de dias
    bool isDay = true; // Booleano para verificar se é dia

    // Start é chamado antes da primeira atualização do frame
    void Start()
    {
        // Inicializa referências aos componentes e define estados iniciais
        horta = GameObject.FindGameObjectWithTag("Horta").GetComponent<HortaManager>();
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
        canvaTextDanger = FindObjectOfType<CanvaTextDanger>();
        dayCounter = 0;
    }

    // Update é chamado uma vez por frame
    void Update()
    {
        // Se o jogador está na área de gatilho, pressiona 'E', e é dia, muda para a noite
        if (playerHere && Input.GetKeyDown(KeyCode.E) && isDay)
        {
            SetNoite(); // Muda para noite
            dayCounter++; // Incrementa o contador de dias
            spawner.StartWave(dayCounter); // Inicia uma nova onda de inimigos
            isDay = false; // Define isDay para falso
            StartCoroutine(CheckForZombies()); // Inicia a verificação de zumbis restantes
        }
        if(isDay)
            timer += Time.deltaTime;
        else
            timer = 0;
        waveTimer();
        if(timerTextNight)
        {
            timerText += Time.deltaTime;
            if(timerText >= 5)
            {
                canvaTextDanger.textDangerTimer();
                timerTextNight = false;
                timerText = 0;
            }
        }
    }

    void applyDay()
    {
        if (isDay)
        {
            horta.SecarAgua();
            isDay = false; // Define isDay para falso
            SetNoite(); // Muda para noite
            dayCounter++; // Incrementa o contador de dias
            spawner.StartWave(dayCounter); // Inicia uma nova onda de inimigos
            canvaTextDanger.textDangerTimer();
            StartCoroutine(CheckForZombies()); // Inicia a verificação de zumbis restantes
            canvaTextDanger.textDanger.text = ($"Cuidado noite {counter += 1} iniciando").ToString();
            timerTextNight = true;
        }
    }
    // Método para definir o cenário para o dia
    void SetDia()
    {
        render.material.mainTexture = dia; // Muda a textura do céu para o dia
        direcional.enabled = true; // Ativa a luz direcional
        SetNuvens(true); // Ativa as nuvens
        luaa.enabled = false; // Desativa a lua
        ApagarLuz(); // Desliga luzes adicionais
        isDay = true; // Define isDay para verdadeiro
    }

    // Método para definir o cenário para a noite
    void SetNoite()
    {
        render.material.mainTexture = noite; // Muda a textura do céu para a noite
        direcional.enabled = false; // Desativa a luz direcional
        SetNuvens(false); // Desativa as nuvens
        luaa.enabled = true; // Ativa a lua
        ligarluz(); // Liga luzes adicionais
    }

    // Método para ativar ou desativar nuvens
    void SetNuvens(bool estado)
    {
        n1.enabled = estado; // Define nuvem 1
        n2.enabled = estado; // Define nuvem 2
        n3.enabled = estado; // Define nuvem 3
        n4.enabled = estado; // Define nuvem 4
    }

    // Evento de gatilho quando o jogador entra na área
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica se o colisor pertence ao jogador
        {
            playerHere = true; // Define playerHere para verdadeiro
        }
    }

    // Evento de gatilho quando o jogador sai da área
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica se o colisor pertence ao jogador
        {
            playerHere = false; // Define playerHere para falso
        }
    }

    // Método para ligar luzes adicionais à noite
    void ligarluz()
    {
        GameObject[] luzes = GameObject.FindGameObjectsWithTag("luz");
        foreach (GameObject vela in luzes)
        {
            Light li = vela.GetComponent<Light>();
            if (li != null)
            {
                li.enabled = true; // Ativa o componente de luz
            }
            ParticleSystem ps = vela.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var emission = ps.emission;
                emission.enabled = true; // Ativa a emissão de partículas
            }
        }
    }

    // Método para desligar luzes adicionais durante o dia
    void ApagarLuz()
    {
        GameObject[] luzes = GameObject.FindGameObjectsWithTag("luz");
        foreach (GameObject vela in luzes)
        {
            Light li = vela.GetComponent<Light>();
            if (li != null)
            {
                li.enabled = false; // Desativa o componente de luz
            }
            ParticleSystem ps = vela.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var emission = ps.emission;
                emission.enabled = false; // Desativa a emissão de partículas
            }
        }
    }

    // Coroutine para verificar zumbis restantes a cada 5 segundos
    IEnumerator CheckForZombies()
    {
        while (!isDay) // Executa enquanto for noite
        {
            yield return new WaitForSeconds(5f); // Espera por 5 segundos
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0) // Verifica se não há mais zumbis
            {
                SetDia(); // Muda para dia
            }
        }
    }
    public void waveTimer()
    {
        if(timer >= 5)
        {
            applyDay();            
        }
    }
}