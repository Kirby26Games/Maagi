using UnityEngine;

public class MovimientoEnemigo : MonoBehaviour
{
    public enum CriteriosSalto { Siempre, Nunca, DetectaBorde }
    [Header("Capacidades")]
    public CriteriosSalto CriterioSalto;
    public bool PuedeUsarEscaleras;
    public bool EsVolador;
    public bool CogeObjetos;
    [Header("Memoria")]
    private SistemaEnemigo _Enemigo;
    [Header("Propiedades")]
    private Rigidbody _Cuerpo;
    private Collider _Colision;
    public float VelocidadMovimiento;
    [Header("Salto")]
    public float DistanciaSalto;
    public int SaltosEnElAireMaximos;
    private int _SaltosEnElAire;
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
    [Header("Física")]
    private float _Gravedad = -9.82f;
    [Header("Comportamientos Enemigo")]
    // VUELO
    private int _MaximosMurosGolpeados = 4;       // Cantidad máxima de muros golpeados antes de volver a Idle

    private void Awake()
    {
        _Enemigo = GetComponent<SistemaEnemigo>();
        _Cuerpo = GetComponent<Rigidbody>();
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
        if (_Enemigo.Estado.ColaDeAccion[0] == _Enemigo.DiccionarioAcciones["Mover"] || _Enemigo.Estado.ColaDeAccion[0] == _Enemigo.DiccionarioAcciones["CogerObjeto"])
        {
            Perseguir();
        }
        else
        {
            // Si no está persiguiendo, llamamos a otro método para que siga actualizando las fuerzas que actúan sobre el objeto
            Patrullar();
        }

        // Si está en el suelo y puede saltar recupera sus saltos
        if(CriterioSalto != CriteriosSalto.Nunca && _Enemigo.Gravedad.EnSuelo)
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
        velocidadFinal.y += _Enemigo.Gravedad.EjeY;

        // Mandar la velocidad resultante al cuerpo
        _Cuerpo.linearVelocity = velocidadFinal;
    }

    private void Perseguir()
    {
        // A velocidadFinal se le van a añadir los distintos desplazamientos
        Vector3 velocidadFinal = Vector3.zero;
        // Añadir la velocidad correcta en la dirección correcta
        velocidadFinal += Mathf.Sign((_Enemigo.Estado.DestinoFijado - transform.position).x) * VelocidadMovimiento * Vector3.right;

        // Revisar si debe saltar en caso de que sea capaz
        Saltar();

        // Añadir la gravedad que le afecta
        velocidadFinal.y += _Enemigo.Gravedad.EjeY;

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
        if (_Enemigo.Gravedad.EnSuelo && Mathf.Abs(_Enemigo.Estado.DestinoFijado.y - transform.position.y) < 3f)
        {
           SoltarEscalera();
           return false;
        }

        // Cambios de variables necesarios para poder llevar a cabo el movimiento a través de la escalera
        _EnEscalera = true;
        transform.position = new Vector3(PosicionEscalera, transform.position.y, transform.position.z);
        _Colision.isTrigger = true;

        if (transform.position.y < _Enemigo.Estado.DestinoFijado.y)
        {
            // Subir escalera
            VelocidadSubirEscaleras = Mathf.Abs(VelocidadSubirEscaleras);
            return true;
        }
        if (transform.position.y > _Enemigo.Estado.DestinoFijado.y)
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
        _Enemigo.Gravedad.EjeY = Mathf.Sqrt(DistanciaSalto * -2 * _Gravedad);
    }

    private bool PuedoSaltar()
    {
        bool puedo = false;
        //Si estoy en el suelo, siempre puedo saltar
        if (_Enemigo.Gravedad.EnSuelo)
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
                if(_Enemigo.Estado.DistanciaAObstaculo.x < VelocidadMovimiento &&
                    _Enemigo.Estado.DistanciaAObstaculo.x > -1f &&
                    _Enemigo.Estado.DistanciaAObstaculo.y < DistanciaSalto &&
                    _Enemigo.Estado.DistanciaAObstaculo.y > -1f &&
                    _Enemigo.Estado.DestinoFijado.y > transform.position.y)
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
        if(_Enemigo.Gravedad.EnSuelo)
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
            if (_Enemigo.Estado.ColaDeAccion[0] == _Enemigo.DiccionarioAcciones["Mover"])
            {
                MurosGolpeados = 0;
            }
            else
            {
                return;
            }
        }

        // Una vez termina la rutina, sucede esto
        if(MurosGolpeados >= _MaximosMurosGolpeados)
        {
            // Si todavía detecta a algún objetivo, reinicia la rutina desde el principio
            if(_Enemigo.Estado.ObjetivoFijado != null)
            {
                MurosGolpeados = 0;
            }
            // En caso contrario, detiene la rutina y llena la cola con acciones Idle
            else
            {
                _Enemigo.Estado.Estado = EstadoEnemigo.Estados.Vigilante;
                for (int i = 0; i < _Enemigo.Estado.ColaDeAccion.Length; i++)
                {
                    _Enemigo.Estado.InsertarAccion(_Enemigo.DiccionarioAcciones["Idle"], i, true);
                }
                MurosGolpeados = -1;
                _Cuerpo.linearVelocity = Vector3.zero;
                return; 
            }
        }

        // Las acciones que realiza durante la rutina. La DireccionVuelo se modifica en ColisionesEnemigo
        Vector3 velocidadFinal = Vector3.zero;
        // TODO: Si el enemigo volador interrumpe el vuelo para tomar ciertas acciones, suceden aquí
        if (_Enemigo.Estado.ColaDeAccion[0] == _Enemigo.DiccionarioAcciones["Atacar"] || _Enemigo.Estado.ColaDeAccion[0] == _Enemigo.DiccionarioAcciones["Curar"])
        {
            Debug.Log("No mueve porque prefiere hacer una acción importante: " + _Enemigo.Estado.ColaDeAccion[0]);
        }
        else
        {
            velocidadFinal = DireccionVuelo;
            velocidadFinal.z = 0f;
        }
        _Cuerpo.linearVelocity = VelocidadVuelo * velocidadFinal.normalized;
    }
}
