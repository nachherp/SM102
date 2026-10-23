using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CocheCarretera : MonoBehaviour
{
    public float velocidad = 4f;
    public float limite = 11f;

    private Rigidbody2D cuerpo;

    void Awake()
    {
        cuerpo = GetComponent<Rigidbody2D>();

        cuerpo.bodyType = RigidbodyType2D.Kinematic;
        cuerpo.gravityScale = 0f;
        cuerpo.constraints = RigidbodyConstraints2D.FreezeRotation;
        cuerpo.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void FixedUpdate()
    {
        Vector2 siguiente = cuerpo.position;

        siguiente.x += velocidad * Time.fixedDeltaTime;

        if (velocidad > 0f && siguiente.x > limite)
        {
            cuerpo.position =
                new Vector2(-limite, siguiente.y);

            return;
        }

        if (velocidad < 0f && siguiente.x < -limite)
        {
            cuerpo.position =
                new Vector2(limite, siguiente.y);

            return;
        }

        cuerpo.MovePosition(siguiente);
    }
}