using UnityEngine;

public class VariablesGlobales : MonoBehaviour
{
    public static VariablesGlobales Instancia;
    [Header("Ubicaci�n Habitaciones")]
    public Vector3 Tama�oHabitacion;
    public float EspacioEntreHabitaciones;
    public float MargenCambioHabitacion;
    [Header("Transiciones")]
    public float TiempoCambioHabitacion;
    void Awake()
    {
        Instancia = this;
    }
}
