using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectEgg : MonoBehaviour
{
    public ParticleSystem collectParticles; // Asigna aqu� tu sistema de part�culas

    private void OnTriggerEnter(Collider other)
    {
        // Si el objeto que entra es el Player
        if (other.CompareTag("Player"))
        {
            // Sumar huevo al manager
            EggManager.instance.AddEgg();

            // Activar part�culas (si existen)
            if (collectParticles != null)
            {
                // Desvinculamos el sistema de part�culas para que no se destruya con el huevo
                collectParticles.transform.parent = null;
                collectParticles.Play();

                // Destruir part�culas despu�s de que terminen
                Destroy(collectParticles.gameObject, collectParticles.main.duration);
            }

            // Destruir el huevo
            Destroy(gameObject);
        }
    }
}
