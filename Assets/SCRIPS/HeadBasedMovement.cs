using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class HeadBasedMovement : MonoBehaviour
{
    public Transform vrCamera; // Cámara VR
    public float speed = 2.0f;
    public float deadZone = 10f;
    public float gravity = -9.81f;

    [Header("Altura Fija del Jugador")]
    public float targetHeight = 1.5f; // Altura media deseada
    public float heightSmooth = 5f;   // Suavidad del ajuste de altura

    private CharacterController controller;
    private float verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 move = Vector3.zero;

        float pitch = vrCamera.eulerAngles.x;
        float yaw = vrCamera.eulerAngles.y;

        if (pitch > 180) pitch -= 360;

        // Movimiento hacia adelante / atrás
        if (pitch > deadZone)
            move += -GetFlatForward();
        else if (pitch < -deadZone)
            move += GetFlatForward();

        float roll = vrCamera.eulerAngles.z;
        if (roll > 180) roll -= 360;

        // Movimiento lateral
        if (roll > deadZone)
            move += GetFlatRight();
        else if (roll < -deadZone)
            move += -GetFlatRight();

        if (move.magnitude > 0.1f)
        {
            move.Normalize();
            move *= speed;
        }

        // --- Control de altura constante ---
        Vector3 currentPos = transform.position;
        float desiredY = GetGroundHeight() + targetHeight; // altura del suelo + altura fija
        float smoothedY = Mathf.Lerp(currentPos.y, desiredY, Time.deltaTime * heightSmooth);
        currentPos.y = smoothedY;
        transform.position = currentPos;

        // --- Aplicar movimiento (solo XZ) ---
        move.y = 0;
        controller.Move(move * Time.deltaTime);
    }

    // Detecta el suelo debajo del jugador
    float GetGroundHeight()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 2f, Vector3.down, out hit, 5f))
        {
            return hit.point.y;
        }
        return transform.position.y; // si no detecta suelo
    }

    Vector3 GetFlatForward()
    {
        Vector3 fwd = vrCamera.forward;
        fwd.y = 0f;
        return fwd.normalized;
    }

    Vector3 GetFlatRight()
    {
        Vector3 right = vrCamera.right;
        right.y = 0f;
        return right.normalized;
    }
}
