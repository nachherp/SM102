using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(Rigidbody2D))]
public class MovimientoSeguro : MonoBehaviour
{
    public float velocidad = 5f;

    private Rigidbody2D cuerpo;
    private Vector2 direccion;
    private ContactFilter2D filtro;
    private RaycastHit2D[] golpes = new RaycastHit2D[16];

    void Awake()
    {
        cuerpo = GetComponent<Rigidbody2D>();

        // La pelota no puede empujar la nave.
        cuerpo.bodyType = RigidbodyType2D.Kinematic;
        cuerpo.gravityScale = 0f;
        cuerpo.constraints = RigidbodyConstraints2D.FreezeRotation;
        cuerpo.interpolation = RigidbodyInterpolation2D.Interpolate;

        filtro.SetLayerMask(
            Physics2D.GetLayerCollisionMask(gameObject.layer)
        );
        filtro.useTriggers = false;
    }

    void Update()
    {
        direccion = Vector2.zero;

#if ENABLE_INPUT_SYSTEM
        var teclado = Keyboard.current;

        if (teclado != null)
        {
            if (teclado.aKey.isPressed ||
                teclado.leftArrowKey.isPressed)
                direccion.x -= 1f;

            if (teclado.dKey.isPressed ||
                teclado.rightArrowKey.isPressed)
                direccion.x += 1f;

            if (teclado.wKey.isPressed ||
                teclado.upArrowKey.isPressed)
                direccion.y += 1f;

            if (teclado.sKey.isPressed ||
                teclado.downArrowKey.isPressed)
                direccion.y -= 1f;
        }
#else
        direccion = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );
#endif

        direccion = Vector2.ClampMagnitude(direccion, 1f);
    }

    void FixedUpdate()
    {
        Vector2 movimiento =
            direccion * velocidad * Time.fixedDeltaTime;

        float distancia = movimiento.magnitude;

        if (distancia == 0f)
            return;

        Vector2 sentido = movimiento.normalized;

        // Comprueba si hay una pared en el camino.
        int cantidad = cuerpo.Cast(
            sentido, filtro, golpes, distancia + 0.02f
        );

        for (int i = 0; i < cantidad; i++)
        {
            Rigidbody2D otro = golpes[i].rigidbody;

            // La pelota sigue interactuando con la nave,
            // pero no bloquea sus controles.
            if (otro != null &&
                otro.bodyType == RigidbodyType2D.Dynamic)
                continue;

            // Permite alejarse de una pared que ya toca.
            if (Vector2.Dot(sentido, golpes[i].normal) >= 0f)
                continue;

            distancia = Mathf.Min(
                distancia,
                Mathf.Max(0f, golpes[i].distance - 0.02f)
            );
        }

        cuerpo.MovePosition(
            cuerpo.position + sentido * distancia
        );
    }
}