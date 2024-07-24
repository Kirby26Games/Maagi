using TreeEditor;
using UnityEngine;

[RequireComponent(typeof(MovimientoPersonaje), typeof(SistemaGravedad), typeof(SistemaRaycast))]
<<<<<<< HEAD
[RequireComponent(typeof(ControlesPersonaje), typeof(TamañoPersonaje), typeof(ColisionesPersonaje))]
[RequireComponent(typeof(AtaquePersonaje))]
=======
[RequireComponent(typeof(ControlesPersonaje), typeof(TamañoPersonaje), typeof(ApuntarPersonaje))]
>>>>>>> origin/Alex

public class SistemasPersonaje : MonoBehaviour
{
    //Este script contendra un indice de todos los scripts en uso
    public MovimientoPersonaje Movimiento;
    public SistemaGravedad Gravedad;
    public SistemaRaycast Raycast;
    public ControlesPersonaje Controles;
    public TamañoPersonaje Tamaño;
<<<<<<< HEAD
    public ColisionesPersonaje Colisiones;
    public AtaquePersonaje Ataque;
=======
    public ApuntarPersonaje Apuntar;
>>>>>>> origin/Alex


    private void Awake()
    {    
        Movimiento = GetComponent<MovimientoPersonaje>();
        Gravedad = GetComponent<SistemaGravedad>();      
        Raycast = GetComponent<SistemaRaycast>();
        Controles = GetComponent<ControlesPersonaje>();      
<<<<<<< HEAD
        Tamaño = GetComponent<TamañoPersonaje>(); 
        Colisiones = GetComponent<ColisionesPersonaje>();
        Ataque = GetComponent<AtaquePersonaje>();

=======
        Tamaño = GetComponent<TamañoPersonaje>();
        Apuntar = GetComponent<ApuntarPersonaje>();
>>>>>>> origin/Alex
    }
}
