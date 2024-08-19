using UnityEngine;

public class MovimientoEnemigo : MonoBehaviour
{
    public enum CriteriosSalto { Siempre, Nunca, DetectaBorde }
    [Header("Capacidades")]
    public CriteriosSalto CriterioSalto;
    public bool PuedeUsarEscaleras;
    public bool EsVolador;
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
    private float _CooldownEscalera;
    private float _MinimoCooldownEscalera;
    [Header("Vuelo")]
    public float VelocidadVuelo;
    [HideInInspector] public int MurosGolpeados;
    [HideInInspector] public Vector3 DireccionVuelo;

    private void Awake()
    {
        _EstadoActual = GetComponent<EstadoEnemigo>();
        _Cuerpo = GetComponent<Rigidbody>();
        _Gravedad = GetComponent<SistemaGravedad>();
        _Colision = GetComponent<Collider>();
    }

    private void Start()
    {
        // Debug: No sucede nada malo si están todas activas, pero no es deseable
        if(EsVolador && (PuedeUsarEscaleras || CriterioSalto != CriteriosSalto.Nunca))
        {
            throw new System.Exception("Un enemigo no puede ser volador y tener otras capacidades activas");
        }
        else if(EsVolador)
        {
            // Si es volador, hacemos que su percepción sea en todas direcciones
            GetComponentInChildren<DeteccionEnemigo>().AmplitudMirada = 360f;
            // Esto es equivalente a Idle para voladores
            MurosGolpeados = -1;
            // Dirección inicial que tomará al detectar a un objetivo
            DireccionVuelo = new Vector3(Random.Range(0f,1f), Random.Range(0f,1f), 0f).normalized;
        }

        // Inicializar otras variables de apoyo
        _MinimoCooldownEscalera = 2f;
        _CooldownEscalera = _MinimoCooldownEscalera;

    }

    private void Update()
    {
        // Voladores vuelan, es lo que suelen hacer
        if(EsVolador)
        {
            Volar();
            return;
        }

        // COMPORTAMIENTO HABITUAL: Sobre este deben ir comportamientos específicos con sus condiciones y return al final
        // Si está alerta persigue al objetivo
        if (_EstadoActual.ColaDeAccion[0] == EstadoEnemigo.Acciones.Mover)
        {
            Perseguir();
        }
        else
        {
            // Si no está persiguiendo, llamamos a otro método para que siga actualizando las fuerzas que actúan sobre el objeto
            Patrullar();
        }

        // Si está en el suelo y puede saltar recupera sus saltos
        if(CriterioSalto != CriteriosSalto.Nunca && _Gravedad.EnSuelo)
        {
            ReiniciarSaltos();
        }
        // Gestionamos un contador para que no use escaleras muy de seguido y pueda salir de ellas
        if(PuedeUsarEscaleras && _CooldownEscalera < _MinimoCooldownEscalera * 2)
        {
            _CooldownEscalera += Time.deltaTime;
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
            // Si está todo correcto para subir escaleras y no ha subido hace muy poco dejamos que las use
            if (UsarEscalera() && _CooldownEscalera > _MinimoCooldownEscalera)
            {
                // Las escaleras son siempre verticales. La dirección (subir / bajar) ya está incluida en VelocidadSubirEscaleras
                velocidadFinal = VelocidadSubirEscaleras * Vector3.up;
            }
        }

        // Mandar la velocidad resultante al cuerpo
        _Cuerpo.linearVelocity = velocidadFinal;
    }

    private bool UsarEscalera()
    {
        // Si no está cerca de escaleras no las puede usar
        if (!CercaEscalera && !_EnEscalera)
        {
            return false;
        }

        // Si no es necesario coger la escalera la suelta (o la ignora)
        if (_Gravedad.EnSuelo && Mathf.Abs(_EstadoActual.DestinoFijado.y - transform.position.y) < 3f)
        {
           SoltarEscalera();
           return false;
        }

        // Cambios de variables necesarios para poder llevar a cabo el movimiento a través de la escalera
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

        return true;
    }

    public void SoltarEscalera()
    {
        // Devolver al objeto a su estado natural una vez salga de la escalera. Si se llama y no está cogido, no debería suceder nada
        _Colision.isTrigger = false;
        _EnEscalera = false;
        VelocidadSubirEscaleras = 0f;
        _CooldownEscalera = 0f;
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
        // Una vez en el suelo, recupera la cantidad de saltos que puede realizar
        if(_Gravedad.EnSuelo)
        {
            _SaltosEnElAire = 0;
        }
    }

    private void Volar()
    {
        // Si la rutina no ha comenzado todavía
        if(MurosGolpeados < 0)
        {
            // Una vez detectado un objetivo comienza la rutina del volador
            if (_EstadoActual.ColaDeAccion[0] == EstadoEnemigo.Acciones.Mover)
            {
                MurosGolpeados = 0;
            }
            else
            {
                return;
            }
        }

        // Una vez termina la rutina, sucede esto
        if(MurosGolpeados >= VariablesGlobales.Instancia.MaximosMurosGolpeados)
        {
            // Si todavía detecta a algún objetivo, reinicia la rutina desde el principio
            if(_EstadoActual.ObjetivoFijado != null)
            {
                MurosGolpeados = 0;
            }
            // En caso contrario, detiene la rutina y llena la cola con acciones Idle
            else
            {
                _EstadoActual.Estado = EstadoEnemigo.Estados.Vigilante;
                for (int i = 0; i < _EstadoActual.ColaDeAccion.Length; i++)
                {
                    _EstadoActual.InsertarAccion(EstadoEnemigo.Acciones.Idle, i, true);
                }
                MurosGolpeados = -1;
                _Cuerpo.linearVelocity = Vector3.zero;
                return; 
            }
        }

        // Las acciones que realiza durante la rutina. La DireccionVuelo se modifica en ColisionesEnemigo
        Vector3 velocidadFinal = Vector3.zero;
        // TODO: Si el enemigo volador interrumpe el vuelo para tomar ciertas acciones, suceden aquí
        if (_EstadoActual.ColaDeAccion[0] == EstadoEnemigo.Acciones.Atacar || _EstadoActual.ColaDeAccion[0] == EstadoEnemigo.Acciones.Curar)
        {
            Debug.Log("No mueve porque prefiere hacer una acción importante: " + _EstadoActual.ColaDeAccion[0]);
        }
        else
        {
            velocidadFinal = DireccionVuelo;
            velocidadFinal.z = 0f;
        }
        _Cuerpo.linearVelocity = VelocidadVuelo * velocidadFinal.normalized;
    }
}
