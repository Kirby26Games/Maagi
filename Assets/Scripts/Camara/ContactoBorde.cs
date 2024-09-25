using UnityEngine;

public class ContactoBorde : MonoBehaviour
{
    public Vector3 MovimientoDeCamara;
    public Vector3 ModMovimientoDePersonaje;
    [SerializeField]
    private Vector3 _ModBorde; // Siempre un vector con un -1 o 1 y 0 en el resto de posiciones
    private float _ModGlobal;
    [Header("Ubicación Habitaciones")]
    private Vector3 _TamañoHabitacion = new(22, 11, 0);      // Tamaño en 3 dimensiones de la habitación mínima
    private float _EspacioEntreHabitaciones = 1;             // Espacio después de los bordes de una habitación
    private float _MargenCambioHabitacion = 2;               // Margen de maniobra para el teletransporte del personaje


    private void Start()
    {
        // La cámara se desplaza en x igual al tamaño de una habitación + 2 veces el espacio vacío después de los bordes
        _ModGlobal = (_TamañoHabitacion.x + 2 * _EspacioEntreHabitaciones);
        // Relativo al centro del prefab de la habitación, si se debe mover y en qué dirección multiplicado por esta cantidad
        MovimientoDeCamara.x = transform.parent.position.x + _ModBorde.x * _ModGlobal;
        // La cámara se desplaza en y igual al tamaño de una habitación + 2 veces el espacio vacío después de los bordes
        _ModGlobal = (_TamañoHabitacion.y + 2 * _EspacioEntreHabitaciones);
        // Relativo al centro del prefab de la habitación, si se debe mover y en qué dirección multiplicado por esta cantidad
        MovimientoDeCamara.y = transform.parent.position.y + _ModBorde.y * _ModGlobal;

        // El personaje se debe los a través de los 2 bordes + 2 espacio vacío después del borde + un margen de maniobra
        _ModGlobal = 4 * _EspacioEntreHabitaciones + _MargenCambioHabitacion;
        // Depende del borde, decide si se debe mover y en que dirección
        ModMovimientoDePersonaje.x = _ModBorde.x * _ModGlobal;
        ModMovimientoDePersonaje.y = _ModBorde.y * _ModGlobal;
    }
}
