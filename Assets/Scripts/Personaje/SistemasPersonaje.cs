using TreeEditor;
using UnityEngine;

[RequireComponent(typeof(MovimientoPersonaje), typeof(SistemaGravedad), typeof(SistemaRaycast))]
<<<<<<< HEAD
[RequireComponent(typeof(ControlesPersonaje), typeof(Tama�oPersonaje), typeof(ColisionesPersonaje))]
[RequireComponent(typeof(AtaquePersonaje))]
=======
[RequireComponent(typeof(ControlesPersonaje), typeof(Tama�oPersonaje), typeof(ApuntarPersonaje))]
>>>>>>> origin/Alex

public class SistemasPersonaje : MonoBehaviour
{
    //Este script contendra un indice de todos los scripts en uso
    public MovimientoPersonaje Movimiento;
    public SistemaGravedad Gravedad;
    public SistemaRaycast Raycast;
    public ControlesPersonaje Controles;
    public Tama�oPersonaje Tama�o;
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
        Tama�o = GetComponent<Tama�oPersonaje>(); 
        Colisiones = GetComponent<ColisionesPersonaje>();
        Ataque = GetComponent<AtaquePersonaje>();

=======
        Tama�o = GetComponent<Tama�oPersonaje>();
        Apuntar = GetComponent<ApuntarPersonaje>();
>>>>>>> origin/Alex
    }
}
