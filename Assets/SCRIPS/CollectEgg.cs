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
    public float teleportDistance = 3f; // 🔹 Distancia lateral al huevo siguiente
    public float delayBeforeNextEgg = 1.5f; // 🔹 Tiempo para habilitar el siguiente huevo

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

        // 🔹 Contador
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

        // 🔹 Retrasa el teletransporte y la activación del siguiente huevo
        StartCoroutine(HandleTeleportAfterDelay());
    }

    private IEnumerator HandleTeleportAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        HandleTeleportAndOrder();
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

    private void HandleTeleportAndOrder()
    {
        if (eggsOrder == null || eggsOrder.Count == 0) return;

        int nextIndex = eggsOrder.IndexOf(this) + 1;

        if (nextIndex < eggsOrder.Count)
        {
            CollectEgg nextEgg = eggsOrder[nextIndex];
            nextEgg.gameObject.SetActive(true);

            // 🔹 Aseguramos que el collider realmente se active
            StartCoroutine(EnableColliderSafely(nextEgg, delayBeforeNextEgg));

            // 🔹 Teletransporta al jugador a un lado del siguiente huevo
            if (player != null)
            {
                Vector3 eggPos = nextEgg.transform.position;
                Vector3 offset = nextEgg.transform.right * teleportDistance;
                Vector3 destino = eggPos + offset;
                destino.y += teleportOffsetY;

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

    // 🔹 Activa el collider del siguiente huevo con seguridad
    private IEnumerator EnableColliderSafely(CollectEgg egg, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (egg != null)
        {
            Collider col = egg.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false;
                yield return null; // Espera un frame antes de activarlo
                col.enabled = true;
                Debug.Log(" Collider reactivado: " + egg.name);
            }
        }
    }
}
