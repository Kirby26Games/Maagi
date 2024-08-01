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
            Personaje.Movimiento.Saltar(1);
        }
        if (Input.GetKey(Controles.Saltar) && (Input.GetKeyDown(Controles.Derecha) || Input.GetKeyDown(Controles.Izquierda)) && Personaje.Movimiento.EnEscalera)
        {
            Personaje.Movimiento.SoltarEscalera();
            Personaje.Movimiento.Saltar(.1f);
        }


        if (Input.GetKeyDown(Controles.Correr))
        {
            Personaje.Movimiento.Correr(true);
        }
        if (Input.GetKeyUp(Controles.Correr))
        {
            Personaje.Movimiento.Correr(false);
        }


        if ((Input.GetKeyDown(Controles.Subir) || Input.GetKeyDown(Controles.Bajar)) && Personaje.Movimiento.CercaEscalera)
        {
            Personaje.Movimiento.PuedoSubir = true;
        }

        if ((Input.GetKeyDown(Controles.Derecha) || Input.GetKeyDown(Controles.Izquierda)) && !Personaje.Movimiento.AtravesandoSuelo)
        {
            Personaje.Movimiento.SoltarEscalera();
        }

        if (Input.GetKeyDown(Controles.Inventario))
        {
            Personaje.Inventario.ToggleInterfaz();
        }

    }
}
