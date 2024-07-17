using UnityEngine;

public class ContactoBorde : MonoBehaviour
{
    public Vector3 MovimientoDeCamara;
    public Vector3 ModMovimientoDePersonaje;
    [SerializeField]
    private Vector3 _ModBorde;
    private float _ModGlobal;

    private void Start()
    {
        _ModGlobal = (VariablesGlobales.Instancia.TamañoHabitacion.x + 2 * VariablesGlobales.Instancia.EspacioEntreHabitaciones);
        MovimientoDeCamara.x = transform.parent.position.x + _ModBorde.x * _ModGlobal;
        MovimientoDeCamara.y = transform.parent.position.y + _ModBorde.y * _ModGlobal;

        _ModGlobal = 4 * VariablesGlobales.Instancia.EspacioEntreHabitaciones + VariablesGlobales.Instancia.MargenCambioHabitacion;
        ModMovimientoDePersonaje.x = _ModBorde.x * _ModGlobal;
        ModMovimientoDePersonaje.y = _ModBorde.y * _ModGlobal;
    }
}
