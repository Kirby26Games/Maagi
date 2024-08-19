using UnityEngine;

public class ColisionesPersonaje : MonoBehaviour
{
    private SistemasPersonaje Personaje;
    public SphereCollider ColisionDeteccion;
    public CapsuleCollider ColisionPersonaje;

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

        if (other.TryGetComponent(out ObjetoEscena objetoEscena))
        {
            Personaje.Inventario.GestorObjetosCogibles(objetoEscena);
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

        if (other.TryGetComponent(out ObjetoEscena objetoEscena))
        {
            Personaje.Inventario.GestorObjetosCogibles(objetoEscena);
        }
    }
}
