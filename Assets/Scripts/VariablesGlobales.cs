using UnityEngine;
using UnityEngine.Rendering;

public class VariablesGlobales : MonoBehaviour
{
    public static VariablesGlobales Instancia;
    [Header("Ubicaci�n Habitaciones")]
    public Vector3 Tama�oHabitacion;        // Tama�o en 3 dimensiones de la habitaci�n m�nima
    public float EspacioEntreHabitaciones;  // Espacio despu�s de los bordes de una habitaci�n
    public float MargenCambioHabitacion;    // Margen de maniobra para el teletransporte del personaje
    [Header("Transiciones")]
    public float TiempoCambioHabitacion;    // Tiempo de durac��n del fundido negro -> transparente
    [Header("IAEnemigo")]
    public float RadioDeteccion;            // Distancia en la que un enemigo cambia a Combate
    public int MemoriaAtencion;             // Cantidad de objetivos que puede recordar un enemigo
    void Awake()
    {
        Instancia = this;
    }
}
