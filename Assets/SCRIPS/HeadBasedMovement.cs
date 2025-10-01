using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class HeadBasedMovement : MonoBehaviour
{
    public Transform vrCamera; // Arrastra aqu� la c�mara principal del XR Rig
    public float speed = 2.0f;
    public float deadZone = 10f; // Zona muerta en grados para no moverse si mira recto

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 move = Vector3.zero;

        // Obtener rotaci�n de la c�mara
        float pitch = vrCamera.eulerAngles.x; // inclinaci�n vertical
        float yaw = vrCamera.eulerAngles.y;   // rotaci�n horizontal

        // Ajustar pitch a rango -180 / 180
        if (pitch > 180) pitch -= 360;

        // Adelante / Atr�s
        if (pitch > deadZone) // mira hacia abajo
            move += -vrCamera.forward;
        else if (pitch < -deadZone) // mira hacia arriba
            move += vrCamera.forward;

        // Derecha / Izquierda (usamos el yaw)
        float roll = vrCamera.eulerAngles.z;
        if (roll > 180) roll -= 360;

        if (roll > deadZone) // inclina cabeza a la derecha
            move += vrCamera.right;
        else if (roll < -deadZone) // inclina cabeza a la izquierda
            move += -vrCamera.right;

        // Normalizar y aplicar velocidad
        if (move.magnitude > 0.1f)
        {
            move.Normalize();
            controller.Move(move * speed * Time.deltaTime);
        }
    }
}
