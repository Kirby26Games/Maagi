using UnityEngine;

public class PuzzlePeso : MonoBehaviour
{
    public Puerta PuertaDueño;
    public int PesoNecesario;

    private void Start()
    {
        PuertaDueño.AddCount(PesoNecesario);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Peso peso))
        {
            //Algo con peso entra aumentar peso
            PuertaDueño.RemoveCount(peso.PesoObj);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Peso peso))
        {
            //Algo con peso sale disminuir peso
            PuertaDueño.RemoveCount(-peso.PesoObj);
        }
    }
}
