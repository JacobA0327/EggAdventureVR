using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EggCounter : MonoBehaviour
{
    public static EggCounter instance; // Para acceder desde otros scripts fácilmente
    public TextMeshProUGUI textoHuevos; // Texto en pantalla que muestra los huevos
    private int huevosRecolectados = 0; // Total de huevos recogidos

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        ActualizarTexto();
    }

    private void ActualizarTexto()
    {
        if (textoHuevos != null)
            textoHuevos.text = "Huevos: " + huevosRecolectados;
    }

    //  Versión correcta que se comunica con el temporizador
    public void SumarHuevo()
    {
        huevosRecolectados++;
        ActualizarTexto();

        // 🔹 Avisar al GameTimer para revisar victoria
        GameTimer timer = FindObjectOfType<GameTimer>();
        if (timer != null)
            timer.RevisarVictoria(huevosRecolectados);
    }

    public int GetTotalHuevos()
    {
        return huevosRecolectados;
    }
}
