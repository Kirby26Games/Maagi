using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public class MovimientoPersonaje : MonoBehaviour
{

    [Header("Movimiento")]
    public float VelocidadBase = 3;
    public float MultiplicadorAlCorrer = 2;
    private float VelocidadModificador = 1;
    private float VelocidadFinal;
    private Vector3 MovimientoXZ;
    private Vector3 MovimientoFinal;
    [Header("Salto")]
    public float DistanciaSalto;
    public int SaltosEnElAireMaximos;
    private int SaltosEnElAire;
    private Rigidbody RBPersonaje;
    private ControlesPersonaje Controles;
    private SistemasPersonaje Personaje;
    [Header("Escalera")]
    public int VelocidadSubirEscaleras;
    public bool EnEscalera;
    public bool CercaEscalera;



    private void Awake()
    {
        Controles = GetComponent<ControlesPersonaje>();
        RBPersonaje = GetComponent<Rigidbody>();
        Personaje = GetComponent<SistemasPersonaje>();
    }


    private void Start()
    {
        CalcularVelocidad();
    }

    void Update()
    {
        if ((CercaEscalera && Controles.EjeZ != 0) || EnEscalera)
        {
            SubirEscaleras();
        }
        else
        {
            Movimiento();
        }

        ReiniciarSaltos();
        
    }

    void Movimiento()
    {
        
        MovimientoXZ = new Vector3(Controles.EjeX, 0, 0).normalized;
        MovimientoFinal = transform.TransformDirection(MovimientoXZ) * VelocidadFinal;
        MovimientoFinal = Vector3.ProjectOnPlane(MovimientoFinal, Personaje.Raycast.DatosPendiente.normal);
        MovimientoFinal += Personaje.Gravedad.DireccionGravedad;
        RBPersonaje.linearVelocity = MovimientoFinal;
    }

    public void Saltar()
    {
        if (!PuedoSaltar())
        {
            return;
        }
        Personaje.Gravedad.EjeY = Mathf.Sqrt(DistanciaSalto * -2 * Personaje.Gravedad.Gravedad); 
    }

    public bool PuedoSaltar()
    {
        bool Puedo = false;
        //Si estoy en el suelo, siempre puedo saltar
        if (Personaje.Gravedad.EnSuelo || EnEscalera)
        {
            Puedo = true;
        }
        //Si estoy en el aire, puedo saltar si no he llegado a los saltos maximos
        else if (SaltosEnElAire < SaltosEnElAireMaximos)
        {
            Puedo = true;
            SaltosEnElAire++;
        }
        return Puedo;
    }

    public void ReiniciarSaltos()
    {
        if (Personaje.Gravedad.EnSuelo)
        {
            SaltosEnElAire = 0;
        }
    }

    public void Correr(bool Corriendo)
    {
        if (Corriendo)
        {
            VelocidadModificador = MultiplicadorAlCorrer;
        }
        else
        {
            VelocidadModificador = 1;
        }
        CalcularVelocidad();
    }

    public void CalcularVelocidad()
    {
        VelocidadFinal = VelocidadBase * VelocidadModificador;
    }

    public void SubirEscaleras()
    {
        //MODIFICACIONES: Sistema gravedad pone EjeY = 0 cuando EnEscalera = true; Funcion PuedoSaltar ahora devuelve true si esta EnEscalera;
        EnEscalera = true;
        Personaje.Ataque.PuedoAtacar = false;

        MovimientoXZ = new Vector3(Controles.EjeX, Controles.EjeZ, 0).normalized;
        MovimientoFinal = transform.TransformDirection(MovimientoXZ) * VelocidadSubirEscaleras;
        RBPersonaje.linearVelocity = MovimientoFinal;
    }
}