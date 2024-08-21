using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class DeteccionEnemigo : MonoBehaviour
{
    [Header("Interactuables")]
    private SphereCollider _EsferaDeteccion;
    [Header("Memoria")]
    private EstadoEnemigo _EstadoActual;
    public GameObject[] Objetivos;
    private int _MemoriaUsada;
    [Header("Parametros")]
    public Vector3 DireccionMirada;
    public float AmplitudMirada;
    [SerializeField] private LayerMask _CapasIgnoradasPorVision;

    private void Awake()
    {
        // Guardamos los scripts necesarios
        _EsferaDeteccion = GetComponent<SphereCollider>();
        _EstadoActual = GetComponentInParent<EstadoEnemigo>();
    }
    private void Start()
    {
        // Inicializar todas las variables
        _EsferaDeteccion.radius = VariablesGlobales.Instancia.RadioDeteccion;

        Objetivos = new GameObject[VariablesGlobales.Instancia.MemoriaAtencion];
        _MemoriaUsada = 0;
    }

    private void Update()
    {
        // No revisa nada si aún no ha detectado a nadie
        if (_MemoriaUsada < 1 && _EstadoActual.DestinoFijado == Vector3.zero)
        {
            // Reset si ya ha visto a alguien antes
            if(_EstadoActual.ObjetivoFijado != null)
            {
                _EstadoActual.ObjetivoFijado = null;
                _EstadoActual.DistanciaAObstaculo = new Vector2(-1f, -1f);
            }
            return;
        }
        // Revisa la distancia de cada objetivo para evaluar la prioridad
        Objetivos = OrdenarMemoria(Objetivos);
        // Lógica del enemigo
        CambiarEstadoEnemigo();
    }

    private void CambiarEstadoEnemigo()
    {
        // Revisa que el objetivo no esté a rango de ataque
        if (_EstadoActual.ObjetivoFijado != null &&
            Vector3.Distance(transform.position, _EstadoActual.ObjetivoFijado.transform.position) < VariablesGlobales.Instancia.RadioCombate &&
            _EstadoActual.ColaDeAccion[0] != EstadoEnemigo.Acciones.CogerObjeto)
        {
            _EstadoActual.Estado = EstadoEnemigo.Estados.Combate;
            return;
        }

        for (int i = 0; i < _MemoriaUsada; i++)
        {
            // Comprueba si el objetivo está subiendo una escalera
            if (Objetivos[i].transform.GetComponent<Collider>().isTrigger && 
                Mathf.Abs(Vector2.SignedAngle(Objetivos[i].transform.position - transform.position, DireccionMirada)) <= 120f)
            {
                _EstadoActual.Estado = EstadoEnemigo.Estados.Alerta;
                _EstadoActual.DestinoFijado = Objetivos[i].transform.position;
                _EstadoActual.DestinoFijado.y += Mathf.Sign(_EstadoActual.DestinoFijado.y - transform.position.y) * 2f;
                return;
            }
            // Si tiene visibilidad de un objetivo lo persigue, siguiendo el orden de prioridad
            if (ComprobarVisibilidad(Objetivos[i].transform.position,
                Objetivos[i].GetComponent<Collider>().bounds.max.y,
                Objetivos[i].GetComponent<Collider>().bounds.min.y))
            {
                _EstadoActual.Estado = EstadoEnemigo.Estados.Alerta;
                _EstadoActual.ObjetivoFijado = Objetivos[i];
                return;
            }
        }
        _EstadoActual.ObjetivoFijado = null;
        if (_EstadoActual.DestinoFijado != Vector3.zero)
        {
            if(ComprobarVisibilidad(_EstadoActual.DestinoFijado, _EstadoActual.DestinoFijado.y + 1f, _EstadoActual.DestinoFijado.y - 1f))
            {
                return;
            }
        }
    }

    private bool ComprobarVisibilidad(Vector3 posicion, float alturaMaxima, float alturaMinima)
    {
        bool detectadoObjetivo = false;
        // Comprueba si el ángulo entre su mirada y donde está el objetivo se sale por cualquiera de los lados de la amplitud
        if (Mathf.Abs(Vector2.SignedAngle(posicion - transform.position, DireccionMirada)) > AmplitudMirada / 2)
        {
            return false;
        }
        RaycastHit[] detectados;

        // Puntos a detectar del objetivo
        Vector3 maximoObjetivo = new(posicion.x, alturaMaxima - 0.05f, posicion.z);
        Vector3 minimoObjetivo = new(posicion.x, alturaMinima + 0.05f, posicion.z);
        
        // Detecta todos los objetos entre él y la parte superior del objetivo
        detectados = Physics.RaycastAll(transform.position + 0.25f * transform.up, maximoObjetivo - (transform.position + 0.1f * transform.up), Vector3.Distance(maximoObjetivo, transform.position) + 0.01f, ~_CapasIgnoradasPorVision);
        Debug.DrawRay(transform.position + 0.25f * transform.up, maximoObjetivo - (transform.position + 0.1f * transform.up));
        // Si solo choca con un mismo objeto devuelve true
        if (ContieneSoloPersonaje(detectados))
        {
            detectadoObjetivo = true;
        }
        
        // Detecta todos los objetos entre él y la parte inferior del objetivo
        detectados = Physics.RaycastAll(transform.position - 0.25f * transform.up, minimoObjetivo - (transform.position - 0.1f * transform.up), Vector3.Distance(minimoObjetivo, transform.position) + 0.01f, ~_CapasIgnoradasPorVision);
        Debug.DrawRay(transform.position - 0.25f * transform.up, minimoObjetivo - (transform.position - 0.1f * transform.up));
        // Si solo choca con un mismo objeto devuelve true
        if (ContieneSoloPersonaje(detectados))
        {
            detectadoObjetivo = true;
            _EstadoActual.DistanciaAObstaculo = new Vector2(-1f, -1f);
        }
        // Si se detecta la parte de arriba pero no la de abajo se puede saltar
        else if (detectadoObjetivo)
        {
            _EstadoActual.DistanciaAObstaculo = BuscarMasCercano(detectados);
        }

        // De otra forma, devuelve false
        return detectadoObjetivo;
    }

    // Devuelve la menor distancia entre los objetos detectados al mirar al objetivo
    public Vector2 BuscarMasCercano(RaycastHit[] detectados)
    {
        if(detectados.Length < 0)
        {
            return new Vector2(-1f, -1f);
        }

        float menorDistancia = detectados[0].distance;
        for (int i = 0; i < detectados.Length; i++)
        {
            if(menorDistancia > detectados[i].distance)
            {
                menorDistancia = detectados[i].distance;
            }
        }
        return new Vector2(Mathf.Abs(detectados[0].point.x - transform.position.x), Mathf.Abs(detectados[0].point.y - transform.position.y));
    }

    // Coprueba si todas las instancias de objetos detectados por un Raycast son el personaje y/o escaleras
    private bool ContieneSoloPersonaje(RaycastHit[] detectados)
    {
        for (int i = 0; i < detectados.Length; i++)
        {
            if (detectados[i].collider.gameObject.GetComponent<SistemasPersonaje>() == null &&
                detectados[i].collider.gameObject.GetComponent<Escaleras>() == null)
            {
                return false;
            }
        }
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si el objeto tiene SistemasPersonaje
        if (other.gameObject.TryGetComponent(out SistemasPersonaje sistemaPersonaje))
        {
            if(other == sistemaPersonaje.Colisiones.ColisionPersonaje)
            {
                // Y si no lo habíamos detectado ya antes y tenemos espacio en memoria
                int indice = EncontrarEnPosicion(Objetivos, other.gameObject);
                if (indice < 0 && _MemoriaUsada < Objetivos.Length)
                {
                    // Recuérdalo
                    Objetivos[_MemoriaUsada] = other.gameObject;
                    _MemoriaUsada++;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Si lo habíamos detectado ya antes
        int indice = EncontrarEnPosicion(Objetivos, other.gameObject);
        if (indice > -1)
        {
            // Olvídalo
            Objetivos[indice] = null;
            _MemoriaUsada--;
            // Y reorganiza la memoria
            ReorganizarMemoria();
        }
    }

    // Busca un GameObject en un vector, devuelve su posición o -1 si no lo encuentra
    private int EncontrarEnPosicion(GameObject[] vector, GameObject buscado)
    {
        for (int i = 0; i < vector.Length; i++)
        {
            if (buscado == vector[i])
            {
                return i;
            }
        }
        return -1;
    }

    // Reorganiza el vector Objetivo manteniendo el orden y dejando instancias vacías al final
    private void ReorganizarMemoria()
    {
        for (int i = 0; i < Objetivos.Length; i++)
        {
            if (Objetivos[i] == null)
            {
                for (int j = i; j < Objetivos.Length - 1; j++)
                {
                    Objetivos[j] = Objetivos[j + 1];
                }
                Objetivos[^1] = null;
            }
        }
    }

    private GameObject[] OrdenarMemoria(GameObject[] memoria)
    {
        // Revisa cuantos espacios vacíos hay en la memoria para tenerlos en cuenta
        int cuentaVacios = 0;
        GameObject[] ordenada = new GameObject[memoria.Length];
        for (int i = 0; i < memoria.Length; i++)
        {
            if (memoria[i] == null)
            {
                ordenada[memoria.Length - cuentaVacios - 1] = null;
                cuentaVacios++;
            }
        }

        // Ordena por proximidad los objetivos no vacíos detectados
        for (int i = 0; i < memoria.Length - cuentaVacios; i++)
        {
            int posicion = 0;
            for (int j = 0; j < memoria.Length - cuentaVacios; j++)
            {
                if (Vector3.Distance(memoria[i].transform.position, transform.position) > Vector3.Distance(memoria[j].transform.position, transform.position))
                {
                    posicion++;
                }
            }
            ordenada[posicion] = memoria[i];
        }

        return ordenada;
    }

    public GameObject BuscarObjeto()
    {
        Collider[] listaObjetos = Physics.OverlapSphere(transform.position, VariablesGlobales.Instancia.RadioDeteccion, _CapasIgnoradasPorVision);
        GameObject objetoObjetivo = null;
        for (int i = 0; i < listaObjetos.Length; i++)
        {
            if(listaObjetos[i].gameObject.GetComponent<ObjetoEscena>() != null)
            {
                if(objetoObjetivo == null)
                {
                    objetoObjetivo = listaObjetos[i].gameObject;
                }
                else if(Vector3.Distance(objetoObjetivo.transform.position,transform.position) > Vector3.Distance(listaObjetos[i].gameObject.transform.position,transform.position))
                {
                    objetoObjetivo = listaObjetos[i].gameObject;
                }
            }
        }
        return objetoObjetivo;
    }
}
