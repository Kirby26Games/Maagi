using TreeEditor;
using UnityEngine;

[RequireComponent(typeof(MovimientoPersonaje), typeof(SistemaGravedad), typeof(SistemaRaycast))]
<<<<<<< HEAD
[RequireComponent(typeof(ControlesPersonaje), typeof(TamaņoPersonaje), typeof(ColisionesPersonaje))]
[RequireComponent(typeof(AtaquePersonaje))]
=======
[RequireComponent(typeof(ControlesPersonaje), typeof(TamaņoPersonaje), typeof(ApuntarPersonaje))]
>>>>>>> origin/Alex

public class SistemasPersonaje : MonoBehaviour
{
    //Este script contendra un indice de todos los scripts en uso
    public MovimientoPersonaje Movimiento;
    public SistemaGravedad Gravedad;
    public SistemaRaycast Raycast;
    public ControlesPersonaje Controles;
    public TamaņoPersonaje Tamaņo;
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
        Tamaņo = GetComponent<TamaņoPersonaje>(); 
        Colisiones = GetComponent<ColisionesPersonaje>();
        Ataque = GetComponent<AtaquePersonaje>();

=======
        Tamaņo = GetComponent<TamaņoPersonaje>();
        Apuntar = GetComponent<ApuntarPersonaje>();
>>>>>>> origin/Alex
    }
}
