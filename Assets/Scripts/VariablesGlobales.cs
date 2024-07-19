using UnityEngine;

public class VariablesGlobales : MonoBehaviour
{
    public static VariablesGlobales Instancia;
    [Header("Ubicación Habitaciones")]
    public Vector3 TamañoHabitacion;
    public float EspacioEntreHabitaciones;
    public float MargenCambioHabitacion;
    [Header("Transiciones")]
    public float TiempoCambioHabitacion;
    void Awake()
    {
        Instancia = this;
    }
}
