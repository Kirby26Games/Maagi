using UnityEngine;

public class EstadoEnemigo : MonoBehaviour
{
    public enum Estados { Vigilante, Alerta, Combate }
    public Estados Estado;
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
}
