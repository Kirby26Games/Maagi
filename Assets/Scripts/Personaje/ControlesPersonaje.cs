using System.Globalization;
using System.Threading.Tasks;
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

    private async void Start()
    {
        await Task.Delay(1);
        Personaje.Clase = GestorClases.Instancia.Clases[0];
        Personaje.Estadisticas.Fuerza.Base = 3;
        Personaje.Estadisticas.Fuerza.Calcular();
        Personaje.Estadisticas.Constitucion.Base = 5;
        Personaje.Estadisticas.Constitucion.Calcular();
        Personaje.Estadisticas.Nivel.Base = 1;
        Personaje.Estadisticas.Nivel.Calcular();
    }
    private void Update()
    {
        EjeX = _EjeXTotal();
        EjeZ = _EjeZTotal();
        RatonHorizontal = Input.GetAxis("Mouse X");
        RatonVertical = Input.GetAxis("Mouse Y");
        

        if (Input.GetKeyDown(Controles.Saltar))
        {
            Personaje.Movimiento.Saltar(1);
        }
        if (Input.GetKey(Controles.Saltar) && (Input.GetKeyDown(Controles.Derecha) || Input.GetKeyDown(Controles.Izquierda)) && Personaje.Movimiento.EnEscalera)
        {
            Personaje.Movimiento.SoltarEscalera();
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


        if (Input.GetKeyUp(Controles.Apuntar))
        {
            Personaje.Apuntar.DetectarObjetivos();
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

        if (Input.GetKeyDown(Controles.Interactuar) && Personaje.Inventario.ObjetosCogibles.Count > 0)
        {
            Personaje.Inventario.SortObjetosCogibles();
            ObjetoEscena objetoAgregado = Personaje.Inventario.ObjetosCogibles[0];

            if (Personaje.Inventario.AgregarAInventario(objetoAgregado))
            {
                Destroy(objetoAgregado.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Personaje.Clase.Habilidades[2].Lanzar(Personaje);
        }
    }

    //Hace el getaxisraw pero con los botones que uno elige
    float _EjeXTotal()
    {
        if (Input.GetKey(Controles.Derecha))
        {
            return 1;
        }
        if (Input.GetKey(Controles.Izquierda))
        {
            return -1;
        }
        return 0;
    }

    //Hace el getaxisraw pero con los botones que uno elige
    float _EjeZTotal()
    {
        if (Input.GetKey(Controles.Subir))
        {
            return 1;
        }
        if (Input.GetKey(Controles.Bajar))
        {
            return -1;
        }
        return 0;


    }
}
