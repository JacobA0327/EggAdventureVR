using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectEgg : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Player debe tener este Tag
        {
            EggManager.instance.AddEgg();
            Destroy(gameObject); // Desaparece el huevo
        }
    }
}

