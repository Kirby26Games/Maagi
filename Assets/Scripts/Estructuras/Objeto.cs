using System;
using UnityEngine;
[Serializable]
public class Objeto
{
    public string Nombre;
    
    public void Definir(string nombre = "Objeto")
    {
        Nombre = nombre;
    }
}
