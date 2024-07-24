using System.Collections.Generic;
using UnityEngine;

public class EnemigoDeteccion : MonoBehaviour
{
    [Header("Interactuables")]
    private SphereCollider _EsferaDeteccion;
    [Header("Memoria")]
    public EstadoEnemigo EstadoActual;
    public GameObject[] Objetivo;
    private int _MemoriaUsada;
    [Header("Parametros")]
    public Vector3 DireccionMirada;
    public float AmplitudMirada;
    private void Start()
    {
        // Inicializar todas las variables
        _EsferaDeteccion = GetComponent<SphereCollider>();
        _EsferaDeteccion.radius = VariablesGlobales.Instancia.RadioDeteccion;
        
        Objetivo = new GameObject[VariablesGlobales.Instancia.MemoriaAtencion];
        _MemoriaUsada = 0;

        EstadoActual = GetComponentInParent<EstadoEnemigo>();
    }

    private void Update()
    {
        // Revisa la distancia de cada objetivo para evaluar la prioridad
        Objetivo = OrdenarMemoria(Objetivo);
        // Revisa que el objetivo no esté a rango de ataque
        if(EstadoActual.ObjetivoFijado != null && Vector3.Distance(transform.position, EstadoActual.ObjetivoFijado.transform.position) < VariablesGlobales.Instancia.RadioCombate)
        {
            EstadoActual.Estado = "Combate";
        }
        else
        {
            EstadoActual.Estado = string.Empty;
        }
        // Si no está en combate, revisa su estado
        if(EstadoActual.Estado != "Combate")
        {
            CambiarEstadoEnemigo();
        }
    }

    private void CambiarEstadoEnemigo()
    {
        for (int i = 0; i < _MemoriaUsada; i++)
        {
            // Si tiene visibilidad de un objetivo lo persigue, siguiendo el orden de prioridad
            if (ComprobarVisibilidad(i))
            {
                EstadoActual.Estado = "Perseguir";
                EstadoActual.ObjetivoFijado = Objetivo[i];
                return;
            }
        }
        // Si nadie está visible vuelve a vigilar
        EstadoActual.Estado = "Vigilante";
        EstadoActual.ObjetivoFijado = null;
    }

    private bool ComprobarVisibilidad(int iteracion)
    {
        RaycastHit[] detectados;
        Vector3 maximoObjetivo = new(Objetivo[iteracion].transform.position.x, Objetivo[iteracion].GetComponent<Collider>().bounds.max.y - 0.01f, Objetivo[iteracion].transform.position.z);
        Vector3 minimoObjetivo = new(Objetivo[iteracion].transform.position.x, Objetivo[iteracion].GetComponent<Collider>().bounds.min.y + 0.01f, Objetivo[iteracion].transform.position.z);
        // Detecta todos los objetos entre él y la parte superior del objetivo
        detectados = Physics.RaycastAll(transform.position, maximoObjetivo - transform.position, Vector3.Distance(maximoObjetivo, transform.position) + 0.01f);
        Debug.DrawRay(transform.position, maximoObjetivo - transform.position);
        // Si solo choca con un mismo objeto devuelve true
        if (SonTodosIguales(detectados))
        {
            return true;
        }
        // Detecta todos los objetos entre él y la parte inferior del objetivo
        detectados = Physics.RaycastAll(transform.position, minimoObjetivo - transform.position, Vector3.Distance(minimoObjetivo, transform.position) + 0.01f);
        Debug.DrawRay(transform.position, maximoObjetivo - transform.position);
        // Si solo choca con un mismo objeto devuelve true
        if (SonTodosIguales(detectados))
        {
            return true;
        }
        // De otra forma, devuelve false
        return false;
    }

    // Coprueba si todas las instancias de objetos detectados por un Raycast son iguales o no
    private bool SonTodosIguales(RaycastHit[] detectados)
    {
        for (int i = 0; i < detectados.Length; i++)
        {
            for (int j = i + 1; j < detectados.Length; j++)
            {
                if (detectados[i].collider.gameObject.name != detectados[j].collider.gameObject.name)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si el objeto tiene Movimiento Simple (cambiar más adelante)
        if(other.gameObject.GetComponent<MovimientoSimple>() != null)
        {
            // Y si no lo habíamos detectado ya antes y tenemos espacio en memoria
            int indice = EncontrarEnPosicion(Objetivo, other.gameObject);
            if(indice < 0 && _MemoriaUsada < Objetivo.Length)
            {
                // Recuérdalo
                Objetivo[_MemoriaUsada] = other.gameObject;
                _MemoriaUsada++;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Si lo habíamos detectado ya antes
        int indice = EncontrarEnPosicion(Objetivo, other.gameObject);
        if(indice > -1)
        {
            // Olvídalo
            Objetivo[indice] = null;
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
            if(buscado == vector[i])
            {
                return i;
            }
        }
        return -1;
    }

    // Reorganiza el vector Objetivo manteniendo el orden y dejando instancias vacías al final
    private void ReorganizarMemoria()
    {
        for (int i = 0; i < Objetivo.Length; i++)
        {
            if (Objetivo[i] == null)
            {
                for (int j = i; j < Objetivo.Length - 1; j++)
                {
                    Objetivo[j] = Objetivo[j + 1];
                }
                Objetivo[^1] = null;
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
            if(memoria[i] == null)
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
}
