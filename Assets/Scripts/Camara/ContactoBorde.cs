using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using static ContactoBorde;

public class ContactoBorde : MonoBehaviour
{
    public enum Direccion { arriba, abajo, izquierda, derecha}
    public Direccion DireccionBorde;
    private Transform _Filtro;
    private Vector3 MovimientoDeCamara;
    private Vector3 ModMovimientoDePersonaje;
    [SerializeField]
    private Vector3 _ModCamara; // Siempre un vector con un -1 o 1 y 0 en el resto de posiciones
    private Vector3 _ModPersonaje;

    private void Start()
    {
        // Define el filtro delante de la cámara para hacer fundido en negro en transiciones entre habitaciones
        _Filtro = Camera.main.transform.GetChild(0).GetChild(0);

        // La cámara se desplaza en x igual al tamaño de una habitación + 2 veces el espacio vacío después de los bordes
        _ModPersonaje = VariablesGlobales.Instancia.TamañoHabitacion + 2 * VariablesGlobales.Instancia.EspacioEntreHabitaciones * Vector3.one;
        _ModPersonaje.z  = 0f;

        DireccionTranslado();
    }

    void DireccionTranslado()
    {
        float modificadorTemporal = 4 * VariablesGlobales.Instancia.EspacioEntreHabitaciones + VariablesGlobales.Instancia.MargenCambioHabitacion;
        switch (DireccionBorde)
        {
            case Direccion.arriba:
                MovimientoDeCamara.y = transform.parent.position.y + 1 * _ModPersonaje.y;
                ModMovimientoDePersonaje.y = 1 * modificadorTemporal;
                break;

            case Direccion.abajo:
                MovimientoDeCamara.y = transform.parent.position.y - 1 * _ModPersonaje.y;
                ModMovimientoDePersonaje.y = -1 * modificadorTemporal;
                break;

            case Direccion.derecha:
                MovimientoDeCamara.x = transform.parent.position.x + 1 * _ModPersonaje.x;
                ModMovimientoDePersonaje.x = 1 * modificadorTemporal;
                break;

            case Direccion.izquierda:
                MovimientoDeCamara.x = transform.parent.position.x - 1 * _ModPersonaje.x;
                ModMovimientoDePersonaje.x = -1 * modificadorTemporal;
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
            ResetFiltro();
        }
    }

    private async void ResetFiltro()
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
