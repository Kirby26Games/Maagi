using System.Threading.Tasks;
using UnityEngine;

public class EstadoEnemigo : MonoBehaviour
{
    public enum Estados { Vigilante, Alerta, Combate }
    public Estados Estado;
    public SistemaEnemigo.Accion[] ColaDeAccion;
    public GameObject ObjetivoFijado;
    public Vector3 DestinoFijado;
    public Vector2 DistanciaAObstaculo;
    private float _ContadorObjetivoInalcanzable;
    private SistemaEnemigo _Enemigo;

    private void Awake()
    {
        _Enemigo = GetComponent<SistemaEnemigo>();
    }

    private async void Start()
    {
        // Estado inicial del enemigo
        Estado = Estados.Vigilante; // null -> Vigilante -> Alerta -> Combate
        ObjetivoFijado = null;
        DistanciaAObstaculo = new Vector2(-1f,-1f);
        DestinoFijado = Vector3.zero;

        // Las acciones que desea realizar el objeto se incluyen aquí en orden
        ColaDeAccion = new SistemaEnemigo.Accion[VariablesGlobales.Instancia.TamañoColaAccion];
        for (int i = 0; i < VariablesGlobales.Instancia.TamañoColaAccion; i++)
        {
            ColaDeAccion[i] = _Enemigo.DiccionarioAcciones["Idle"];
        }

        // Variable de memoria que usamos para comprobar que no se ha cambiado de forma externa la acción que estamos realizando mientras la hacemos
        SistemaEnemigo.Accion buffer = ColaDeAccion[0];
        while (true)
        {
            buffer = ColaDeAccion[0];
            ColaDeAccion[0].AlIniciarAccion();
            await Task.Delay(ColaDeAccion[0].Duracion);
            ReponerColaDeAccion(buffer);
        }
    }

    private void Update()
    {
        // EJEMPLO DE ACCESO EXTERNO A LA COLA DE ACCIÓN. Se debe usar este método para añadir nuevas acciones y no acceder directamente al vector
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            InsertarAccion(_Enemigo.DiccionarioAcciones["Curar"], 1, true);
        }
    }

    public bool RecordarPosicion()
    {
        // Si todavía está viendo a su objetivo pero no suficientemente cerca para combatir
        if(ObjetivoFijado != null && Estado != Estados.Combate)
        {
            // Refresca su motivación
            _ContadorObjetivoInalcanzable = 0f;
            // Su destino es este objetivo
            DestinoFijado = ObjetivoFijado.transform.position;
            // No completes el movimiento
            return false;
        }
        // Si está relativamente cerca de su destino y no se cumple lo anterior
        else if(Mathf.Abs(transform.position.x - DestinoFijado.x) < 1.5f && Mathf.Abs(transform.position.y - DestinoFijado.y) < 3f)
        {
            // Refresca su motivación
            _ContadorObjetivoInalcanzable = 0f;
            // Devuelve al estado natural
            Estado = Estados.Vigilante;
            // Completa el movimiento
            return true;
        }
        // De otra forma, añade tiempo para terminar su motivación
        _ContadorObjetivoInalcanzable += Time.deltaTime;
        // Si ha pasado demasiado tiempo perdiendo motivación
        if(_ContadorObjetivoInalcanzable > VariablesGlobales.Instancia.MaximoTiempoInalcanzable)
        {
            // Completa el movimiento
            return true;
        }
        // De otra forma, no completes el movimiento
        return false;
    }

    public bool RecordarObjeto()
    {
        // Si hay algún cambio en la detección de enemigos, termina esta acción
        if(Estado != Estados.Vigilante || !_Enemigo.Movimiento.CogeObjetos)
        {
            return true;
        }
        if (ObjetivoFijado == null)
        {
            ObjetivoFijado = _Enemigo.Deteccion.BuscarObjeto();
            if(ObjetivoFijado != null)
            {
                DestinoFijado = ObjetivoFijado.transform.position;
                return false;
            }
            else
            {
                return true;
            }
        }
        // De otra forma, no completes el movimiento
        return false;
    }

    public void ReponerColaDeAccion(SistemaEnemigo.Accion buffer)
    {
        // Variable que evalua si hemos completado una tarea con el fin de reponerla si fuera necesario
        bool realizada = ColaDeAccion[0].CriterioParada();

        // Si hemos cambiado la acción a ejecutar durante la ejecución de la antigua, no necesitamos reponerla
        if (buffer != ColaDeAccion[0])
        {
            realizada = false;
        }
        // Si necesitamos reponer la acción, lo hacemos de la siguiente forma
        if (realizada)
        {
            // Movemos todas las acciones a una posición con mayor prioridad en la cola (la primera queda eliminada)
            for (int i = 0; i < ColaDeAccion.Length - 1; i++)
            {
                ColaDeAccion[i] = ColaDeAccion[i + 1];
            }
            // La última acción pasa a ser un Idle (predefinido), que son los que menos prioridad tienen a la hora de ser cambiados
            ColaDeAccion[ColaDeAccion.Length - 1] = _Enemigo.DiccionarioAcciones["Idle"];
            // Elegimos una nueva acción con mayor criterio
            DecidirSiguienteAccion();
        }
    }

    private void DecidirSiguienteAccion()
    {
        // Si no cambiamos nada, la nueva acción es un Idle
        SistemaEnemigo.Accion accionDecidida = _Enemigo.DiccionarioAcciones["Idle"];
        // Si el objeto quiere combatir, la nueva acción será un Combate
        if (Estado == Estados.Combate)
        {
            accionDecidida = _Enemigo.DiccionarioAcciones["Atacar"];
            // TODO: Si en un futuro queremos que el combate sea inmediato, podemos pedir que fuerce la acción de la siguiente forma:
            // InsertarAccion(accionDecidida, 0, true);
        }
        // Si el objeto quiere perseguir y aún tiene motivación, la nueva acción será un Mover (y recuperará la motivación)
        else if (Estado == Estados.Alerta && _ContadorObjetivoInalcanzable < VariablesGlobales.Instancia.MaximoTiempoInalcanzable)
        {
            _ContadorObjetivoInalcanzable = 0f;
            accionDecidida = _Enemigo.DiccionarioAcciones["Mover"];
        }
        // Si es capaz de recoger objetos, irá a por el más cercano en caso de no tener nada mejor que hacer
        else if(_Enemigo.Movimiento.CogeObjetos && _Enemigo.Deteccion.BuscarObjeto() != null)
        {
            accionDecidida = _Enemigo.DiccionarioAcciones["CogerObjeto"];
        }
        // Añadimos la acción a la cola con el método adecuado, sin forzar y al final de la prioridad
        InsertarAccion(accionDecidida, ColaDeAccion.Length - 1, false);
    }

    public void InsertarAccion(SistemaEnemigo.Accion accionInsertada, int posicionNueva, bool forzarCambio)
    {
        // Comprobamos que la posición en la cola existe
        if (posicionNueva >= ColaDeAccion.Length || posicionNueva < 0)
        {
            Debug.Log("Posición para nueva acción incorrecta");
            return;
        }

        // Si no queremos forzar el cambio, buscamos una posición que tenga un Idle con menor prioridad (mayor índice) para sustuirlo
        if (!forzarCambio)
        {
            for (int i = posicionNueva; i < ColaDeAccion.Length; i++)
            {
                if (ColaDeAccion[i] == _Enemigo.DiccionarioAcciones["Idle"])
                {
                    posicionNueva = i;
                    break;
                }
            }
            // Avisamos en caso de que no se haya podido añadir la nueva acción sin forzar la cola
            if (ColaDeAccion[posicionNueva] != _Enemigo.DiccionarioAcciones["Idle"] && ColaDeAccion[posicionNueva] != accionInsertada)
            {
                Debug.Log("El cambio no se produjo porque no se puede forzar el cambio");
                return;
            }
        }
        // ALTERNATIVA: Al forzar una acción empujamos a todas las demás acciones en la prioridad para hacerle hueco
        //else
        //{
        //    for (int i = ColaDeAccion.Length - 1; i > posicionNueva; i--)
        //    {
        //        ColaDeAccion[i] = ColaDeAccion[i - 1];
        //    }
        //}

        // Cambiamos la acción en la posición que queremos sobreescribir
        ColaDeAccion[posicionNueva] = accionInsertada;
    }
}
