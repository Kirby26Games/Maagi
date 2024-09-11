using System;
using UnityEngine;

public abstract class Clase 
{
    //Clase abstracta para hacer herencias y que cada clase se defina de forma unica
    public int ID;
    public string Nombre;
    public Habilidad[] Habilidades;
    //Las funciones en comun se hacen en ella
    public void DefinirID(int id)
    {
        ID = id;
    }
    public abstract void Definir();
}

[Serializable]
public abstract class Habilidad
{
    //Se puede usar [HideInIspector] si queremos que no se vean pero sean publicas
    public int ID;
    public string Nombre;
    public string Descripcion;
    public int Coste; //Coste de mana
    [SerializeReference] public Efecto[] Efectos;

    public void DefinirID(int id)
    {
        ID = id;
    }
    public abstract void Definir();
    public void Lanzar(SistemasPersonaje personaje)
    {
        //Envia mensaje de usar habilidad 
        Debug.Log("Lanzo " + Nombre);

        personaje.Estadisticas.ManaActual.Modificar(Coste);

        //Paso por todos los efectos y los lanzo
        for (int i = 0; i < Efectos.Length; i++)
        {
            Efectos[i].Lanzar(personaje);
        }
    }
}

[Serializable]
public abstract class Efecto
{
    [HideInInspector] public string Nombre;
    public abstract void Definir();
    public abstract void Lanzar(SistemaBase lanzador);
}

[Serializable]
public abstract class Estadistica
{
    public int Base;
    public int Modificador;
    public int ValorFinal;
    public int Minimo = -3; //Minimo que tiene esta estadistica
    public int Maximo = 10000; //Maximo que tiene esta estadistica
    public string Nombre;
    [HideInInspector] public Estadisticas Estadisticas;
    //Como se calcula esta estadistica
    public abstract void Calcular();

    //Modificar el valor base de la estadistica
    public void Modificar(int cantidad)
    {
        Base += cantidad;
        Base = Mathf.Clamp(Base, Minimo, Maximo);
        Calcular();
    }
    //Calculos base que necesitaremos en el calculo de todas las habilidades
    public int CalculosBase()
    {
        return Base + Modificador;
    }
    //Escalado de la Estadistica dentro de otra, llamaremos a esto en el Calcular() de vida, si queremos
    //por ejemplo que por cada punto de constitucion consigamos 10 de vida, seria Constitucion.Escalado(5)
    public int Escalado(float escalado = 1)
    {
        return Mathf.FloorToInt(ValorFinal * escalado);
    }
}
