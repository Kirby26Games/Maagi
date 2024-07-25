using System.Collections.Generic;
using UnityEngine;

public class EnemigoDeteccion : MonoBehaviour
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
    private void Start()
    {
        // Inicializar todas las variables
        _EsferaDeteccion = GetComponent<SphereCollider>();
        _EsferaDeteccion.radius = VariablesGlobales.Instancia.RadioDeteccion;
        
        Objetivos = new GameObject[VariablesGlobales.Instancia.MemoriaAtencion];
        _MemoriaUsada = 0;

        _EstadoActual = GetComponentInParent<EstadoEnemigo>();
    }

    private void Update()
    {
        // No revisa nada si aún no ha detectado a nadie
        if( _MemoriaUsada < 1 )
        {
            return;
        }
        // Revisa la distancia de cada objetivo para evaluar la prioridad
        Objetivos = OrdenarMemoria(Objetivos);
        // Revisa que el objetivo no esté a rango de ataque
        if(_EstadoActual.ObjetivoFijado != null && Vector3.Distance(transform.position, _EstadoActual.ObjetivoFijado.transform.position) < VariablesGlobales.Instancia.RadioCombate)
        {
            _EstadoActual.Estado = "Combate";
        }
        else
        {
            _EstadoActual.Estado = string.Empty;
        }
        // Si no está en combate, revisa su estado
        if(_EstadoActual.Estado != "Combate")
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
                _EstadoActual.Estado = "Alerta";
                _EstadoActual.ObjetivoFijado = Objetivos[i];
                return;
            }
        }
        // Si nadie está visible vuelve a vigilar
        _EstadoActual.Estado = "Vigilante";
        _EstadoActual.ObjetivoFijado = null;
    }

    private bool ComprobarVisibilidad(int iteracion)
    {
        // Comprueba si el ángulo entre su mirada y donde está el objetivo se sale por cualquiera de los lados de la amplitud
        if (Mathf.Abs(Vector2.SignedAngle(Objetivos[iteracion].transform.position - transform.position, DireccionMirada)) > AmplitudMirada / 2)
        {
            return false;
        }
        RaycastHit[] detectados;
        Vector3 maximoObjetivo = new(Objetivos[iteracion].transform.position.x, Objetivos[iteracion].GetComponent<Collider>().bounds.max.y - 0.01f, Objetivos[iteracion].transform.position.z);
        Vector3 minimoObjetivo = new(Objetivos[iteracion].transform.position.x, Objetivos[iteracion].GetComponent<Collider>().bounds.min.y + 0.01f, Objetivos[iteracion].transform.position.z);
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
            int indice = EncontrarEnPosicion(Objetivos, other.gameObject);
            if(indice < 0 && _MemoriaUsada < Objetivos.Length)
            {
                // Recuérdalo
                Objetivos[_MemoriaUsada] = other.gameObject;
                _MemoriaUsada++;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Si lo habíamos detectado ya antes
        int indice = EncontrarEnPosicion(Objetivos, other.gameObject);
        if(indice > -1)
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
