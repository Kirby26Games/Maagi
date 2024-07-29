using UnityEngine;
using System.Collections.Generic;

public class DeteccionEnemigo : MonoBehaviour
{
    [Header("Interactuables")]
    private SphereCollider _EsferaDeteccion;
    [Header("Memoria")]
    public GameObject[] Objetivo;
    private int _MemoriaUsada;
    [Header("Parametros")]
    public Vector3 DireccionMirada;
    public float AmplitudMirada;

    private void Awake()
    {
        _EsferaDeteccion = GetComponent<SphereCollider>();
        _EstadoActual = GetComponentInParent<EstadoEnemigo>();
    }
    private void Start()
    {
        // Inicializar todas las variables
        _EsferaDeteccion.radius = VariablesGlobales.Instancia.RadioDeteccion;
<<<<<<< HEAD:Assets/Scripts/Enemigo/EnemigoDeteccion.cs
        
        Objetivo = new GameObject[VariablesGlobales.Instancia.MemoriaAtencion];
=======

        Objetivos = new GameObject[VariablesGlobales.Instancia.MemoriaAtencion];
>>>>>>> origin/Noah:Assets/Scripts/Enemigo/DeteccionEnemigo.cs
        _MemoriaUsada = 0;
    }

    private void Update()
    {
<<<<<<< HEAD:Assets/Scripts/Enemigo/EnemigoDeteccion.cs
=======
        // No revisa nada si aún no ha detectado a nadie
        if (_MemoriaUsada < 1)
        {
            return;
        }
        // Revisa la distancia de cada objetivo para evaluar la prioridad
        Objetivos = OrdenarMemoria(Objetivos);
        // Revisa que el objetivo no esté a rango de ataque
        if (_EstadoActual.ObjetivoFijado != null && Vector3.Distance(transform.position, _EstadoActual.ObjetivoFijado.transform.position) < VariablesGlobales.Instancia.RadioCombate)
        {
            _EstadoActual.Estado = "Combate";
        }
        else
        {
            _EstadoActual.Estado = string.Empty;
        }
        // Si no está en combate, revisa su estado
        if (_EstadoActual.Estado != "Combate")
        {
            CambiarEstadoEnemigo();
        }
    }

    private void CambiarEstadoEnemigo()
    {
>>>>>>> origin/Noah:Assets/Scripts/Enemigo/DeteccionEnemigo.cs
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
        if (other.gameObject.GetComponent<MovimientoSimple>() != null)
        {
            // Y si no lo habíamos detectado ya antes y tenemos espacio en memoria
<<<<<<< HEAD:Assets/Scripts/Enemigo/EnemigoDeteccion.cs
            int indice = EncontrarEnPosicion(Objetivo, other.gameObject);
            if(indice < 0 && _MemoriaUsada < Objetivo.Length)
=======
            int indice = EncontrarEnPosicion(Objetivos, other.gameObject);
            if (indice < 0 && _MemoriaUsada < Objetivos.Length)
>>>>>>> origin/Noah:Assets/Scripts/Enemigo/DeteccionEnemigo.cs
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
<<<<<<< HEAD:Assets/Scripts/Enemigo/EnemigoDeteccion.cs
        int indice = EncontrarEnPosicion(Objetivo, other.gameObject);
        if(indice > -1)
=======
        int indice = EncontrarEnPosicion(Objetivos, other.gameObject);
        if (indice > -1)
>>>>>>> origin/Noah:Assets/Scripts/Enemigo/DeteccionEnemigo.cs
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
<<<<<<< HEAD:Assets/Scripts/Enemigo/EnemigoDeteccion.cs
=======

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
>>>>>>> origin/Noah:Assets/Scripts/Enemigo/DeteccionEnemigo.cs
}
