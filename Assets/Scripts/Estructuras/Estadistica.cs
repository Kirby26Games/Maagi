using System;
using UnityEngine;

[Serializable]
public class Fuerza : Estadistica
{
    public Fuerza() { }
    public override void Calcular()
    {
        Nombre = IdiomaEstadisticas.Fuerza;
        ValorFinal = CalculosBase();
        Estadisticas.Ataque.Calcular();
    }

}
[Serializable]
public class Destreza : Estadistica
{
    public override void Calcular()
    {   
        Nombre = IdiomaEstadisticas.Destreza;
        ValorFinal = CalculosBase();
    }
}
[Serializable]
public class Constitucion : Estadistica
{
    public override void Calcular()
    {
       
        Nombre = IdiomaEstadisticas.Constitucion;
        ValorFinal = CalculosBase();
        Estadisticas.VidaActual.Calcular();
    }
}
[Serializable]
public class Inteligencia : Estadistica
{
    public override void Calcular()
    {
        Nombre = IdiomaEstadisticas.Inteligencia;
        ValorFinal = CalculosBase();
        Estadisticas.Magia.Calcular();
    }
}
[Serializable]
public class Intuicion : Estadistica
{
    public override void Calcular()
    {
       
        Nombre = IdiomaEstadisticas.Intuicion;
        ValorFinal = CalculosBase();
    }
}
[Serializable]
public class Memoria : Estadistica
{
    public override void Calcular()
    {     
        Nombre = IdiomaEstadisticas.Memoria;
        ValorFinal = CalculosBase();
        Estadisticas.ManaActual.Calcular();
    }
}
[Serializable]
public class Carisma : Estadistica
{
    public override void Calcular()
    {      
        Nombre = IdiomaEstadisticas.Carisma;
        ValorFinal = CalculosBase();
    }
}
[Serializable]
public class Ataque : Estadistica
{
    public override void Calcular()
    {    
        Nombre = IdiomaEstadisticas.Ataque;
        ValorFinal = CalculosBase() + Estadisticas.Fuerza.Escalado() + Estadisticas.Nivel.Escalado(1f / 2f);
    }
}
[Serializable]
public class Magia : Estadistica
{
    public override void Calcular()
    {
        Nombre = IdiomaEstadisticas.Magia;
        ValorFinal = CalculosBase() + Estadisticas.Inteligencia.Escalado() + Estadisticas.Nivel.Escalado(1f / 2f);     
    }
}

[Serializable]
public class Nivel : Estadistica
{
    public override void Calcular()
    {
        Nombre = IdiomaEstadisticas.Nivel;
        ValorFinal = CalculosBase();
    }
}

[Serializable]
public class VidaActual : Estadistica
{
    public override void Calcular()
    {
        Nombre = IdiomaEstadisticas.VidaActual;
        Minimo = 0;
        Maximo = Estadisticas.VidaMaxima.ValorFinal;
        Estadisticas.VidaMaxima.Calcular();
        Base = Mathf.Clamp(Base, 0, Estadisticas.VidaMaxima.ValorFinal);
        ValorFinal = Estadisticas.VidaMaxima.ValorFinal - CalculosBase();
        ValorFinal = Mathf.Clamp(ValorFinal, 0, Estadisticas.VidaMaxima.ValorFinal);
    }
}
[Serializable]
public class VidaMaxima : Estadistica
{
    public override void Calcular()
    {
        Nombre = IdiomaEstadisticas.VidaMaxima;
        ValorFinal = 10 + CalculosBase() + Estadisticas.Constitucion.Escalado(5) + Estadisticas.Nivel.Escalado(5);
        if (ValorFinal < 1)
        {
            ValorFinal = 1;
        }

    }
}
[Serializable]
public class ManaActual : Estadistica
{
    public override void Calcular()
    {
        Nombre = IdiomaEstadisticas.ManaActual;
        Estadisticas.ManaMaximo.Calcular();
        Base = Mathf.Clamp(Base, 0, Estadisticas.ManaMaximo.ValorFinal);
        ValorFinal = Estadisticas.ManaMaximo.ValorFinal - CalculosBase();
        ValorFinal = Mathf.Clamp(ValorFinal, 0, Estadisticas.ManaMaximo.ValorFinal);
    }
}
[Serializable]
public class ManaMaximo : Estadistica
{
    public override void Calcular()
    {
        Nombre = IdiomaEstadisticas.ManaMaximo;
        ValorFinal = 10 + CalculosBase() + Estadisticas.Memoria.Escalado(5) + Estadisticas.Nivel.Escalado(5);
    }
}
