using UnityEngine;

public class EstadoEnemigo : MonoBehaviour
{
    public enum Estados { Vigilante, Alerta, Combate }
    public Estados Estado;
    public GameObject ObjetivoFijado;
    public Vector3 DestinoFijado;
    public float DistanciaAObstaculo;

    private void Start()
    {
        // Estado inicial del enemigo
        Estado = Estados.Vigilante; // null -> Vigilante -> Alerta -> Combate
        ObjetivoFijado = null;
        DistanciaAObstaculo = -1f;
        DestinoFijado = Vector3.zero;
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
