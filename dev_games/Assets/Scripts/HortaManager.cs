using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HortaManager : MonoBehaviour
{
    public float tempoHorta;
    public float contadorBuff = 0;

    private bool buffado = false;
    // Start is called before the first frame update
    void Start()
    {
        tempoHorta = 60f;
    }


    // Update is called once per frame
    void Update()
    {
        if (contadorBuff > 0)
        {
            if(!buffado)
            {
                buffado = true;
                tempoHorta = 30f;
            }
            contadorBuff -= Time.deltaTime;
        }
        else
        {
            contadorBuff = 0;
            buffado = false;
            tempoHorta = 60f;
        }
    }
}
