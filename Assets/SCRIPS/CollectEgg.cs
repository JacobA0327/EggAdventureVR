using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectEgg : MonoBehaviour
{
    public ParticleSystem pickupEffect;  // Efecto de partículas
    public AudioClip pickupSound;        // (Opcional) Sonido al recoger
    public float destroyDelay = 1f;      // Tiempo antes de desaparecer el huevo

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el que toca es el jugador
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            // 🔹 Llamar al contador de huevos
            if (EggCounter.instance != null)
            {
                EggCounter.instance.SumarHuevo();
            }

            // Mostrar efecto de partículas
            if (pickupEffect != null)
            {
                pickupEffect.transform.parent = null; // separa del huevo
                pickupEffect.Play();
                Destroy(pickupEffect.gameObject, pickupEffect.main.duration);
            }

            // Reproducir sonido si tiene
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // Destruir el huevo
            Destroy(gameObject, destroyDelay + 0.1f);

        }
    }
}
