using UnityEngine;

public class SistemaGravedad : MonoBehaviour
{
    public float Gravedad = -9.82f;
    public bool EnSuelo;
    public float EjeY;
    public float LimiteVelocidadCaida = -20;
    public Vector3 DireccionGravedad;

    // Update is called once per frame
    void Update()
    {
        CalcularGravedad();
    }
    public void CalcularGravedad()
    {
        if (EnSuelo && EjeY <= 0)
        {
            EjeY = 0;
            DireccionGravedad = transform.up * EjeY;
        }
        else if (EjeY > LimiteVelocidadCaida)
        {
            EjeY += Gravedad * Time.deltaTime;
            DireccionGravedad = transform.up * EjeY;
        }
    }
}
