using UnityEngine;

public class VariablesGlobales : MonoBehaviour
{
    public static VariablesGlobales Instancia;
    public Vector3 Tama�oHabitacion;
    public float EspacioEntreHabitaciones;
    public float MargenCambioHabitacion;
    void Awake()
    {
        Instancia = this;
    }
}
