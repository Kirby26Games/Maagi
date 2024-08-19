using Unity.VisualScripting;
using UnityEngine;

public class ColisionesEnemigo : MonoBehaviour
{
    private MovimientoEnemigo _Enemigo;
    private InventarioEnemigo _Inventario;

    private void Awake()
    {
        // Guarda los scripts necesarios
        _Enemigo = GetComponent<MovimientoEnemigo>();
        _Inventario = GetComponent<InventarioEnemigo>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Gestiona la detección de escaleras
        if (other.TryGetComponent(out Escaleras escaleras))
        {
            _Enemigo.CercaEscalera = true;
            _Enemigo.PosicionEscalera = escaleras.transform.position.x;
            _Enemigo.VelocidadSubirEscaleras = escaleras.VelocidadSubirEscalera;
        }

        // Gestiona la detección de objetos
        if (other.TryGetComponent(out ObjetoEscena objetoEscena))
        {
            _Inventario.GestorObjetosCogibles(objetoEscena);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Gestiona la detección de escaleras
        if (other.TryGetComponent(out Escaleras escaleras))
        {
            _Enemigo.CercaEscalera = false;
            _Enemigo.SoltarEscalera();
        }
        if (other.TryGetComponent(out ObjetoEscena objetoEscena))
        {
            _Inventario.GestorObjetosCogibles(objetoEscena);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Si no es volador, no es necesario el siguiente código
        if(!_Enemigo.EsVolador)
        {
            return;
        }

        // Gestiona la dirección de vuelo a partir del lugar donde ha chocado
        // TODO: Detectar cuando choque contra un muro o cualquier otro objeto valido
        if (collision.gameObject != gameObject && collision.gameObject.GetComponent<SistemasPersonaje>() == null)
        {
            Vector3 vectorNormal = collision.GetContact(0).normal.normalized;
            if (Mathf.Abs(vectorNormal.x) > 0.5f)
            {
                _Enemigo.DireccionVuelo.x = vectorNormal.x;
            }
            if (Mathf.Abs(vectorNormal.y) > 0.5f)
            {
                _Enemigo.DireccionVuelo.y = vectorNormal.y;
            }
            _Enemigo.MurosGolpeados++;
            // VERSION COMPLEJA para que rebote con ángulo para superficies que no sean horizontales ni verticales 
            //_Movimiento.DireccionVuelo -= 2 * Vector3.Dot(vectorNormal, _Movimiento.DireccionVuelo) * vectorNormal;
        }
    }
}
