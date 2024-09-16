using UnityEngine;

public class ContactoBorde : MonoBehaviour
{
    public Vector3 MovimientoDeCamara;
    public Vector3 ModMovimientoDePersonaje;
    [SerializeField]
    private Vector3 _ModBorde; // Siempre un vector con un -1 o 1 y 0 en el resto de posiciones
    private float _ModGlobal;
    [Header("Ubicaci�n Habitaciones")]
    private Vector3 _Tama�oHabitacion = new(22, 11, 0);      // Tama�o en 3 dimensiones de la habitaci�n m�nima
    private float _EspacioEntreHabitaciones = 1;             // Espacio despu�s de los bordes de una habitaci�n
    private float _MargenCambioHabitacion = 2;               // Margen de maniobra para el teletransporte del personaje


    private void Start()
    {
        // La c�mara se desplaza en x igual al tama�o de una habitaci�n + 2 veces el espacio vac�o despu�s de los bordes
        _ModGlobal = (_Tama�oHabitacion.x + 2 * _EspacioEntreHabitaciones);
        // Relativo al centro del prefab de la habitaci�n, si se debe mover y en qu� direcci�n multiplicado por esta cantidad
        MovimientoDeCamara.x = transform.parent.position.x + _ModBorde.x * _ModGlobal;
        // La c�mara se desplaza en y igual al tama�o de una habitaci�n + 2 veces el espacio vac�o despu�s de los bordes
        _ModGlobal = (_Tama�oHabitacion.y + 2 * _EspacioEntreHabitaciones);
        // Relativo al centro del prefab de la habitaci�n, si se debe mover y en qu� direcci�n multiplicado por esta cantidad
        MovimientoDeCamara.y = transform.parent.position.y + _ModBorde.y * _ModGlobal;

        // El personaje se debe los a trav�s de los 2 bordes + 2 espacio vac�o despu�s del borde + un margen de maniobra
        _ModGlobal = 4 * _EspacioEntreHabitaciones + _MargenCambioHabitacion;
        // Depende del borde, decide si se debe mover y en que direcci�n
        ModMovimientoDePersonaje.x = _ModBorde.x * _ModGlobal;
        ModMovimientoDePersonaje.y = _ModBorde.y * _ModGlobal;
    }
}
