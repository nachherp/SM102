using UnityEngine;

public class PerderJuego : MonoBehaviour
{
    public GameObject pelota;

    private bool perdiste = false;

    void Awake()
    {
        Time.timeScale = 1f;
    }

    void OnCollisionEnter2D(Collision2D choque)
    {
        if (perdiste || choque.gameObject != pelota)
            return;

        perdiste = true;
        Destroy(pelota);
        Time.timeScale = 0f;
    }

    void OnGUI()
    {
        if (!perdiste)
            return;

        GUIStyle estilo = new GUIStyle(GUI.skin.label);
        estilo.fontSize = 50;
        estilo.alignment = TextAnchor.MiddleCenter;
        estilo.normal.textColor = Color.red;

        GUI.Label(
            new Rect(0, 0, Screen.width, Screen.height),
            "PERDISTE",
            estilo
        );
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
    }
}