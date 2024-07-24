using TreeEditor;
using UnityEngine;

[RequireComponent(typeof(MovimientoPersonaje), typeof(SistemaGravedad), typeof(SistemaRaycast))]
[RequireComponent(typeof(ControlesPersonaje), typeof(TamañoPersonaje), typeof(ColisionesPersonaje))]
[RequireComponent(typeof(AtaquePersonaje), typeof(ApuntarPersonaje))]


public class SistemasPersonaje : MonoBehaviour
{
    //Este script contendra un indice de todos los scripts en uso
    public MovimientoPersonaje Movimiento;
    public SistemaGravedad Gravedad;
    public SistemaRaycast Raycast;
    public ControlesPersonaje Controles;
    public TamañoPersonaje Tamaño;
    public ColisionesPersonaje Colisiones;
    public AtaquePersonaje Ataque;
    public ApuntarPersonaje Apuntar;


    private void Awake()
    {    
        Movimiento = GetComponent<MovimientoPersonaje>();
        Gravedad = GetComponent<SistemaGravedad>();      
        Raycast = GetComponent<SistemaRaycast>();
        Controles = GetComponent<ControlesPersonaje>();      
        Tamaño = GetComponent<TamañoPersonaje>(); 
        Colisiones = GetComponent<ColisionesPersonaje>();
        Ataque = GetComponent<AtaquePersonaje>();
        Apuntar = GetComponent<ApuntarPersonaje>();
    }
}
