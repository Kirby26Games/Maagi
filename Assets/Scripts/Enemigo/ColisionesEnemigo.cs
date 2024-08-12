using Unity.VisualScripting;
using UnityEngine;

public class ColisionesEnemigo : MonoBehaviour
{
    private MovimientoEnemigo _Movimiento;
    private InventarioEnemigo _Inventario;
    private Collision _ComprobadorColision;
    private float _TiempoColision;

    private void Awake()
    {
        _Movimiento = GetComponent<MovimientoEnemigo>();
    }

    private void Update()
    {
        // TODO: Esto no debería pasar
        if(_ComprobadorColision != null)
        {
            if(_TiempoColision > .5f)
            {
                EspejoAngulo(_ComprobadorColision);
                _TiempoColision = 0f;
                _ComprobadorColision = null;
                Debug.Log("Helper helped");
            }
            else
            {
                _TiempoColision += Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Escaleras escaleras))
        {
            _Movimiento.CercaEscalera = true;
            _Movimiento.PosicionEscalera = escaleras.transform.position.x;
            _Movimiento.VelocidadSubirEscaleras = escaleras.VelocidadSubirEscalera;
        }

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
        if (other.TryGetComponent(out Escaleras escaleras))
        {
            _Movimiento.CercaEscalera = false;
            _Movimiento.SoltarEscalera();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // TODO: Detectar cuando choque contra un muro o cualquier otro objeto valido
        if (collision.gameObject.GetComponent<SistemasPersonaje>() == null)
        {
            _ComprobadorColision = collision;
            EspejoAngulo(collision);
        }
    }

    private void EspejoAngulo(Collision collision)
    {
        Vector3 vectorNormal = collision.GetContact(0).normal.normalized;
        _Movimiento.DireccionVuelo -= 2 * Vector3.Dot(vectorNormal, _Movimiento.DireccionVuelo) * vectorNormal;
    }

    private void OnCollisionExit(Collision collision)
    {
        if(_ComprobadorColision == collision)
        {
            _ComprobadorColision = null;
            _TiempoColision = 0f;
        }
    }
}
