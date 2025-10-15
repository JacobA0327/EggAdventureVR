using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBasedMovement : MonoBehaviour
{
    [Header("Head Settings")]
    public Transform head; // Cámara o CardboardReticlePointer
    public float moveSpeed = 2f;
    public float headTiltThreshold = 10f;

    [Header("Altura fija")]
    public float fixedHeight = 1.6f; // altura constante del jugador sobre el suelo

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (head == null || controller == null)
            return;

        // Mantener altura constante
        Vector3 position = transform.position;
        position.y = fixedHeight;
        transform.position = position;

        // Detectar inclinación vertical (mirar hacia arriba/abajo)
        float tilt = head.eulerAngles.x;
        if (tilt > 180) tilt -= 360; // convierte rango 0-360 en -180 a 180

        Vector3 move = Vector3.zero;

        // Si el jugador mira hacia arriba, avanza
        if (tilt < -headTiltThreshold)
        {
            move = new Vector3(head.forward.x, 0, head.forward.z);
            move.Normalize();
            move *= moveSpeed;
        }

        // Movimiento horizontal (sin gravedad, sin cambio de altura)
        controller.Move(move * Time.deltaTime);
    }
}
