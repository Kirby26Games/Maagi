using UnityEngine;

public class ColisionesEnemigo : MonoBehaviour
{
    private MovimientoEnemigo _Movimiento;
    private InventarioEnemigo _Inventario;

    private void Awake()
    {
        _Movimiento = GetComponent<MovimientoEnemigo>();
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
}
