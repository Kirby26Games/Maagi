using TreeEditor;
using UnityEngine;

[RequireComponent(typeof(MovimientoPersonaje), typeof(SistemaGravedad), typeof(SistemaRaycast))]
[RequireComponent(typeof(ControlesPersonaje), typeof(Tama�oPersonaje))]

public class SistemasPersonaje : MonoBehaviour
{
    //Este script contendra un indice de todos los scripts en uso
    public MovimientoPersonaje Movimiento;
    public SistemaGravedad Gravedad;
    public SistemaRaycast Raycast;
    public ControlesPersonaje Controles;
    public Tama�oPersonaje Tama�o;


    private void Awake()
    {    
        Movimiento = GetComponent<MovimientoPersonaje>();
        Gravedad = GetComponent<SistemaGravedad>();      
        Raycast = GetComponent<SistemaRaycast>();
        Controles = GetComponent<ControlesPersonaje>();      
        Tama�o = GetComponent<Tama�oPersonaje>();     
    }
}
