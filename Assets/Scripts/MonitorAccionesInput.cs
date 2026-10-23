using UnityEngine;
using UnityEngine.InputSystem;

// Comprobador de entradas: el objeto permanece quieto, sin mecánica de movimiento.
public class MonitorAccionesInput : MonoBehaviour
{
    public Vector2 Movimiento { get; private set; }
    public int Saltos { get; private set; }
    public int CambiosColor { get; private set; }
    private SpriteRenderer sprite;
    private float ultimoSalto = -10f;
    private GUIStyle titulo, texto, boton;

    void Awake() { sprite = GetComponent<SpriteRenderer>(); }

    public void OnMove(InputValue value) { Movimiento = value.Get<Vector2>(); }

    public void OnJump(InputValue value)
    {
        if (!value.isPressed) return;
        Saltos++;
        ultimoSalto = Time.unscaledTime;
    }

    public void OnChangeColor(InputValue value)
    {
        if (!value.isPressed) return;
        CambiosColor++;
        sprite.color = CambiosColor % 2 == 1 ? Color.red : Color.cyan;
    }

    void OnGUI()
    {
        if (titulo == null)
        {
            titulo = new GUIStyle(GUI.skin.label) { fontSize = 34, fontStyle = FontStyle.Bold, wordWrap = true };
            texto = new GUIStyle(GUI.skin.label) { fontSize = 23, wordWrap = true };
            boton = new GUIStyle(GUI.skin.box) { fontSize = 25, alignment = TextAnchor.MiddleCenter };
        }
        var previous = GUI.matrix;
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / 1600f, Screen.height / 900f, 1));
        GUI.Label(new Rect(65, 35, 1470, 60), "EJERCICIO 1 · COMPROBAR LAS ACCIONES", titulo);
        GUI.Label(new Rect(65, 100, 1470, 65), "PlayerControls  /  mapa Player  /  PlayerInput: Send Messages", texto);
        GUI.Box(new Rect(840, 190, 690, 615), "");
        GUI.Label(new Rect(875, 215, 610, 50), "Move · Value / Vector2", titulo);
        GUI.Label(new Rect(875, 280, 610, 45), "Valor recibido: " + Movimiento.ToString("F2"), texto);
        Tecla(new Rect(1115, 345, 100, 65), "W / ↑", Movimiento.y > 0);
        Tecla(new Rect(995, 425, 100, 65), "A / ←", Movimiento.x < 0);
        Tecla(new Rect(1115, 425, 100, 65), "S / ↓", Movimiento.y < 0);
        Tecla(new Rect(1235, 425, 100, 65), "D / →", Movimiento.x > 0);
        Tecla(new Rect(875, 540, 610, 75), "Jump · Espacio    |    Pulsaciones: " + Saltos, Time.unscaledTime - ultimoSalto < .35f);
        GUI.Label(new Rect(875, 645, 610, 95), "ChangeColor · C\nCambios de color: " + CambiosColor, texto);
        GUI.Label(new Rect(65, 640, 700, 140), "El cuadrado permanece quieto.\nPrueba las teclas y observa el panel.\nC alterna el color; Espacio enciende Jump.", texto);
        GUI.Label(new Rect(65, 820, 1470, 55), "Haz clic dentro de Game para dar foco al teclado. No necesitas mover el objeto en este ejercicio.", texto);
        GUI.matrix = previous;
    }

    void Tecla(Rect rect, string label, bool active)
    {
        Color previous = GUI.backgroundColor;
        GUI.backgroundColor = active ? new Color(.15f, 1, .65f) : new Color(.45f, .55f, .65f);
        GUI.Box(rect, label, boton);
        GUI.backgroundColor = previous;
    }
}
