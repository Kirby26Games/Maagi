using UnityEngine;

public class PuzzlePeso : MonoBehaviour
{
    public Puerta PuertaDue�o;
    public int PesoNecesario;

    private void Start()
    {
        PuertaDue�o.AddCount(PesoNecesario);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Peso peso))
        {
            //Algo con peso entra aumentar peso
            PuertaDue�o.RemoveCount(peso.PesoObj);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Peso peso))
        {
            //Algo con peso sale disminuir peso
            PuertaDue�o.RemoveCount(-peso.PesoObj);
        }
    }
}
