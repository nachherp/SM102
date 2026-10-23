using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(Rigidbody2D))]
public class JugadorCarretera : MonoBehaviour
{
    public float velocidad = 4f;
    public float fuerzaImpulso = 6f;
    public float duracionImpulso = 0.2f;
    public float esperaEntreImpulsos = 0.8f;

    public ControlCruce juego;

    private Rigidbody2D cuerpo;
    private Vector2 direccion;
    private Vector2 ultimaDireccion = Vector2.up;

    private bool solicitarImpulso;
    private float tiempoImpulso;
    private float tiempoEspera;

    void Awake()
    {
        cuerpo = GetComponent<Rigidbody2D>();

        cuerpo.bodyType = RigidbodyType2D.Dynamic;
        cuerpo.gravityScale = 0f;
        cuerpo.linearDamping = 0f;

        cuerpo.constraints =
            RigidbodyConstraints2D.FreezeRotation;

        cuerpo.collisionDetectionMode =
            CollisionDetectionMode2D.Continuous;

        cuerpo.interpolation =
            RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        direccion = Vector2.zero;

        if (juego.terminado)
            return;

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

            if (teclado.spaceKey.wasPressedThisFrame)
                solicitarImpulso = true;
        }
#else
        direccion = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        if (Input.GetKeyDown(KeyCode.Space))
            solicitarImpulso = true;
#endif

        direccion = Vector2.ClampMagnitude(direccion, 1f);

        if (direccion.sqrMagnitude > 0f)
            ultimaDireccion = direccion.normalized;
    }

    void FixedUpdate()
    {
        if (juego.terminado)
            return;

        tiempoEspera = Mathf.Max(
            0f,
            tiempoEspera - Time.fixedDeltaTime
        );

        if (solicitarImpulso &&
            tiempoEspera <= 0f &&
            tiempoImpulso <= 0f)
        {
            cuerpo.linearVelocity = direccion * velocidad;

            cuerpo.AddForce(
                ultimaDireccion * fuerzaImpulso,
                ForceMode2D.Impulse
            );

            tiempoImpulso = duracionImpulso;
            tiempoEspera = esperaEntreImpulsos;
        }

        solicitarImpulso = false;

        // Durante el impulso dejamos actuar a la física.
        if (tiempoImpulso > 0f)
        {
            tiempoImpulso -= Time.fixedDeltaTime;
            return;
        }

        // Después recuperamos el movimiento normal.
        cuerpo.linearVelocity = direccion * velocidad;
    }

    void OnCollisionEnter2D(Collision2D choque)
    {
        if (juego.terminado)
            return;

        if (choque.gameObject.CompareTag("Coche"))
        {
            juego.Terminar(false);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (juego.terminado)
            return;

        if (otro.CompareTag("Meta"))
            juego.Terminar(true);
    }
}