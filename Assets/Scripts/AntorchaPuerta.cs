using System.Collections.Generic;
using UnityEngine;

public class AntorchaPuerta : MonoBehaviour
{
    public int AntorchasEncendidas;
    public bool Resuelto;
    List<AntorchaPuzle> Antorchas = new();
    //temporal
    public GameObject Puerta;
    private void Start()
    {
        ComprobarPuzzle();
    }
    public void TotalAntorchas(AntorchaPuzle nt)
    {
        Antorchas.Add(nt);
        if (Resuelto)
        {
            //algo lista
        }
    }

    public void ComprobarAntorchas()
    {
        AntorchasEncendidas = 0;
        for (int i = 0; i < Antorchas.Count; i++)
        {
            if (Antorchas[i].Prendida == true)
            {
                AntorchasEncendidas++;
            }
        }
        ComprobarPuzzle();
    }

    void ComprobarPuzzle()
    {
        Debug.Log(Antorchas.Count);
        if (AntorchasEncendidas >= Antorchas.Count)
        {
            Resuelto = true;
            Debug.LogWarning("Resolviste las Antorchas");
            //Hacer algo
            Puerta.SetActive(false);
        }
        else
        {
            Resuelto = false;
            Puerta.SetActive(true);
            //hacer algo
        }
    }
}
