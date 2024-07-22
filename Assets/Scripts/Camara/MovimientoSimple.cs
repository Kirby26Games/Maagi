using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MovimientoSimple : MonoBehaviour
{
    public Transform Camara;
    private Transform _Filtro;
    public float Velocidad;

    private void Start()
    {
        // Define el filtro delante de la cámara para hacer fundido en negro en transiciones entre habitaciones
        _Filtro = Camara.GetChild(0).GetChild(0);
    }
    void Update()
    {
        // Sistema de controles OBSOLETO
        if(Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * Time.deltaTime * Velocidad;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * Time.deltaTime * Velocidad;
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up * Time.deltaTime * Velocidad;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detecta el contacto con el borde de una habitación y cambia la cámara, el personaje y ejecuta la transición
        ContactoBorde bordeTemporal = other.GetComponent<ContactoBorde>();
        if (bordeTemporal != null)
        {
            Camara.position = bordeTemporal.MovimientoDeCamara;
            transform.position += bordeTemporal.ModMovimientoDePersonaje;
            ResetFiltro();
        }

        // Cambia el efecto de la gravedad sobre el personaje al tocar una escalera
        EscaleraSimple escaleraTemporal = other.GetComponent<EscaleraSimple>();
        if (escaleraTemporal != null)
        {
            GetComponent<Rigidbody>().useGravity = false;
            // Reinicia el movimiento en caso de estar cayendo
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Cambia el efecto de la gravedad sobre el personaje al dejar de tocar una escalera
        EscaleraSimple escaleraTemporal = other.GetComponent<EscaleraSimple>();
        if (escaleraTemporal != null)
        {
            GetComponent<Rigidbody>().useGravity = true;
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
