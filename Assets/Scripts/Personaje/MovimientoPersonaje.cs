using System.Globalization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MovimientoPersonaje : MonoBehaviour
{
    Camera cam;
    [Header("Movimiento")]
    public float VelocidadBase = 3;
    public float MultiplicadorAlCorrer = 2;
    private float VelocidadModificador = 1;
    private float VelocidadFinal;
    private Vector3 MovimientoXZ;
    private Vector3 MovimientoFinal;
    //private float UltimoEjeX;
    [Header("Salto")]
    public bool Saltando;
    public float DistanciaSalto;
    public int SaltosEnElAireMaximos;
    private int SaltosEnElAire;
    private Rigidbody RBPersonaje;
    private SistemasPersonaje Personaje;
    [Header("Escalera")]
    public int VelocidadSubirEscaleras;
    public bool AtravesandoSuelo;
    public bool EnEscalera;
    public bool PuedoSubir;
    public bool CercaEscalera;
    public float PosicionEscalera;
    private Collider ColPersonaje;



    private void Awake()
    {
        cam = Camera.main;
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
        SubirEscaleras(PuedoSubir);

        if (!EnEscalera)
        {
            Movimiento();
        }

        FlipDireccion();
        ReiniciarSaltos();
        
    }

    void Movimiento()
    {
        if (Personaje.Gravedad.EjeY <= 0 || CercaEscalera)
        {
            //MovimientoXZ = new Vector3(Controles.EjeX, 0, 0).normalized;
            MovimientoXZ = (cam.transform.right * Personaje.Controles.EjeX).normalized;
            //UltimoEjeX = Personaje.Controles.EjeX;
        }

        //Esto esta de mas, borrar a discrcion
        //else
        //{
        //    MovimientoXZ = (cam.transform.right * UltimoEjeX).normalized;
        //    //MovimientoXZ = new Vector3(UltimoEjeX, 0, 0).normalized;
        //}

        //MovimientoFinal = MovimientoXZ * VelocidadFinal;
        MovimientoFinal = cam.transform.TransformDirection(MovimientoXZ) * VelocidadFinal;
        MovimientoFinal = Vector3.ProjectOnPlane(MovimientoFinal, Personaje.Raycast.DatosPendiente.normal);
        MovimientoFinal += Personaje.Gravedad.DireccionGravedad;
        RBPersonaje.linearVelocity = MovimientoFinal;

    }

    void FlipDireccion()
    {
        //como rota el personaje
        if (Personaje.Controles.EjeX > 0)
        {
            transform.rotation = Quaternion.LookRotation(cam.transform.forward * (cam.transform.TransformDirection(transform.up).y), cam.transform.TransformDirection(transform.up));
            //transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, 0 - transform.localEulerAngles.z * (cam.transform.TransformDirection(transform.up).y), transform.localEulerAngles.z);
        }
        else if (Personaje.Controles.EjeX < 0)
        {
            transform.rotation = Quaternion.LookRotation(-cam.transform.forward * (cam.transform.TransformDirection(transform.up).y), cam.transform.TransformDirection(transform.up));
        }
    }

    public void Saltar(float porcentajeSalto = .1f)
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

            if (Personaje.Gravedad.EnSuelo && Personaje.Controles.EjeZ == 0)
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