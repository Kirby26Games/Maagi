using UnityEngine;

public class SistemaRaycast : MonoBehaviour
{
    public bool EnSuelo;
    RaycastHit Datos; //Datos que devuelve el raycast
    public float Tamaño; //Altura del personaje
    public float Ancho; //Ancho del personaje

    void Start()
    {
        CalcularTamano();
    }

    void Update()
    {
        DeteccionSuelo();
    }

    public void DeteccionSuelo() 
    {
        if (Physics.SphereCast(transform.position, Ancho / 2, Vector3.down, out Datos, Tamaño / 4 + 0.001f))
        {
            Debug.DrawRay(transform.position, Vector3.down * (Datos.distance + Ancho / 2), Color.red);
            EnSuelo = true;
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.down * (Tamaño / 2 + 0.001f), Color.blue);
            EnSuelo = false;
        }
    }

    public void CalcularTamano()
    {
        //Consiguiendo los limites de colision del personaje
        Tamaño = GetComponent<Collider>().bounds.size.y;
        Ancho = GetComponent<Collider>().bounds.size.x;
    }
}
