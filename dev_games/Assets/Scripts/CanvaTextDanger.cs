using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CanvaTextDanger : MonoBehaviour
{
    public TMP_Text textDanger;
    void Start()
    {
        textDanger = GetComponentInChildren<TMP_Text>();
    }
    public void textDangerTimer()
    {
        textDanger.enabled = !textDanger.enabled;
    }
}
