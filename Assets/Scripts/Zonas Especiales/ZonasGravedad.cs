using UnityEngine;

public class ZonasGravedad : MonoBehaviour
{
    private Quaternion RotacionObjetivo;
    private int VelocidadRotacion = 3;

    public void CalcularCambioGravedad(GameObject objeto)
    {
        RotacionObjetivo = transform.rotation;

        RotarPersonaje(objeto);

    }

    private void RotarPersonaje(GameObject objeto)
    {
        objeto.transform.rotation = RotacionObjetivo;
    }
    

}
