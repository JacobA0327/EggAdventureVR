using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EggManager : MonoBehaviour
{
    public static EggManager instance;
    public TextMeshProUGUI eggText;
    private int eggCount = 0;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void AddEgg()
    {
        eggCount++;
        eggText.text = "Huevos: " + eggCount;
    }
}
