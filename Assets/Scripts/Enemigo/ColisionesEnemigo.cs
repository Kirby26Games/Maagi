using Unity.VisualScripting;
using UnityEngine;

public class ColisionesEnemigo : MonoBehaviour
{
    private MovimientoEnemigo _Movimiento;
    private InventarioEnemigo _Inventario;

    private void Awake()
    {
        // Guarda los scripts necesarios
        _Movimiento = GetComponent<MovimientoEnemigo>();

    }

    private void OnTriggerEnter(Collider other)
    {
        // Gestiona la detección de escaleras
        if (other.TryGetComponent(out Escaleras escaleras))
        {
            _Movimiento.CercaEscalera = true;
            _Movimiento.PosicionEscalera = escaleras.transform.position.x;
            _Movimiento.VelocidadSubirEscaleras = escaleras.VelocidadSubirEscalera;
        }

        // Gestiona la detección de objetos
        if (other.TryGetComponent(out ObjetoEscena objetoEscena))
        {
            if (_Inventario.AgregarAInventario(objetoEscena.Objeto))
            {
                Destroy(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Gestiona la detección de escaleras
        if (other.TryGetComponent(out Escaleras escaleras))
        {
            _Movimiento.CercaEscalera = false;
            _Movimiento.SoltarEscalera();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Si no es volador, no es necesario el siguiente código
        if(!_Movimiento.EsVolador)
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
                _Movimiento.DireccionVuelo.x = vectorNormal.x;
            }
            if (Mathf.Abs(vectorNormal.y) > 0.5f)
            {
                _Movimiento.DireccionVuelo.y = vectorNormal.y;
            }
            _Movimiento.MurosGolpeados++;
            // VERSION COMPLEJA para que rebote con ángulo para superficies que no sean horizontales ni verticales 
            //_Movimiento.DireccionVuelo -= 2 * Vector3.Dot(vectorNormal, _Movimiento.DireccionVuelo) * vectorNormal;
        }
    }
}
