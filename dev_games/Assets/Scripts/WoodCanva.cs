using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WoodCanva : MonoBehaviour
{
    private TMP_Text woodCounterText;
    private int woodCount = 0;

    private void Start()
    {
        woodCounterText = GetComponentInChildren<TMP_Text>();
    }
    public void AddLogWood(int value)
    {
        woodCount += value;
        UpdateWoodLogCounter();
    }
    private void UpdateWoodLogCounter()
    {
        if (woodCounterText != null)
        {
            woodCounterText.text = woodCount.ToString();
        }
    }
}
