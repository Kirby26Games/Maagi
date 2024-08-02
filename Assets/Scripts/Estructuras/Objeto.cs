using System;
using UnityEngine;
[Serializable]
public struct Objeto
{
    public string Nombre;
    
    public void Definir(string nombre = "Objeto")
    {
        Nombre = nombre;
    }
}
