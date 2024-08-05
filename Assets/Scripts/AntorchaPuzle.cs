using UnityEngine;

public class AntorchaPuzle : Antorcha
{
    public AntorchaPuerta PuertaAbrir;

    private void Start()
    {
        PuertaAbrir.TotalAntorchas(this);
    }
    public override void Interactuar()
    {
        base.Interactuar();
        PuertaAbrir.ComprobarAntorchas();
    }
}
