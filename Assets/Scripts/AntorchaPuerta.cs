using System.Collections.Generic;
using UnityEngine;

public class AntorchaPuerta : MonoBehaviour
{
    public int AntorchasNecesarias;
    public bool Resuelto;
    public List<AntorchaPuzle> Antorchas;

    private void Awake()
    {
        AntorchasNecesarias = 0;
    }
    private void Start()
    {
        if (Resuelto)
        {
            // algo
        }
    }
    public void TotalAntorchasNecesarias(AntorchaPuzle Antorcha)
    {
        AntorchasNecesarias += 1;
        Antorchas.Add(Antorcha);
        if (Resuelto)
        {
            //algo lista
        }
    }

    public void ComprobarAntorchas(AntorchaPuzle Antorcha)
    {
        if (Antorcha.Encendida)
        {
            AntorchasNecesarias -= 1;
        }
        else
        {
            AntorchasNecesarias += 1;
        }

        ComprobarPuzzle();
    }

    void ComprobarPuzzle()
    {
        if (AntorchasNecesarias < 1)
        {
            Resuelto = true;
            Debug.LogWarning("Resolviste las Antorchas");
            //Hacer algo
        }
        else
        {
            Resuelto = false;
            //hacer algo
        }
    }
}
