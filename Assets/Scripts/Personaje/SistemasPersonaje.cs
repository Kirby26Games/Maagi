using TreeEditor;
using UnityEngine;

[RequireComponent(typeof(MovimientoPersonaje), typeof(SistemaGravedad), typeof(SistemaRaycast))]
[RequireComponent(typeof(ControlesPersonaje), typeof(TamaņoPersonaje), typeof(ColisionesPersonaje))]
[RequireComponent(typeof(AtaquePersonaje))]

public class SistemasPersonaje : MonoBehaviour
{
    //Este script contendra un indice de todos los scripts en uso
    public MovimientoPersonaje Movimiento;
    public SistemaGravedad Gravedad;
    public SistemaRaycast Raycast;
    public ControlesPersonaje Controles;
    public TamaņoPersonaje Tamaņo;
    public ColisionesPersonaje Colisiones;
    public AtaquePersonaje Ataque;


    private void Awake()
    {    
        Movimiento = GetComponent<MovimientoPersonaje>();
        Gravedad = GetComponent<SistemaGravedad>();      
        Raycast = GetComponent<SistemaRaycast>();
        Controles = GetComponent<ControlesPersonaje>();      
        Tamaņo = GetComponent<TamaņoPersonaje>(); 
        Colisiones = GetComponent<ColisionesPersonaje>();
        Ataque = GetComponent<AtaquePersonaje>();
    }
}
