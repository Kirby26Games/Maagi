using UnityEngine;

public class ColisionesPersonaje : MonoBehaviour
{
    public bool CercaEscalera;
    private SistemasPersonaje Personaje;

    private void Awake()
    {
        Personaje = GetComponent<SistemasPersonaje>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ZonasGravedad zonasGravedad))
        {
            zonasGravedad.CalcularCambioGravedad(gameObject);
        }

        if (other.TryGetComponent(out Escaleras escaleras))
        {
            Personaje.Movimiento.CercaEscalera = true;
            Personaje.Movimiento.VelocidadSubirEscaleras = escaleras.VelocidadSubirEscalera;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Escaleras escaleras))
        {
            Personaje.Movimiento.CercaEscalera = false;
            Personaje.Movimiento.EnEscalera = false;
            Personaje.Ataque.PuedoAtacar = true;
        }
    }

}
