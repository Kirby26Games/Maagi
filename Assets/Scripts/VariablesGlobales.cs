using UnityEngine;
using UnityEngine.Rendering;

public class VariablesGlobales : MonoBehaviour
{
    public static VariablesGlobales Instancia;
    [Header("F�sica")]
    public float Gravedad = -9.82f;
    [Header("Ubicaci�n Habitaciones")]
    public Vector3 Tama�oHabitacion;        // Tama�o en 3 dimensiones de la habitaci�n m�nima
    public float EspacioEntreHabitaciones;  // Espacio despu�s de los bordes de una habitaci�n
    public float MargenCambioHabitacion;    // Margen de maniobra para el teletransporte del personaje
    [Header("Transiciones")]
    public float TiempoCambioHabitacion;    // Tiempo de durac��n del fundido negro -> transparente
    [Header("IAEnemigo")]
    public float RadioDeteccion;            // Distancia en la que un enemigo cambia a Perseguir
    public float RadioCombate;              // Distancia en la que un enemigo cambia a Ataque
    public int MemoriaAtencion;             // Cantidad de objetivos que puede recordar un enemigo
    public float FuerzaSalto;               // Fuerza inicial del salto en los enemigos
    [SerializeReference] public BolaDeFuego Bolita = new BolaDeFuego(); // Ejemplo de SerializeReference
    public int Tama�oInventario;            // La cantidad de objetos que puede coger un enemigo
    public int Tama�oColaAccion;            // La cantidad de acciones que puede recordar para hacer un enemigo
    void Awake()
    {
        Instancia = this;
    }
}
