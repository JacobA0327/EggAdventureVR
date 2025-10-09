using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EggCounter : MonoBehaviour
{
    public static EggCounter instance;   // Para acceder desde otros scripts fácilmente
    public TextMeshProUGUI textoHuevos;  // Texto en pantalla que muestra los huevos
    private int contadorHuevos = 0;      // Total de huevos recogidos

    void Awake()
    {
        // Asegura que solo haya un contador activo
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        ActualizarTexto();
    }

    // Llamado cada vez que se recoge un huevo
    public void SumarHuevo()
    {
        contadorHuevos++;
        ActualizarTexto();
    }

    // Actualiza el texto en pantalla
    private void ActualizarTexto()
    {
        if (textoHuevos != null)
            textoHuevos.text = "Huevos: " + contadorHuevos;
    }
}
