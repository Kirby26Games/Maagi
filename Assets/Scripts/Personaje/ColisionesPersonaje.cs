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
            Personaje.Movimiento.PosicionEscalera = escaleras.transform.position.x;
            Personaje.Movimiento.VelocidadSubirEscaleras = escaleras.VelocidadSubirEscalera;
        }

        if (other.TryGetComponent(out Suelo suelo))
        {
            Personaje.Movimiento.AtravesandoSuelo = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Escaleras escaleras))
        {
            Personaje.Movimiento.SoltarEscalera();
            Personaje.Movimiento.CercaEscalera = false;
        }

        if (other.TryGetComponent(out Suelo suelo))
        {
            Personaje.Movimiento.AtravesandoSuelo = false;
        }
    }

}
