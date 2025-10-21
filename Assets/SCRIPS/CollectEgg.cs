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

        // 🔹 Teletransporte y control de orden
        HandleTeleportAndOrder();

        Destroy(gameObject, destroyDelay + 0.1f);
    }

    private void HandleTeleportAndOrder()
    {
        if (eggsOrder == null || eggsOrder.Count == 0) return;

        int nextIndex = eggsOrder.IndexOf(this) + 1;

        if (nextIndex < eggsOrder.Count)
        {
            CollectEgg nextEgg = eggsOrder[nextIndex];
            nextEgg.gameObject.SetActive(true);

            // 🔹 Desactiva temporalmente el collider del siguiente huevo
            Collider eggCollider = nextEgg.GetComponent<Collider>();
            if (eggCollider != null)
                StartCoroutine(EnableColliderAfterDelay(eggCollider, 1f));

            // 🔹 Calcula una posición lateral al huevo siguiente
            if (player != null)
            {
                Vector3 eggPos = nextEgg.transform.position;
                Vector3 offset = nextEgg.transform.right * teleportDistance; // mueve a un lado del huevo
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

    // 🔹 Espera 1 segundo antes de activar el siguiente collider
    private IEnumerator EnableColliderAfterDelay(Collider col, float delay)
    {
        col.enabled = false;
        yield return new WaitForSeconds(delay);
        col.enabled = true;
    }
}