using Sirenix.OdinInspector;
using UnityEngine;

public class AntorchaPuzle : Antorcha
{
    public Puerta PuertaDue�o;

    private void Start()
    {
        PuertaDue�o.AddCount();
    }

    [Button]
    public override void Interactuar()
    {
        base.Interactuar();
        if (!Prendida)
        {
            PuertaDue�o.AddCount();
        }
        else
        {
            PuertaDue�o.RemoveCount();
        }
    }
}
