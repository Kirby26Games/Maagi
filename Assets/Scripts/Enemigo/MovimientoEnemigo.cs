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
    private bool _CercaEscalera;
    private float _PosicionEscalera;
    private float _VelocidadSubirEscaleras;
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
        // Si está alerta persigue al objetivo
        if(_EstadoActual.Estado == EstadoEnemigo.Estados.Alerta)
        {
            Perseguir();
        }
        else if(_EstadoActual.Estado == EstadoEnemigo.Estados.Vigilante)
        {
            Patrullar();
        }
        // Si está en el suelo y puede saltar recupera sus saltos
        if(CriterioSalto != CriteriosSalto.Nunca && _Gravedad.EnSuelo)
        {
            ReiniciarSaltos();
        }
    }

    private void Patrullar()
    {
        // A velocidadFinal se le van a añadir los distintos desplazamientos
        Vector3 velocidadFinal = Vector3.zero;

        // Añadir la gravedad que le afecta
        velocidadFinal.y += _Gravedad.EjeY;

        // Mandar la velocidad resultante al cuerpo
        _Cuerpo.linearVelocity = velocidadFinal;
    }

    private void Perseguir()
    {
        // A velocidadFinal se le van a añadir los distintos desplazamientos
        Vector3 velocidadFinal = Vector3.zero;
        // Añadir la velocidad correcta en la dirección correcta
        velocidadFinal += (_EstadoActual.DestinoFijado - transform.position).normalized.x * VelocidadMovimiento * Vector3.right;

        // Revisar si debe saltar en caso de que sea capaz
        Saltar();

        // Añadir la gravedad que le afecta
        velocidadFinal.y += _Gravedad.EjeY;

        // Revisar si debe usar escaleras en caso de que sea capaz
        if (PuedeUsarEscaleras)
        {
            if(UsarEscalera())
            {
                velocidadFinal = _VelocidadSubirEscaleras * Vector3.up;
            }
        }

        // Mandar la velocidad resultante al cuerpo
        _Cuerpo.linearVelocity = velocidadFinal;
    }

    private bool UsarEscalera()
    {
        // Si no está cerca de escaleras no las puede usar
        if (!_CercaEscalera && !_EnEscalera)
        {
            return false;
        }

        // Si no es necesario coger la escalera la ignora
        if (_Gravedad.EnSuelo && Mathf.Abs(_EstadoActual.DestinoFijado.y - transform.position.y) < 3f)
        {
           return false;
        }

        _EnEscalera = true;
        transform.position = new Vector3(_PosicionEscalera, transform.position.y, transform.position.z);
        _Colision.isTrigger = true;

        if (transform.position.y < _EstadoActual.DestinoFijado.y)
        {
            // Subir escalera
            _VelocidadSubirEscaleras = Mathf.Abs(_VelocidadSubirEscaleras);
            return true;
        }
        if (transform.position.y > _EstadoActual.DestinoFijado.y)
        {
            // Bajar escalera
            _VelocidadSubirEscaleras = -1 * Mathf.Abs(_VelocidadSubirEscaleras);
            return true;
        }

        return false;
    }

    private void SoltarEscalera()
    {
        _Colision.isTrigger = false;
        _EnEscalera = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Escaleras escaleras))
        {
            _CercaEscalera = true;
            _PosicionEscalera = escaleras.transform.position.x;
            _VelocidadSubirEscaleras = escaleras.VelocidadSubirEscalera;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Escaleras escaleras))
        {
            _CercaEscalera = false;
            SoltarEscalera();
        }
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

            // Detecta borde se fija en la distancia al obstáculo más cercano (si existe) y si el objetivo está por encima de su posición
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
