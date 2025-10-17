    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    public class ReticlePointer : MonoBehaviour
    {
        public float maxDistance = 10f; // Distancia máxima del raycast
        public LayerMask interactableLayer; // Capa de objetos interactuables
        void Update()
        {
            RaycastHit hit;
            // Realiza un raycast desde la cámara hacia adelante
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance, interactableLayer))
            {
                // Si se detecta un objeto, imprime un mensaje en la consola
                Debug.Log("Objeto detectado por el Reticle Pointer: " + hit.collider.gameObject.name);
            }
            // Si no hay hit, no se hace nada
        }
    }