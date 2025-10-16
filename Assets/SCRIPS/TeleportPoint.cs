using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour
{
    public Transform destino;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (TeleportSystem.Instance != null)
            {
                TeleportSystem.Instance.Teletransportar(destino.position);
            }
        }
    }
}
