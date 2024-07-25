using UnityEngine;

public class EstadoEnemigo : MonoBehaviour
{
    public string Estado;
    public GameObject ObjetivoFijado;

    private void Start()
    {
        // Estado inicial del enemigo
        Estado = "Vigilante"; // null -> Vigilante -> Alerta -> Combate
        ObjetivoFijado = null;
    }
}
