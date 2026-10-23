using UnityEngine;

public class ControlCruce : MonoBehaviour
{
    public bool terminado = false;

    private bool gano = false;

    void Awake()
    {
        terminado = false;
        Time.timeScale = 1f;
    }

    public void Terminar(bool victoria)
    {
        if (terminado)
            return;

        terminado = true;
        gano = victoria;
        Time.timeScale = 0f;
    }

    void OnGUI()
    {
        GUI.Label(
            new Rect(15, 15, 500, 30),
            "WASD o flechas: llega a la franja verde."
        );

        if (!terminado)
            return;

        GUIStyle estilo = new GUIStyle(GUI.skin.label);
        estilo.fontSize = 50;
        estilo.alignment = TextAnchor.MiddleCenter;
        estilo.normal.textColor =
            gano ? Color.green : Color.red;

        GUI.Label(
            new Rect(0, 0, Screen.width, Screen.height),
            gano ? "GANASTE" : "PERDISTE",
            estilo
        );
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
    }
}