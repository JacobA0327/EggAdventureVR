using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EggManager : MonoBehaviour
{
    public static EggManager instance;

    public Text eggText;
    private int eggCount = 0;

    private void Awake()
    {
        // Singleton
        if (instance == null) instance = this;
    }

    public void AddEgg()
    {
        eggCount++;
        eggText.text = "Huevos: " + eggCount;
        Debug.Log(" Total de huevos: " + eggCount);
    }
}
