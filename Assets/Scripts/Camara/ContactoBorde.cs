using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ContactoBorde : MonoBehaviour
{
    public enum Direccion { arriba,abajo,derecha,izquierda}
    public Direccion direccion;
    Transform _Filtro;
    Vector3 MovimientoDeCamara;
    Vector3 ModMovimientoDePersonaje;
    private Vector3 _ModCamara;
    private float _ModPersonaje;

    private void Start()
    {
        // Define el filtro delante de la cámara para hacer fundido en negro en transiciones entre habitaciones
        _Filtro = Camera.main.transform.GetChild(0).GetChild(0);

        // La cámara se desplaza en x/y igual al tamaño de una habitación + 2 veces el espacio vacío después de los bordes
        _ModCamara = (VariablesGlobales.Instancia.TamañoHabitacion + (Vector3.one * VariablesGlobales.Instancia.MargenCambioHabitacion) * VariablesGlobales.Instancia.EspacioEntreHabitaciones);

        // El personaje se debe los a través de los 2 bordes + 2 espacio vacío después del borde + un margen de maniobra
        _ModPersonaje = 4 * VariablesGlobales.Instancia.EspacioEntreHabitaciones + VariablesGlobales.Instancia.MargenCambioHabitacion;

        DireccionTranslado();
    }

    void DireccionTranslado()
    {
        switch (direccion)
        {
            case Direccion.arriba:
                MovimientoDeCamara.y = transform.parent.position.y + 1 * _ModCamara.y;
                ModMovimientoDePersonaje.y = 1 * _ModPersonaje;
                break;

            case Direccion.abajo:
                MovimientoDeCamara.y = transform.parent.position.y -1 * _ModCamara.y;
                ModMovimientoDePersonaje.y = -1 * _ModPersonaje;
                break;

            case Direccion.derecha:
                MovimientoDeCamara.x = transform.parent.position.x + 1 * _ModCamara.x;
                ModMovimientoDePersonaje.x = 1 * _ModPersonaje;
                break;

            case Direccion.izquierda:
                MovimientoDeCamara.x = transform.parent.position.x -1 * _ModCamara.x;
                ModMovimientoDePersonaje.x = -1 * _ModPersonaje;
                break;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detecta el contacto con el jugador y cambia su posicion y la de la cámara y ejecuta la transición
        if (other.TryGetComponent(out SistemasPersonaje _))
        {
            //mueve camara
            Camera.main.transform.position = MovimientoDeCamara;
            //mueve personaje
            other.transform.root.position += ModMovimientoDePersonaje;
            _ResetFiltro();
        }
    }

    //esto debe separarse en otro codigo "Esto para despues/mas adelante"
    private async void _ResetFiltro()
    {
        Image imagenFiltro = _Filtro.GetComponent<Image>();
        // Reinicia el temporizador
        float temporizador = 0f;
        // Durante el rango que tiene Lerp...
        while (temporizador < 1f)
        {
            // ...cambia la opacidad del filtro...
            imagenFiltro.color = new Color(imagenFiltro.color.r, imagenFiltro.color.g, imagenFiltro.color.b, Mathf.Lerp(1f, 0f, temporizador));
            // ...en tiempo igual al definido en variables globales...
            temporizador += Time.deltaTime / VariablesGlobales.Instancia.TiempoCambioHabitacion;
            // ...una vez cada frame.
            await Task.Yield();
        }
    }
}
