using UnityEngine;
using UnityEngine.Rendering;

public class VariablesGlobales : MonoBehaviour
{
    public static VariablesGlobales Instancia;
    [Header("Ubicación Habitaciones")]
    public Vector3 TamañoHabitacion;        // Tamaño en 3 dimensiones de la habitación mínima
    public float EspacioEntreHabitaciones;  // Espacio después de los bordes de una habitación
    public float MargenCambioHabitacion;    // Margen de maniobra para el teletransporte del personaje
    [Header("Transiciones")]
    public float TiempoCambioHabitacion;    // Tiempo de duracíón del fundido negro -> transparente
    [Header("IAEnemigo")]
    public float RadioDeteccion;            // Distancia en la que un enemigo cambia a Combate
    public int MemoriaAtencion;             // Cantidad de objetivos que puede recordar un enemigo
    void Awake()
    {
        Instancia = this;
    }
}
