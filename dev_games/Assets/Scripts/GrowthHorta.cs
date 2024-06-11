using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthHorta : MonoBehaviour
{
    private AimController player;

    private const float TEMPO_CHANCE = 5f; // Intervalo de tempo para verificar crescimento
    private const float TAMANHO_CRESCIMENTO = 0.05f; // Incremento de tamanho ao crescer
    private const int CHANCE_CRESCER = 10; // Chance percentual de crescimento

    public float tempoCrescimento;
    public int estagioDeCrescimento = 0;

    // Enumerador para os estágios de crescimento
    private enum Estagio
    {
        Inicial,
        Intermediario,
        Final
    }

    // Start é chamado antes do primeiro frame
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<AimController>();
        HortaReset();
    }

    // Update é chamado uma vez por frame
    void Update()
    {
        if (estagioDeCrescimento < (int)Estagio.Final)
        {
            tempoCrescimento += Time.deltaTime;
            if (tempoCrescimento > TEMPO_CHANCE)
            {
                tempoCrescimento -= TEMPO_CHANCE;
                TentarCrescer();
            }
        }
        // Verifica se a tecla 'E' foi pressionada.
        if (Input.GetKeyDown(KeyCode.E))
        {
            VerificarRaycast();
        }
    }

    // Tenta fazer a planta crescer com base em uma chance aleatória
    private void TentarCrescer()
    {
        if (CHANCE_CRESCER >= Random.Range(1, 100))
        {
            estagioDeCrescimento++;
            AtualizarCrescimento();
        }
    }

    // Atualiza o estado de crescimento da planta
    private void AtualizarCrescimento()
    {
        if (estagioDeCrescimento == (int)Estagio.Intermediario)
        {
            CrescimentoIntermediario();
        }
        else if (estagioDeCrescimento == (int)Estagio.Final)
        {
            CrescimentoFinal();
        }
    }

    // Define o crescimento intermediário da planta
    private void CrescimentoIntermediario()
    {
        if (CompareTag("Alface") || CompareTag("Tomate"))
        {
            AjustarCrescimento(0.25f, 20 * TAMANHO_CRESCIMENTO);
        }
        else
        {
            AjustarCrescimento(0.5f, 19 * TAMANHO_CRESCIMENTO);
        }
    }

    // Define o crescimento final da planta
    private void CrescimentoFinal()
    {
        if (CompareTag("Alface"))
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if (CompareTag("Tomate"))
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            AjustarCrescimento(1.5f, TAMANHO_CRESCIMENTO);
        }
    }

    // Ajusta o crescimento da planta
    private void AjustarCrescimento(float novaEscala, float incrementoAltura)
    {
        transform.localScale = new Vector3(novaEscala, novaEscala, novaEscala);
        transform.position = new Vector3(transform.position.x, transform.position.y + incrementoAltura, transform.position.z);
    }

    //// Método chamado quando o jogador clica na planta
    //private void OnMouseDown()
    //{
    //    if (estagioDeCrescimento == (int)Estagio.Final)
    //    {
    //        HortaReset();
    //        player.EatFood(10);
    //    }
    //}

    // Reseta a planta para o estado inicial
    public void HortaReset()
    {
        estagioDeCrescimento = 0;
        transform.position = new Vector3(transform.position.x, transform.position.y - 20 * TAMANHO_CRESCIMENTO, transform.position.z);
    }
    //Verifica seo jogador está olhando para a planta
    private void VerificarRaycast()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 10))
        {
            if (hit.transform == this.transform && estagioDeCrescimento == (int)Estagio.Final)
            {
                HortaReset();
                player.EatFood(10);
                Debug.Log("comida comida");
            }
        }
    }
}