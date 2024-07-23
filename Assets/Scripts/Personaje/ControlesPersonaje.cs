using System.Globalization;
using UnityEngine;

public class ControlesPersonaje : MonoBehaviour
{
    [Header("Ejes")]
    public float EjeX;
    public float EjeZ;
    public float RatonHorizontal;
    public float RatonVertical;
    private SistemasPersonaje Personaje;

    private void Awake()
    {
        Personaje=GetComponent<SistemasPersonaje>();    
    }

    private void Update()
    {     
        EjeX = Input.GetAxisRaw("Horizontal");
        EjeZ = Input.GetAxisRaw("Vertical");
        RatonHorizontal = Input.GetAxis("Mouse X");
        RatonVertical = Input.GetAxis("Mouse Y");
        if (Input.GetKeyDown(Controles.Saltar))
        {
            Personaje.Movimiento.Saltar();
        }
        if (Input.GetKeyDown(Controles.Correr))
        {
            Personaje.Movimiento.Correr(true);
        }
        if (Input.GetKeyUp(Controles.Correr))
        {
            Personaje.Movimiento.Correr(false);
        }
        if (Input.GetKeyDown(Controles.Salir))
        {
        
        }
    }
}
