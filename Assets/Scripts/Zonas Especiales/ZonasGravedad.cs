using UnityEngine;

public class ZonasGravedad : MonoBehaviour
{
    private Quaternion RotacionObjetivo;

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
