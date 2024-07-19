using UnityEngine;

public class SistemaGravedad : MonoBehaviour
{
    //Constantes de la gravedad
    public float gravedad = -9.82f;
    public float limiteGravedad = -20;

    void Update()
    {
        ComprobacionSuelo();
    }

    public void ComprobacionSuelo()
    {
        //Si esta en el suelo y su velocidad hacia arriba es menor a 0
        if (SistemaPersonaje.Instancia.SistemaRaycast.EnSuelo == true && SistemaPersonaje.Instancia.SistemaMovimiento.ejeY < 0)
        {
            //Ponemos un poco de velocidad negativa para asegurar que este unido al suelo
            SistemaPersonaje.Instancia.SistemaMovimiento.ejeY = -0.01f;
        }
        else
        {
            //Evita que la velocidad de caida aumente indefinidamente cada frame
            if (SistemaPersonaje.Instancia.SistemaMovimiento.ejeY <= limiteGravedad)
            {
                return;
            }
            //Si supera el limite de gravedad, aumentamos la velocidad de caida exponencialmente
            SistemaPersonaje.Instancia.SistemaMovimiento.ejeY += Time.deltaTime * gravedad;
        }
    }

}
