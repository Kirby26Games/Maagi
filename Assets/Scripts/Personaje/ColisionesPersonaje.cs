using UnityEngine;

public class ColisionesPersonaje : MonoBehaviour
{
    public bool CercaEscalera;
    private SistemasPersonaje _Personaje;
    public SphereCollider ColisionDeteccion;
    public CapsuleCollider ColisionPersonaje;

    private void Awake()
    {
        _Personaje = GetComponent<SistemasPersonaje>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ZonasGravedad zonasGravedad))
        {
            zonasGravedad.CalcularCambioGravedad(gameObject);
        }

        if (other.TryGetComponent(out Escaleras escaleras))
        {
            _Personaje.Movimiento.CercaEscalera = true;
            _Personaje.Movimiento.PosicionEscalera = escaleras.transform.position.x;
            _Personaje.Movimiento.VelocidadSubirEscaleras = escaleras.VelocidadSubirEscalera;
        }

        if (other.TryGetComponent(out Suelo suelo))
        {
            _Personaje.Movimiento.AtravesandoSuelo = true;
        }

        if (other.TryGetComponent(out ObjetoEscena objetoEscena))
        {
            _Personaje.Inventario.GestorObjetosCogibles(objetoEscena);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Escaleras escaleras))
        {
            _Personaje.Movimiento.SoltarEscalera();
            _Personaje.Movimiento.CercaEscalera = false;
        }

        if (other.TryGetComponent(out Suelo suelo))
        {
            _Personaje.Movimiento.AtravesandoSuelo = false;
        }

        if (other.TryGetComponent(out ObjetoEscena objetoEscena))
        {
            _Personaje.Inventario.GestorObjetosCogibles(objetoEscena);
        }
    }
}
