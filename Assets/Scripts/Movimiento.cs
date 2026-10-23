using UnityEngine;
using UnityEngine.InputSystem; // <-- Requerido para el nuevo Input System

public class MovimientoNave : MonoBehaviour // <-- Mismo nombre que el archivo MovimientoNave.cs
{
    public float velocidad = 5f;

    void Update()
    {
        if (Keyboard.current == null) return;

        Vector3 direccion = Vector3.zero;

        if (Keyboard.current.leftArrowKey.isPressed)  direccion += Vector3.left;
        if (Keyboard.current.rightArrowKey.isPressed) direccion += Vector3.right;
        if (Keyboard.current.upArrowKey.isPressed)    direccion += Vector3.up;
        if (Keyboard.current.downArrowKey.isPressed)  direccion += Vector3.down;

        transform.Translate(direccion.normalized * (velocidad * Time.deltaTime));
    }
}