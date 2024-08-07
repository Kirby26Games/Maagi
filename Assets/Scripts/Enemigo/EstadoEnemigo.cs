using System.Threading.Tasks;
using UnityEngine;

public class EstadoEnemigo : MonoBehaviour
{
    public enum Estados { Vigilante, Alerta, Combate }
    public Estados Estado;
    public enum Acciones { Mover, Atacar, Curar }     // TODO: Temporal, serán una estructura particular
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
        else if(Vector3.Distance(transform.position, DestinoFijado) < .5f)
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
        print(ColaDeAccion[0]);
        switch(ColaDeAccion[0])
        {
            case Acciones.Atacar:
                await Task.Delay(1000);
                realizada = true;
                break;

            case Acciones.Curar:
                await Task.Delay(1000);
                realizada = true;
                break;

            case Acciones.Mover:
                await Task.Delay(1000);
                realizada = true;
                break;
        }

        if(realizada)
        {
            for(int i = 0; i < ColaDeAccion.Length - 1; i++)
            {
                ColaDeAccion[i] = ColaDeAccion[i + 1];
            }
            ColaDeAccion[ColaDeAccion.Length - 1] = DecidirSiguienteAccion();
        }

        ResolverColaAccion();
    }

    // TODO
    private Acciones DecidirSiguienteAccion()
    {
        return Acciones.Mover;
    }
}
