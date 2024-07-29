using System.Globalization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
    public bool Saltando;
    public float DistanciaSalto;
    public int SaltosEnElAireMaximos;
    private int SaltosEnElAire;
    private Rigidbody RBPersonaje;
    private ControlesPersonaje Controles;
    private SistemasPersonaje Personaje;
    [Header("Escalera")]
    public int VelocidadSubirEscaleras;
    public bool AtravesandoSuelo;
    public bool EnEscalera;
    public bool PuedoSubir;
    public bool CercaEscalera;
    public float PosicionEscalera;
    public float EscaleraLimiteSuperior;
    public float EscaleraLimiteInferior;
    private Collider ColPersonaje;



    private void Awake()
    {
        Controles = GetComponent<ControlesPersonaje>();
        RBPersonaje = GetComponent<Rigidbody>();
        Personaje = GetComponent<SistemasPersonaje>();
        ColPersonaje = GetComponent<Collider>();
    }


    private void Start()
    {
        CalcularVelocidad();
    }

    void Update()
    {
        FlipDireccion();
        
        SubirEscaleras(PuedoSubir);

        if (!EnEscalera)
        {
            Movimiento();
        }
        
        ReiniciarSaltos();
        
    }

    void Movimiento()
    {
        if (Personaje.Gravedad.EjeY <= 0 || CercaEscalera)
        {
            MovimientoXZ = new Vector3(Controles.EjeX, 0, 0).normalized;
        }
        else
        {
            MovimientoXZ = new Vector3(RBPersonaje.linearVelocity.x, 0, 0).normalized;
        }
        
        MovimientoFinal = transform.TransformDirection(MovimientoXZ) * VelocidadFinal;
        MovimientoFinal = Vector3.ProjectOnPlane(MovimientoFinal, Personaje.Raycast.DatosPendiente.normal);
        MovimientoFinal += Personaje.Gravedad.DireccionGravedad;
        RBPersonaje.linearVelocity = MovimientoFinal;
    }

    void FlipDireccion()
    {
        if (Controles.EjeX > 0) 
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (Controles.EjeX < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    public void Saltar(float porcentajeSalto)
    {
        if (!PuedoSaltar())
        {
            return;
        }
        Saltando = true;
        Personaje.Gravedad.EjeY = Mathf.Sqrt((DistanciaSalto * porcentajeSalto) * -2 * Personaje.Gravedad.Gravedad);
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

            if (Personaje.Gravedad.EjeY < 1)
            {
                Saltando = false;
            }
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

    public void SubirEscaleras(bool puedoSubir)
    {
        if ((CercaEscalera && puedoSubir) || EnEscalera)
        {
            Personaje.Gravedad.enabled = false;

            ColPersonaje.isTrigger = true;
            EnEscalera = true;
            Personaje.Ataque.PuedoAtacar = false;

            Personaje.transform.position = new Vector3(PosicionEscalera, transform.position.y, transform.position.z);

            MovimientoXZ = new Vector3(Personaje.Controles.EjeX, Personaje.Controles.EjeZ, 0).normalized;
            MovimientoFinal = transform.TransformDirection(MovimientoXZ) * VelocidadSubirEscaleras;
            RBPersonaje.linearVelocity = MovimientoFinal;

            if (Personaje.Gravedad.EnSuelo && Controles.EjeZ == 0)
            {
                SoltarEscalera();
            }
        }

        PuedoSubir = false;

    }

    public void SoltarEscalera()
    {
        Personaje.Gravedad.enabled = true;
        Personaje.Movimiento.EnEscalera = false;
        Personaje.Ataque.PuedoAtacar = true;
        ColPersonaje.isTrigger = false;
    }
}