//Clase de guerrero
using System;
public class Guerrero : Clase
{
    public override void Definir()
    {
        Habilidades = new Habilidad[]
       {
           //Defino las habilidades, no las llameis HabilidadX, ponedles nombre
            new Habilidad1(),
            new Habilidad2(),
            new AtaqueCargado(),
            new Propulsión(),
            new LanzaHachar(),
       };
        //Defino todos los id y las habilidades
        for (int i = 0; i < Habilidades.Length; i++)
        {
            Habilidades[i].DefinirID(i);
            Habilidades[i].Definir();
        }
        Nombre = IdiomaClases.Guerrero;
    }
}

[Serializable]
public class Habilidad1 : Habilidad
{
    public override void Definir()
    {
        Efectos = new Efecto[]
        {
            GestorClases.Instancia.Efectos[IdiomaEfectos.DañoFisico]
        };
        Coste = 5;
        Nombre = IdiomaHabilidades.Habilidad1;
    }
}
[Serializable]
public class Habilidad2 : Habilidad
{
    public override void Definir()
    {
        Efectos = new Efecto[]
        {
            GestorClases.Instancia.Efectos[IdiomaEfectos.Curacion]
        };
        Coste = 3;
        Nombre = IdiomaHabilidades.Habilidad2;
    }
}
[Serializable]
public class AtaqueCargado: Habilidad
{
    public override void Definir()
    {
        Efectos = new Efecto[]
        {
            GestorClases.Instancia.Efectos[IdiomaEfectos.DañoFisico],
            GestorClases.Instancia.Efectos[IdiomaEfectos.AreaDaño]
        };
        Coste = 5;
        Nombre = IdiomaHabilidades.AtaqueCargado;
    }
}
[Serializable]
public class Propulsión : Habilidad
{
    public override void Definir()
    {
        Efectos = new Efecto[]
        {
            GestorClases.Instancia.Efectos[IdiomaEfectos.Empuje],
            GestorClases.Instancia.Efectos[IdiomaEfectos.AreaDaño]
        };
        Coste = 2;
        Nombre = IdiomaHabilidades.Propulsion;
    }
}

public class LanzaHachar: Habilidad
{
    public override void Definir()
    {
        Efectos = new Efecto[]
        {
            GestorClases.Instancia.Efectos[IdiomaEfectos.Proyectil]
        };
        Coste = 4;
        Nombre = IdiomaHabilidades.LanzarHacha;
    }
}