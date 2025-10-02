using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EggManager : MonoBehaviour
{
    // Instancia global
    public static EggManager instance;

    // Texto en pantalla
    public Text eggText;

    // Contador de huevos
    private int eggCount = 0;

    private void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Método público para añadir un huevo
    public void AddEgg()
    {
        eggCount++;
        UpdateEggText();
    }

    // Actualizar UI
    private void UpdateEggText()
    {
        if (eggText != null)
        {
            eggText.text = "Huevos: " + eggCount;
        }
    }
}
