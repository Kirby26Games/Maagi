using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour
{
    public int Restante;
    int Total;
    public bool Resuelto;
    //temporal
    public GameObject PuertaPuzzle;
    private void Start()
    {
        //ComprobarPuzzle();
    }

    public void AddCount(int Num = 1)
    {
        Total += Num;
        Restante = Total;
    }

    public void RemoveCount(int Num = 1)
    {
        Restante -= Num;
        ComprobarPuzzle();
    }

    void ComprobarPuzzle()
    {
        Debug.Log(Restante);
        if (Restante <= 0)
        {
            Resuelto = true;
            Debug.LogWarning("Abriste la puerta");
            //Hacer algo
            PuertaPuzzle.SetActive(false);
        }
        else
        {
            Resuelto = false;
            PuertaPuzzle.SetActive(true);
            //hacer algo
        }
    }
}
