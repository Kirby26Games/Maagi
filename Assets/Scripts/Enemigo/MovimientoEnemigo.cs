using Unity.VisualScripting;
using UnityEngine;

public class MovimientoEnemigo : MonoBehaviour
{
    public enum CriteriosSalto { Siempre, Nunca, DetectaBorde }
    [Header("Capacidades")]
    public CriteriosSalto CriterioSalto;
    public bool PuedeUsarEscaleras;
    [Header("Memoria")]
    private EstadoEnemigo _EstadoActual;
    [Header("Propiedades")]
    private Rigidbody _Cuerpo;
    private Collider _Colision;
    public float VelocidadMovimiento;
    [Header("Salto")]
    public float DistanciaSalto;
    public int SaltosEnElAireMaximos;
    private int _SaltosEnElAire;
    private SistemaGravedad _Gravedad;
    [Header("Escaleras")]
    [HideInInspector] public bool CercaEscalera;
    [HideInInspector] public float PosicionEscalera;
    [HideInInspector] public float VelocidadSubirEscaleras;
    private bool _EnEscalera;

    private void Awake()
    {
        _EstadoActual = GetComponent<EstadoEnemigo>();
        _Cuerpo = GetComponent<Rigidbody>();
        _Gravedad = GetComponent<SistemaGravedad>();
        _Colision = GetComponent<Collider>();
    }

    private void Update()
    {
        // Si est� alerta persigue al objetivo
        if(_EstadoActual.Estado == EstadoEnemigo.Estados.Alerta)
        {
            Perseguir();
        }
        else if(_EstadoActual.Estado == EstadoEnemigo.Estados.Vigilante)
        {
            if(_EnEscalera)
            {
                SoltarEscalera();
            }
            Patrullar();
        }
        // Si est� en el suelo y puede saltar recupera sus saltos
        if(CriterioSalto != CriteriosSalto.Nunca && _Gravedad.EnSuelo)
        {
            ReiniciarSaltos();
        }
    }

    private void Patrullar()
    {
        // A velocidadFinal se le van a a�adir los distintos desplazamientos
        Vector3 velocidadFinal = Vector3.zero;

        // A�adir la gravedad que le afecta
        velocidadFinal.y += _Gravedad.EjeY;

        // Mandar la velocidad resultante al cuerpo
        _Cuerpo.linearVelocity = velocidadFinal;
    }

    private void Perseguir()
    {
        // A velocidadFinal se le van a a�adir los distintos desplazamientos
        Vector3 velocidadFinal = Vector3.zero;
        // A�adir la velocidad correcta en la direcci�n correcta
        velocidadFinal += (_EstadoActual.DestinoFijado - transform.position).normalized.x * VelocidadMovimiento * Vector3.right;

        // Revisar si debe saltar en caso de que sea capaz
        Saltar();

        // A�adir la gravedad que le afecta
        velocidadFinal.y += _Gravedad.EjeY;

        // Revisar si debe usar escaleras en caso de que sea capaz
        if (PuedeUsarEscaleras)
        {
            if(UsarEscalera())
            {
                velocidadFinal = VelocidadSubirEscaleras * Vector3.up;
            }
        }

        // Mandar la velocidad resultante al cuerpo
        _Cuerpo.linearVelocity = velocidadFinal;
    }

    private bool UsarEscalera()
    {
        // Si no est� cerca de escaleras no las puede usar
        if (!CercaEscalera && !_EnEscalera)
        {
            return false;
        }

        // Si no es necesario coger la escalera la ignora
        if (_Gravedad.EnSuelo && Mathf.Abs(_EstadoActual.DestinoFijado.y - transform.position.y) < 3f)
        {
           return false;
        }

        _EnEscalera = true;
        transform.position = new Vector3(PosicionEscalera, transform.position.y, transform.position.z);
        _Colision.isTrigger = true;

        if (transform.position.y < _EstadoActual.DestinoFijado.y)
        {
            // Subir escalera
            VelocidadSubirEscaleras = Mathf.Abs(VelocidadSubirEscaleras);
            return true;
        }
        if (transform.position.y > _EstadoActual.DestinoFijado.y)
        {
            // Bajar escalera
            VelocidadSubirEscaleras = -1 * Mathf.Abs(VelocidadSubirEscaleras);
            return true;
        }

        return false;
    }

    public void SoltarEscalera()
    {
        _Colision.isTrigger = false;
        _EnEscalera = false;
    }

    private void Saltar()
    {
        // Si no puede o no quiere saltar no lo hace
        if (!PuedoSaltar() || !QuieroSaltar())
        {
            return;
        }

        // En caso contrario, calcula lo que debe saltar
        _Gravedad.EjeY = Mathf.Sqrt(DistanciaSalto * -2 * VariablesGlobales.Instancia.Gravedad);
    }

    private bool PuedoSaltar()
    {
        bool puedo = false;
        //Si estoy en el suelo, siempre puedo saltar
        if (_Gravedad.EnSuelo)
        {
            puedo = true;
        }
        //Si estoy en el aire, puedo saltar si no he llegado a los saltos maximos
        else if (_SaltosEnElAire < SaltosEnElAireMaximos)
        {
            puedo = true;
            _SaltosEnElAire++;
        }
        return puedo;
    }

    private bool QuieroSaltar()
    {
        // Dependiendo del criterio, devuelve true si quiere saltar o false si no
        switch(CriterioSalto)
        {
            // Siempre devuelve true siempre
            case CriteriosSalto.Siempre:
                return true;

            // Detecta borde se fija en la distancia al obst�culo m�s cercano (si existe) y si el objetivo est� por encima de su posici�n
            case CriteriosSalto.DetectaBorde:
                if(_EstadoActual.DistanciaAObstaculo.x < VelocidadMovimiento &&
                    _EstadoActual.DistanciaAObstaculo.x > -1f &&
                    _EstadoActual.DistanciaAObstaculo.y < DistanciaSalto &&
                    _EstadoActual.DistanciaAObstaculo.y > -1f &&
                    _EstadoActual.DestinoFijado.y > transform.position.y)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
        // Nunca devuelve false siempre
        return false;
    }

    public void ReiniciarSaltos()
    {
        if(_Gravedad.EnSuelo)
        {
            _SaltosEnElAire = 0;
        }
    }
}
