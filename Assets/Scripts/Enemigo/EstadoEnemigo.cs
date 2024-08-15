using System.Threading.Tasks;
using UnityEngine;

public class EstadoEnemigo : MonoBehaviour
{
    public enum Estados { Vigilante, Alerta, Combate }
    public Estados Estado;
    public enum Acciones { Idle, Mover, Atacar, Curar }     // TODO: Temporal, tendrán una estructura particular
    public Acciones[] ColaDeAccion;
    public GameObject ObjetivoFijado;
    public Vector3 DestinoFijado;
    public Vector2 DistanciaAObstaculo;
    private float _ContadorObjetivoInalcanzable;

    private void Start()
    {
        // Estado inicial del enemigo
        Estado = Estados.Vigilante; // null -> Vigilante -> Alerta -> Combate
        ObjetivoFijado = null;
        DistanciaAObstaculo = new Vector2(-1f,-1f);
        DestinoFijado = Vector3.zero;

        // Las acciones que desea realizar el objeto se incluyen aquí en orden
        ColaDeAccion = new Acciones[VariablesGlobales.Instancia.TamañoColaAccion];

        // Inicia la gestión de la cola de acciones, que se llamará una y otra vez
        // TODO: evitar que destruir el objeto cause un error por esta recursión. Desactivar este script en un gestor antes de destruirlo es una forma de hacerlo.
        ResolverColaAccion();
    }

    private void Update()
    {
        // EJEMPLO DE ACCESO EXTERNO A LA COLA DE ACCIÓN. Se debe usar este método para añadir nuevas acciones y no acceder directamente al vector
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            InsertarAccion(Acciones.Curar, 0, true);
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
        else if(Mathf.Abs(transform.position.x - DestinoFijado.x) < .5f && Mathf.Abs(transform.position.y - DestinoFijado.y) < 3f)
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

    async public void ResolverColaAccion()
    {
        // Variable que evalua si hemos completado una tarea con el fin de reponerla si fuera necesario
        bool realizada = false;
        // Variable de memoria que usamos para comprobar que no se ha cambiado de forma externa la acción que estamos realizando mientras la hacemos
        Acciones buffer = ColaDeAccion[0];
        switch(ColaDeAccion[0])
        {
            // Realizar un Idle
            case Acciones.Idle:
                // Esperar 1 segundo
                await Task.Delay(100);
                // Completada
                realizada = true;
                break;

            // TODO: Realizar un Atacar
            case Acciones.Atacar:
                await Task.Delay(100);
                realizada = true;
                break;

            // TODO: Realizar un Curar
            case Acciones.Curar:
                await Task.Delay(1000);
                realizada = true;
                break;

            // Realizar un Mover
            case Acciones.Mover:
                // Comprobar si se ha completado el movimiento
                realizada = RecordarPosicion();
                // Esperar 1 segundo
                await Task.Delay(100);
                break;
        }

        // Si hemos cambiado la acción a ejecutar durante la ejecución de la antigua, no necesitamos reponerla
        if(buffer != ColaDeAccion[0])
        {
            realizada = false;
        }
        // Si necesitamos reponer la acción, lo hacemos de la siguiente forma
        if(realizada)
        {
            // Movemos todas las acciones a una posición con mayor prioridad en la cola (la primera queda eliminada)
            for (int i = 0; i < ColaDeAccion.Length - 1; i++)
            {
                ColaDeAccion[i] = ColaDeAccion[i + 1];
            }
            // La última acción pasa a ser un Idle (predefinido), que son los que menos prioridad tienen a la hora de ser cambiados
            ColaDeAccion[ColaDeAccion.Length - 1] = Acciones.Idle;
            // Elegimos una nueva acción con mayor criterio
            DecidirSiguienteAccion();
        }

        // Volvemos a realizar todo el procedimiento de esta función mientras el objeto exista
        ResolverColaAccion();
    }

    private void DecidirSiguienteAccion()
    {
        // Si no cambiamos nada, la nueva acción es un Idle
        Acciones accionDecidida = Acciones.Idle;
        // Si el objeto quiere combatir, la nueva acción será un Combate
        if (Estado == Estados.Combate)
        {
            accionDecidida = Acciones.Atacar;
            // TODO: Si en un futuro queremos que el combate sea inmediato, podemos pedir que fuerce la acción de la siguiente forma:
            // InsertarAccion(accionDecidida, 0, true);
        }
        // Si el objeto quiere perseguir y aún tiene motivación, la nueva acción será un Mover (y recuperará la motivación)
        else if (Estado == Estados.Alerta && _ContadorObjetivoInalcanzable < VariablesGlobales.Instancia.MaximoTiempoInalcanzable)
        {
            _ContadorObjetivoInalcanzable = 0f;
            accionDecidida = Acciones.Mover;
        }
        // Añadimos la acción a la cola con el método adecuado, sin forzar y al final de la prioridad
        InsertarAccion(accionDecidida, ColaDeAccion.Length - 1, false);
    }

    public void InsertarAccion(Acciones accionInsertada, int posicionNueva, bool forzarCambio)
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
                if (ColaDeAccion[i] == Acciones.Idle)
                {
                    posicionNueva = i;
                    break;
                }
            }
            // Avisamos en caso de que no se haya podido añadir la nueva acción sin forzar la cola
            if (ColaDeAccion[posicionNueva] != Acciones.Idle && ColaDeAccion[posicionNueva] != accionInsertada)
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
