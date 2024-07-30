using UnityEngine;

public class EstadoEnemigo : MonoBehaviour
{
    public enum Estados { Vigilante, Alerta, Combate }
    public Estados Estado;
    public GameObject ObjetivoFijado;
    public float DistanciaAObstaculo;

    private void Start()
    {
        // Estado inicial del enemigo
        Estado = Estados.Vigilante; // null -> Vigilante -> Alerta -> Combate
        ObjetivoFijado = null;
        DistanciaAObstaculo = -1f;
    }
}
