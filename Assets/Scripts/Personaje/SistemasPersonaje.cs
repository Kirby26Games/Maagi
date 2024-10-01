using UnityEngine;

[RequireComponent(typeof(MovimientoPersonaje), typeof(SistemaRaycast))]
[RequireComponent(typeof(ControlesPersonaje), typeof(TamañoPersonaje), typeof(ColisionesPersonaje))]

[RequireComponent(typeof(AtaquePersonaje), typeof(InventarioPersonaje), typeof(ApuntarPersonaje))]



public class SistemasPersonaje : SistemaBase
{
    //Este script contendra un indice de todos los scripts en uso
    public MovimientoPersonaje Movimiento;
    public SistemaRaycast Raycast;
    public ControlesPersonaje Controles;
    public TamañoPersonaje Tamaño;
    public ColisionesPersonaje Colisiones;
    public AtaquePersonaje Ataque;
    public InventarioPersonaje Inventario;
    public ApuntarPersonaje Apuntar;



    private void Awake()
    {
        Movimiento = GetComponent<MovimientoPersonaje>();
        MovimientoBase = Movimiento;
        Gravedad = GetComponent<SistemaGravedad>();      
        Raycast = GetComponent<SistemaRaycast>();
        Controles = GetComponent<ControlesPersonaje>();
        ControlesBase = Controles;
        Tamaño = GetComponent<TamañoPersonaje>(); 
        Colisiones = GetComponent<ColisionesPersonaje>();
        Ataque = GetComponent<AtaquePersonaje>();
        Inventario = GetComponent<InventarioPersonaje>();
        Apuntar = GetComponent<ApuntarPersonaje>();
        Estadisticas = GetComponent<Estadisticas>();
    }

    public override void MeMuero()
    {
    }
}
