using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MovimientoSimple : MonoBehaviour
{
    public float Velocidad;

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
}
