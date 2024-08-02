using UnityEngine;

public class PuzzlePeso : MonoBehaviour
{
    public float PesoNecesario;
    float _PesoActual;
    public bool Resuelto;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Peso peso))
        {
            //Algo con peso entra aumentar peso
            _PesoActual += peso.PesoObj;
            ComprobarPeso();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Peso peso))
        {
            //Algo con peso sale disminuir peso
            _PesoActual -= peso.PesoObj;
            ComprobarPeso();
        }
    }

    void ComprobarPeso()
    {
        //es un if que comprueba peso
        Resuelto = (_PesoActual >= PesoNecesario);
        if (Resuelto)
        {
            //Hacer algo
        }
    }
}
