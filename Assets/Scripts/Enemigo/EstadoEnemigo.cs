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

        // Las acciones que desea realizar el objeto se incluyen aqu� en orden
        ColaDeAccion = new SistemaEnemigo.Accion[VariablesGlobales.Instancia.Tama�oColaAccion];
        for (int i = 0; i < VariablesGlobales.Instancia.Tama�oColaAccion; i++)
        {
            ColaDeAccion[i] = _Enemigo.DiccionarioAcciones["Idle"];
        }

        // Variable de memoria que usamos para comprobar que no se ha cambiado de forma externa la acci�n que estamos realizando mientras la hacemos
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
        // EJEMPLO DE ACCESO EXTERNO A LA COLA DE ACCI�N. Se debe usar este m�todo para a�adir nuevas acciones y no acceder directamente al vector
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            InsertarAccion(_Enemigo.DiccionarioAcciones["Curar"], 1, true);
        }
    }

    public bool RecordarPosicion()
    {
        // Si todav�a est� viendo a su objetivo pero no suficientemente cerca para combatir
        if(ObjetivoFijado != null && Estado != Estados.Combate)
        {
            // Refresca su motivaci�n
            _ContadorObjetivoInalcanzable = 0f;
            // Su destino es este objetivo
            DestinoFijado = ObjetivoFijado.transform.position;
            // No completes el movimiento
            return false;
        }
        // Si est� relativamente cerca de su destino y no se cumple lo anterior
        else if(Mathf.Abs(transform.position.x - DestinoFijado.x) < 1.5f && Mathf.Abs(transform.position.y - DestinoFijado.y) < 3f)
        {
            // Refresca su motivaci�n
            _ContadorObjetivoInalcanzable = 0f;
            // Devuelve al estado natural
            Estado = Estados.Vigilante;
            // Completa el movimiento
            return true;
        }
        // De otra forma, a�ade tiempo para terminar su motivaci�n
        _ContadorObjetivoInalcanzable += Time.deltaTime;
        // Si ha pasado demasiado tiempo perdiendo motivaci�n
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
        // Si hay alg�n cambio en la detecci�n de enemigos, termina esta acci�n
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

        // Si hemos cambiado la acci�n a ejecutar durante la ejecuci�n de la antigua, no necesitamos reponerla
        if (buffer != ColaDeAccion[0])
        {
            realizada = false;
        }
        // Si necesitamos reponer la acci�n, lo hacemos de la siguiente forma
        if (realizada)
        {
            // Movemos todas las acciones a una posici�n con mayor prioridad en la cola (la primera queda eliminada)
            for (int i = 0; i < ColaDeAccion.Length - 1; i++)
            {
                ColaDeAccion[i] = ColaDeAccion[i + 1];
            }
            // La �ltima acci�n pasa a ser un Idle (predefinido), que son los que menos prioridad tienen a la hora de ser cambiados
            ColaDeAccion[ColaDeAccion.Length - 1] = _Enemigo.DiccionarioAcciones["Idle"];
            // Elegimos una nueva acci�n con mayor criterio
            DecidirSiguienteAccion();
        }
    }

    private void DecidirSiguienteAccion()
    {
        // Si no cambiamos nada, la nueva acci�n es un Idle
        SistemaEnemigo.Accion accionDecidida = _Enemigo.DiccionarioAcciones["Idle"];
        // Si el objeto quiere combatir, la nueva acci�n ser� un Combate
        if (Estado == Estados.Combate)
        {
            accionDecidida = _Enemigo.DiccionarioAcciones["Atacar"];
            // TODO: Si en un futuro queremos que el combate sea inmediato, podemos pedir que fuerce la acci�n de la siguiente forma:
            // InsertarAccion(accionDecidida, 0, true);
        }
        // Si el objeto quiere perseguir y a�n tiene motivaci�n, la nueva acci�n ser� un Mover (y recuperar� la motivaci�n)
        else if (Estado == Estados.Alerta && _ContadorObjetivoInalcanzable < VariablesGlobales.Instancia.MaximoTiempoInalcanzable)
        {
            _ContadorObjetivoInalcanzable = 0f;
            accionDecidida = _Enemigo.DiccionarioAcciones["Mover"];
        }
        // Si es capaz de recoger objetos, ir� a por el m�s cercano en caso de no tener nada mejor que hacer
        else if(_Enemigo.Movimiento.CogeObjetos && _Enemigo.Deteccion.BuscarObjeto() != null)
        {
            accionDecidida = _Enemigo.DiccionarioAcciones["CogerObjeto"];
        }
        // A�adimos la acci�n a la cola con el m�todo adecuado, sin forzar y al final de la prioridad
        InsertarAccion(accionDecidida, ColaDeAccion.Length - 1, false);
    }

    public void InsertarAccion(SistemaEnemigo.Accion accionInsertada, int posicionNueva, bool forzarCambio)
    {
        // Comprobamos que la posici�n en la cola existe
        if (posicionNueva >= ColaDeAccion.Length || posicionNueva < 0)
        {
            Debug.Log("Posici�n para nueva acci�n incorrecta");
            return;
        }

        // Si no queremos forzar el cambio, buscamos una posici�n que tenga un Idle con menor prioridad (mayor �ndice) para sustuirlo
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
            // Avisamos en caso de que no se haya podido a�adir la nueva acci�n sin forzar la cola
            if (ColaDeAccion[posicionNueva] != _Enemigo.DiccionarioAcciones["Idle"] && ColaDeAccion[posicionNueva] != accionInsertada)
            {
                Debug.Log("El cambio no se produjo porque no se puede forzar el cambio");
                return;
            }
        }
        // ALTERNATIVA: Al forzar una acci�n empujamos a todas las dem�s acciones en la prioridad para hacerle hueco
        //else
        //{
        //    for (int i = ColaDeAccion.Length - 1; i > posicionNueva; i--)
        //    {
        //        ColaDeAccion[i] = ColaDeAccion[i - 1];
        //    }
        //}

        // Cambiamos la acci�n en la posici�n que queremos sobreescribir
        ColaDeAccion[posicionNueva] = accionInsertada;
    }
}
