using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportSystem : MonoBehaviour
{
    public static TeleportSystem Instance { get; private set; }

    [Header("Jugador o cámara VR")]
    public Transform player; // Asignar el objeto Player (no la cámara)
    [Header("Altura fija del jugador")]
    public float fixedHeight = 1.6f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void Teletransportar(Vector3 destino)
    {
        if (player == null) return;

        // Mantiene la altura definida (no depende del terreno)
        destino.y = fixedHeight;

        // Si tiene CharacterController, desactívalo temporalmente para evitar bloqueos
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.position = destino;

        if (cc != null) cc.enabled = true;
    }
}
