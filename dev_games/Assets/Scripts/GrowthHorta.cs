using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthHorta : MonoBehaviour
{
    public float tempoCrescimento;
    private float tempoChance = 5f;
    public int estagioDeCrescimento = 0;
    private float tamanhoCrescimento = 0.05f;
    private int chanceCrescer = 10;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - 20 * tamanhoCrescimento, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(estagioDeCrescimento < 2)
        {
            Debug.Log("Ta funfante");
            tempoCrescimento += Time.deltaTime;
            if (tempoCrescimento > tempoChance)
            {
                tempoCrescimento -= tempoChance;
                if (chanceCrescer >= Random.Range(1, 100))
                {
                    estagioDeCrescimento++;
                    if (estagioDeCrescimento == 1)
                    {
                        
                        if(CompareTag("Alface") || CompareTag("Tomate"))
                        {
                            transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                            transform.position = new Vector3(transform.position.x, transform.position.y + 20 * tamanhoCrescimento, transform.position.z);
                        }
                        else
                        {
                            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                            transform.position = new Vector3(transform.position.x, transform.position.y + 19 * tamanhoCrescimento, transform.position.z);
                        }
                    }
                    else
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
                            transform.position = new Vector3(transform.position.x, transform.position.y + tamanhoCrescimento, transform.position.z);
                            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                        }
                        ;

                    }
                }
            }
        }
    }
}
