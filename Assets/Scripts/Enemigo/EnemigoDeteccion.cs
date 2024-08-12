using System.Collections.Generic;
using UnityEngine;

public class EnemigoDeteccion : MonoBehaviour
{
    [Header("Interactuables")]
    private SphereCollider _EsferaDeteccion;
    [Header("Memoria")]
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
    }

    private void Update()
    {
        for (int i = 0; i < _MemoriaUsada; i++)
        {
            if(ComprobarVisibilidad(i))
            {
                print("Perseguir a: " + Objetivo[i].name);
            }
        }
    }

    private bool ComprobarVisibilidad(int iteracion)
    {
        RaycastHit[] detectados;
        Vector3 maximoObjetivo = new Vector3(Objetivo[iteracion].transform.position.x, Objetivo[iteracion].GetComponent<Collider>().bounds.max.y - 0.01f, Objetivo[iteracion].transform.position.z);
        Vector3 minimoObjetivo = new Vector3(Objetivo[iteracion].transform.position.x, Objetivo[iteracion].GetComponent<Collider>().bounds.min.y + 0.01f, Objetivo[iteracion].transform.position.z);
        detectados = Physics.RaycastAll(transform.position, maximoObjetivo - transform.position, Vector3.Distance(maximoObjetivo, transform.position) + 0.01f);
        Debug.DrawRay(transform.position, maximoObjetivo - transform.position);
        if (SonTodosIguales(detectados))
        {
            return true;
        }
        detectados = Physics.RaycastAll(transform.position, minimoObjetivo - transform.position, Vector3.Distance(minimoObjetivo, transform.position) + 0.01f);
        if (SonTodosIguales(detectados))
        {
            return true;
        }
        return false;
    }

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
        if(other.gameObject.GetComponent<SistemasPersonaje>() != null)
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
                Objetivo[Objetivo.Length - 1] = null;
            }
        }
    }
}
