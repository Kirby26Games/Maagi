using UnityEngine;

public class MovimientoSimple : MonoBehaviour
{
    public Transform Camara;
    public float Velocidad;
    void Update()
    {
        if(Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * Time.deltaTime * Velocidad;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * Time.deltaTime * Velocidad;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ContactoBorde contactoTemporal = other.GetComponent<ContactoBorde>();
        if (contactoTemporal != null)
        {
            Camara.position = contactoTemporal.MovimientoDeCamara;
            transform.position += contactoTemporal.ModMovimientoDePersonaje;
        }
    }
}
