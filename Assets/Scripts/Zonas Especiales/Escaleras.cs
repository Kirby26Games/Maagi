using UnityEngine;

public class Escaleras : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MovimientoPersonaje movimientoPersonaje))
        {
            movimientoPersonaje.CercaEscalera = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out MovimientoPersonaje movimientoPersonaje))
        {
            movimientoPersonaje.CercaEscalera = false;
            movimientoPersonaje.EnEscalera = false;
            movimientoPersonaje.gameObject.GetComponent<AtaquePersonaje>().PuedoAtacar = true;
        }
    }
}
