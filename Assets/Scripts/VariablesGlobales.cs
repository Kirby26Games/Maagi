using UnityEngine;
using UnityEngine.Rendering;

public class VariablesGlobales : MonoBehaviour
{
    public static VariablesGlobales Instancia;
    [Header("Física")]
    public float Gravedad = -9.82f;
    [Header("Ubicación Habitaciones")]
    public Vector3 TamañoHabitacion;        // Tamaño en 3 dimensiones de la habitación mínima
    public float EspacioEntreHabitaciones;  // Espacio después de los bordes de una habitación
    public float MargenCambioHabitacion;    // Margen de maniobra para el teletransporte del personaje
    [Header("Transiciones")]
    public float TiempoCambioHabitacion;    // Tiempo de duracíón del fundido negro -> transparente
    [Header("IAEnemigo")]
    public float RadioDeteccion;            // Distancia en la que un enemigo cambia a Perseguir
    public float RadioCombate;              // Distancia en la que un enemigo cambia a Ataque
    public int MemoriaAtencion;             // Cantidad de objetivos que puede recordar un enemigo
    public float FuerzaSalto;               // Fuerza inicial del salto en los enemigos
    [SerializeReference] public BolaDeFuego Bolita = new BolaDeFuego(); // Ejemplo de SerializeReference
    public int TamañoInventario;            // La cantidad de objetos que puede coger un enemigo
    public int TamañoColaAccion;            // La cantidad de acciones que puede recordar para hacer un enemigo
    void Awake()
    {
        Instancia = this;
    }
}
