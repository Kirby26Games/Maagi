using UnityEngine;

[RequireComponent(typeof(Estadisticas), typeof(SistemaGravedad))]
public class SistemaBase : MonoBehaviour
{
    public SistemaGravedad Gravedad;
    public Estadisticas Estadisticas;

    public Clase Clase;
}
