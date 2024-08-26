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