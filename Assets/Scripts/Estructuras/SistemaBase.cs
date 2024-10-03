using UnityEngine;

[RequireComponent(typeof(Estadisticas), typeof(SistemaGravedad))]
public abstract class SistemaBase : MonoBehaviour
{
    public SistemaGravedad Gravedad;
    public Estadisticas Estadisticas;
    public ControlesBase ControlesBase;
    public MovimientoBase MovimientoBase;
    public Vector3 CentroHabitacion;

    public Clase Clase;
    public abstract void MeMuero();

    // Llamar cuando se produzca un cambio de habitaci�n al personaje y en ella est�n los enemigos
    public void EstablecerCentroHabitacion()
    {
        CentroHabitacion = Camera.main.transform.position;
        CentroHabitacion += Vector3.forward * (253.5f - CentroHabitacion.z);
        Debug.Log(CentroHabitacion);
    }
}
