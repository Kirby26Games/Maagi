using Unity.VisualScripting;
using UnityEngine;

public class MovimientoSimple : MonoBehaviour
{
    public Transform Camara;
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
}
