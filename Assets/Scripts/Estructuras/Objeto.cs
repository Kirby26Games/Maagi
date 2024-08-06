using System;
using UnityEngine;
[Serializable]
public struct Objeto
{
    public string Nombre;
    public int MaximoAcumulable;
    
    public void Definir(string nombre = "Objeto")
    {
        Nombre = nombre;
        MaximoAcumulable = 3;
    }
}
