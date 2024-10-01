using UnityEngine;

[RequireComponent(typeof(Estadisticas), typeof(SistemaGravedad))]
public abstract class SistemaBase : MonoBehaviour
{
    public SistemaGravedad Gravedad;
    public Estadisticas Estadisticas;
    public ControlesBase ControlesBase;
    public MovimientoBase MovimientoBase;

    public Clase Clase;
    public abstract void MeMuero();
}
