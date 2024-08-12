using System;
using UnityEngine;
[Serializable]
public abstract class Objeto
{
    public string Nombre;
    public int ID;
    public int MaximoAcumulable = 1;
    public ObjetoEscena Prefab;
    public Sprite Imagen;

    public abstract void Definir();

    public void DefinirID(int id)
    {
        ID = id;
    }

    public void DefinirObjeto()
    {
        if (Prefab != null) 
        { 
            Prefab.ID = ID;
            Prefab.Nombre = Nombre;
        }
    }
}

[Serializable]
public class Moneda : Objeto
{
    public override void Definir()
    {
        Nombre = IdiomaObjetos.Moneda;
        MaximoAcumulable = 9999;
    }
}

[Serializable]
public class EspadaCorta : Objeto
{
    public override void Definir()
    {
        Nombre = IdiomaObjetos.EspadaCorta;
        MaximoAcumulable = 1;
    }
}

[Serializable]
public class Antorcha : Objeto
{
    public override void Definir()
    {
        Nombre = IdiomaObjetos.Antorcha;
        MaximoAcumulable = 1;
    }
}

[Serializable]
public class ObjetoVacio : Objeto
{
    public override void Definir()
    {
        Nombre = IdiomaObjetos.ObjetoVacio;
        MaximoAcumulable = 1;
    }
}