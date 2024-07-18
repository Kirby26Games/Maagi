using UnityEngine;

[RequireComponent (typeof(SistemaMovimiento), typeof(SistemaRaycast))]

public class SistemaPersonaje : MonoBehaviour
{
    //Este script contendra un indice de todos los scripts del personaje
    public SistemaGravedad SistemaGravedad;
    public SistemaRaycast SistemaRaycast;
    public SistemaMovimiento SistemaMovimiento;

    //Creamos una instancia de este script que se puede acceder desde cualquier otro script
    public static SistemaPersonaje Instancia;

    private void Awake()
    {
        if (Instancia != null) //Si ya existe una instancia del personaje, este se detruye.
        {
            Destroy(gameObject);
        }
        else
        {
            Instancia = this;
        }

        //Busco los scripts dentro del personaje
        SistemaGravedad = GetComponent<SistemaGravedad>();
        SistemaMovimiento = GetComponent<SistemaMovimiento>();
        SistemaRaycast = GetComponent<SistemaRaycast>();

    }

}
