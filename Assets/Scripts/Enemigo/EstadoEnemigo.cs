using System.Threading.Tasks;
using UnityEngine;

public class EstadoEnemigo : MonoBehaviour
{
    public enum Estados { Vigilante, Alerta, Combate }
    public Estados Estado;
    public enum Acciones { Idle, Mover, Atacar, Curar }     // TODO: Temporal, serán una estructura particular
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

        ColaDeAccion = new Acciones[VariablesGlobales.Instancia.TamañoColaAccion];

        ResolverColaAccion();
    }

    private void Update()
    {
        // EJEMPLO DE ACCESO EXTERNO A LA COLA DE ACCIÓN
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            InsertarAccion(Acciones.Curar, 0, true);
        }
    }

    public bool RecordarPosicion()
    {
        if(ObjetivoFijado != null && Estado != Estados.Combate)
        {
            _ContadorObjetivoInalcanzable = 0f;
            DestinoFijado = ObjetivoFijado.transform.position;
            return false;
        }
        else if(Mathf.Abs(transform.position.x - DestinoFijado.x) < .5f && Mathf.Abs(transform.position.y - DestinoFijado.y) < 3f)
        {
            _ContadorObjetivoInalcanzable = 0f;
            Estado = Estados.Vigilante;
            return true;
        }
        _ContadorObjetivoInalcanzable += Time.deltaTime;
        return false;
    }

    async public void ResolverColaAccion()
    {
        bool realizada = false;
        Acciones buffer = ColaDeAccion[0];
        switch(ColaDeAccion[0])
        {
            case Acciones.Idle:
                await Task.Delay(100);
                realizada = true;
                break;

            case Acciones.Atacar:
                await Task.Delay(100);
                realizada = true;
                break;

            case Acciones.Curar:
                await Task.Delay(1000);
                realizada = true;
                break;

            case Acciones.Mover:
                realizada = RecordarPosicion();
                await Task.Delay(100);
                break;
        }

        if(buffer != ColaDeAccion[0])
        {
            realizada = false;
        }
        if(realizada)
        {
            for (int i = 0; i < ColaDeAccion.Length - 1; i++)
            {
                ColaDeAccion[i] = ColaDeAccion[i + 1];
            }
            ColaDeAccion[ColaDeAccion.Length - 1] = Acciones.Idle;
            DecidirSiguienteAccion();
        }

        ResolverColaAccion();
    }

    private void DecidirSiguienteAccion()
    {

        Acciones accionDecidida = Acciones.Idle;
        if (Estado == Estados.Combate)
        {
            accionDecidida = Acciones.Atacar;
        }
        else if (Estado == Estados.Alerta && _ContadorObjetivoInalcanzable < VariablesGlobales.Instancia.MaximoTiempoInalcanzable)
        {
            _ContadorObjetivoInalcanzable = 0f;
            accionDecidida = Acciones.Mover;
        }
        InsertarAccion(accionDecidida, ColaDeAccion.Length - 1, false);
    }

    public void InsertarAccion(Acciones accionInsertada, int posicionNueva, bool forzarCambio)
    {
        if (posicionNueva >= ColaDeAccion.Length || posicionNueva < 0)
        {
            Debug.Log("Posición para nueva acción incorrecta");
            return;
        }


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
            if (ColaDeAccion[posicionNueva] != Acciones.Idle && ColaDeAccion[posicionNueva] != accionInsertada)
            {
                Debug.Log("El cambio no se produjo porque no se puede forzar el cambio");
                return;
            }
        }

        for (int i = ColaDeAccion.Length - 1; i > posicionNueva; i--)
        {
            ColaDeAccion[i] = ColaDeAccion[i - 1];
        }
        ColaDeAccion[posicionNueva] = accionInsertada;
    }
}
