using UnityEngine;

public class SistemaGravedad : MonoBehaviour
{
    public float Gravedad = -9.82f;
    public bool EnSuelo;
    public float EjeY;
    private float LimiteVelocidadCaida = -20;

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
        }
        else if (EjeY > LimiteVelocidadCaida)
        {
            EjeY += Gravedad * Time.deltaTime;
        }
    }
}
