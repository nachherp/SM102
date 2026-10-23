using UnityEngine;

public class MovimientoNube : MonoBehaviour
{
    public float velocidad = 1f;
    public float limite = 10f;

    void Update()
    {
        transform.Translate(
            Vector3.right * velocidad * Time.deltaTime,
            Space.World
        );

        if (transform.position.x > limite)
        {
            transform.position = new Vector3(
                -limite,
                transform.position.y,
                transform.position.z
            );
        }
    }
}