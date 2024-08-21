using UnityEngine;

[RequireComponent(typeof(MovimientoEnemigo), typeof(SistemaGravedad), typeof(RaycastEnemigo))]
[RequireComponent(typeof(EstadoEnemigo), typeof(InventarioEnemigo), typeof(ColisionesEnemigo))]
public class SistemaEnemigo : MonoBehaviour
{
    //Este script contendra un indice de todos los scripts en uso
    public MovimientoEnemigo Movimiento;
    public SistemaGravedad Gravedad;
    public RaycastEnemigo Raycast;
    public DeteccionEnemigo Deteccion;
    public ColisionesEnemigo Colisiones;
    public InventarioEnemigo Inventario;
    public EstadoEnemigo Estado;



    private void Awake()
    {
        Movimiento = GetComponent<MovimientoEnemigo>();
        Gravedad = GetComponent<SistemaGravedad>();
        Raycast = GetComponent<RaycastEnemigo>();
        Deteccion = GetComponentInChildren<DeteccionEnemigo>();
        Colisiones = GetComponent<ColisionesEnemigo>();
        Inventario = GetComponent<InventarioEnemigo>();
        Estado = GetComponent<EstadoEnemigo>();
    }
}
