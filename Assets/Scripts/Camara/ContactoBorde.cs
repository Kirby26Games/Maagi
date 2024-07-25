using UnityEngine;

public class ContactoBorde : MonoBehaviour
{
    public Vector3 MovimientoDeCamara;
    public Vector3 ModMovimientoDePersonaje;
    [SerializeField]
    private Vector3 _ModBorde; // Siempre un vector con un -1 o 1 y 0 en el resto de posiciones
    private float _ModGlobal;

    private void Start()
    {
        // La cámara se desplaza en x igual al tamaño de una habitación + 2 veces el espacio vacío después de los bordes
        _ModGlobal = (VariablesGlobales.Instancia.TamañoHabitacion.x + 2 * VariablesGlobales.Instancia.EspacioEntreHabitaciones);
        // Relativo al centro del prefab de la habitación, si se debe mover y en qué dirección multiplicado por esta cantidad
        MovimientoDeCamara.x = transform.parent.position.x + _ModBorde.x * _ModGlobal;
        // La cámara se desplaza en y igual al tamaño de una habitación + 2 veces el espacio vacío después de los bordes
        _ModGlobal = (VariablesGlobales.Instancia.TamañoHabitacion.y + 2 * VariablesGlobales.Instancia.EspacioEntreHabitaciones);
        // Relativo al centro del prefab de la habitación, si se debe mover y en qué dirección multiplicado por esta cantidad
        MovimientoDeCamara.y = transform.parent.position.y + _ModBorde.y * _ModGlobal;

        // El personaje se debe los a través de los 2 bordes + 2 espacio vacío después del borde + un margen de maniobra
        _ModGlobal = 4 * VariablesGlobales.Instancia.EspacioEntreHabitaciones + VariablesGlobales.Instancia.MargenCambioHabitacion;
        // Depende del borde, decide si se debe mover y en que dirección
        ModMovimientoDePersonaje.x = _ModBorde.x * _ModGlobal;
        ModMovimientoDePersonaje.y = _ModBorde.y * _ModGlobal;
    }
}
