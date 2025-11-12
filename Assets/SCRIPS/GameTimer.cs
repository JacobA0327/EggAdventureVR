using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    [Header("Configuración del tiempo")]
    public float tiempoLimite = 60f; //  1 minuto por defecto
    private float tiempoRestante;

    [Header("Referencias UI")]
    public TextMeshProUGUI textoTiempo;
    public GameObject panelVictoria;
    public GameObject panelDerrota;

    [Header("Configuración del juego")]
    public int totalHuevos = 5; // Total de huevos que debe recoger
    private bool juegoTerminado = false;

    private void Start()
    {
        tiempoRestante = tiempoLimite;
        if (panelVictoria != null) panelVictoria.SetActive(false);
        if (panelDerrota != null) panelDerrota.SetActive(false);
        StartCoroutine(ContarTiempo());
    }

    private IEnumerator ContarTiempo()
    {
        while (tiempoRestante > 0 && !juegoTerminado)
        {
            tiempoRestante -= Time.deltaTime;

            // 🔹 Actualiza el texto visual del tiempo
            if (textoTiempo != null)
            {
                int segundos = Mathf.CeilToInt(tiempoRestante);
                textoTiempo.text = $"Tiempo: {segundos}s";
            }

            yield return null;
        }

        // 🔹 Si el tiempo se acaba y el jugador no ha ganado
        if (!juegoTerminado)
        {
            Derrota();
        }
    }

    // 🔹 Llamado por EggCounter cuando recoge el último huevo
    public void RevisarVictoria(int huevosActuales)
    {
        if (juegoTerminado) return;

        if (huevosActuales >= totalHuevos)
        {
            Victoria();
        }
    }

    private void Victoria()
    {
        juegoTerminado = true;
        Debug.Log("¡Victoria! Has recogido todos los huevos a tiempo.");
        if (panelVictoria != null) panelVictoria.SetActive(true);
        DetenerTiempo();
    }

    private void Derrota()
    {
        juegoTerminado = true;
        Debug.Log(" Se acabó el tiempo. Derrota.");
        if (panelDerrota != null) panelDerrota.SetActive(true);
        DetenerTiempo();
    }

    private void DetenerTiempo()
    {
        Time.timeScale = 0f; // Pausa el juego
    }

    // 🔹 Reiniciar la escena (opcional)
    public void ReiniciarJuego()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
