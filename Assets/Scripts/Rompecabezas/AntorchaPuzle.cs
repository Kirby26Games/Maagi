using Sirenix.OdinInspector;
using UnityEngine;

public class AntorchaPuzle : Antorcha
{
    public Puerta PuertaDueño;

    private void Start()
    {
        PuertaDueño.AddCount();
    }

    [Button]
    public override void Interactuar()
    {
        base.Interactuar();
        if (!Prendida)
        {
            PuertaDueño.AddCount();
        }
        else
        {
            PuertaDueño.RemoveCount();
        }
    }
}
