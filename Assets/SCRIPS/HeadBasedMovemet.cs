using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBasedMovement : MonoBehaviour
{
    [Header("Head Settings")]
    public Transform head; // Cámara o CardboardReticlePointer
    public float moveSpeed = 2f;
    public float headTiltThreshold = 10f;

    [Header("Physics Settings")]
    public float gravity = -9.81f;

    private CharacterController controller;
    private float verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (head == null || controller == null)
            return;

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

        // Aplicar gravedad para mantener altura estable (sin pegarse)
        if (controller.isGrounded)
        {
            verticalVelocity = -1f; // mantiene contacto sin hundirse
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }
}
