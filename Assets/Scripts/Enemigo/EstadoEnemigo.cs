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

    private void Start()
    {
        // Estado inicial del enemigo
        Estado = Estados.Vigilante; // null -> Vigilante -> Alerta -> Combate
        ObjetivoFijado = null;
        DistanciaAObstaculo = new Vector2(-1f,-1f);
        DestinoFijado = Vector3.zero + transform.position.z * Vector3.forward;

        ColaDeAccion = new Acciones[VariablesGlobales.Instancia.TamañoColaAccion];

        ResolverColaAccion();
    }

    private void Update()
    {
        RecordarPosicion();
    }

    public void RecordarPosicion()
    {
        if(ObjetivoFijado != null)
        {
            DestinoFijado = ObjetivoFijado.transform.position;
            return;
        }
        else if(Mathf.Abs(transform.position.x - DestinoFijado.x) < .5f && Mathf.Abs(transform.position.y - DestinoFijado.y) < 3f)
        {
            DestinoFijado = Vector3.zero;
            Estado = Estados.Vigilante;
            return;
        }
    }

    // TODO
    async public void ResolverColaAccion()
    {
        bool realizada = false;
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
                await Task.Delay(100);
                realizada = true;
                break;
        }

        if(realizada)
        {
            for (int i = 0; i < ColaDeAccion.Length - 1; i++)
            {
                ColaDeAccion[i] = ColaDeAccion[i + 1];
            }
            ColaDeAccion[ColaDeAccion.Length - 1] = Acciones.Idle;
            DecidirSiguienteAccion(ColaDeAccion.Length - 1, false);
        }

        ResolverColaAccion();
    }

    // TODO
    public void DecidirSiguienteAccion(int posicionNueva, bool forzarCambio)
    {
        if(posicionNueva >= ColaDeAccion.Length || posicionNueva < 0)
        {
            Debug.Log("Posición para nueva acción incorrecta");
            return;
        }

        Acciones accionDecidida = Acciones.Idle;
        if (Estado == Estados.Combate)
        {
            accionDecidida = Acciones.Atacar;
        }
        else if (Estado == Estados.Alerta)
        {
            accionDecidida = Acciones.Mover;
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
            if (ColaDeAccion[posicionNueva] != Acciones.Idle && ColaDeAccion[posicionNueva] != accionDecidida)
            {
                Debug.Log("El cambio no se produjo porque no se puede forzar el cambio");
                return;
            }
        }

        ColaDeAccion[posicionNueva] = accionDecidida;
    }
}
