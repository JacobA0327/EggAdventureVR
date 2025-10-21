using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectEgg : MonoBehaviour
{
    [Header("Efectos")]
    public ParticleSystem pickupEffect;
    public AudioClip pickupSound;
    public float destroyDelay = 1f;

    private bool collected = false;

    [Header("Orden de huevos (asigna en Inspector)")]
    public List<CollectEgg> eggsOrder;

    [Header("Jugador (para teletransporte)")]
    public Transform player;
    public float teleportOffsetY = 1.6f;
    public float teleportDistance = 3f; // Distancia lateral al huevo siguiente

    private void Start()
    {
        // Solo el primer huevo será visible al iniciar
        if (eggsOrder != null && eggsOrder.Count > 0 && this != eggsOrder[0])
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected || !other.CompareTag("Player")) return;
        collected = true;

        // 🔹 Contador de huevos
        if (EggCounter.instance != null)
            EggCounter.instance.SumarHuevo();

        // 🔹 Efecto visual
        if (pickupEffect != null)
        {
            pickupEffect.transform.parent = null;
            pickupEffect.Play();
            Destroy(pickupEffect.gameObject, pickupEffect.main.duration);
        }

        // 🔹 Sonido
        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        // 🔹 Teletransporte y control de orden
        HandleTeleportAndOrder();

        // 🔹 Destruye el huevo después de terminar las acciones
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay + 0.2f);
        if (this != null && gameObject != null)
            Destroy(gameObject);
    }

    private void HandleTeleportAndOrder()
    {
        if (eggsOrder == null || eggsOrder.Count == 0) return;

        int nextIndex = eggsOrder.IndexOf(this) + 1;

        // 🔹 Comprueba que el siguiente huevo existe antes de acceder
        if (nextIndex < eggsOrder.Count && eggsOrder[nextIndex] != null)
        {
            CollectEgg nextEgg = eggsOrder[nextIndex];
            nextEgg.gameObject.SetActive(true);

            // 🔹 Desactiva temporalmente el collider del siguiente huevo
            Collider eggCollider = nextEgg.GetComponent<Collider>();
            if (eggCollider != null)
                StartCoroutine(EnableColliderAfterDelay(eggCollider, 1.5f)); // espera 1.5 seg

            // 🔹 Calcula una posición lateral al huevo siguiente
            if (player != null)
            {
                Vector3 eggPos = nextEgg.transform.position;
                Vector3 offset = nextEgg.transform.right * teleportDistance; // mueve a un lado del huevo
                Vector3 destino = eggPos + offset;
                destino.y += teleportOffsetY;

                // 🔹 Teletransporta al jugador de forma segura
                CharacterController cc = player.GetComponent<CharacterController>();
                if (cc != null) cc.enabled = false;
                player.position = destino;
                if (cc != null) cc.enabled = true;
            }
        }
        else
        {
            Debug.Log("¡Has recogido el último huevo!");
        }
    }

    // 🔹 Espera antes de reactivar el siguiente huevo para evitar recogerlo de inmediato
    private IEnumerator EnableColliderAfterDelay(Collider col, float delay)
    {
        if (col == null) yield break;
        col.enabled = false;
        yield return new WaitForSeconds(delay);
        if (col != null)
            col.enabled = true;
    }
}
