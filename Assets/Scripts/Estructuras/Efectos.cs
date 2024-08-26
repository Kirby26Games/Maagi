using System;

[Serializable]
public class DañoFisico : Efecto
{
    public override void Definir()
    {
        Nombre = IdiomaEfectos.DañoFisico;
    }

    //Aqui habra que cambair sistemaspersonaje por algo que tenga todo lo que pueda lanzar habilidades, SistemasBase
    public override void Lanzar(SistemasPersonaje lanzador)
    {
        //Aqui puedes meter todo el codigo que quieras.
    }
}

[Serializable]
public class Curacion : Efecto
{
    public override void Definir()
    {
        Nombre = IdiomaEfectos.Curacion;
    }

    //Aqui habra que cambair sistemaspersonaje por algo que tenga todo lo que pueda lanzar habilidades, SistemasBase
    public override void Lanzar(SistemasPersonaje lanzador)
    {
        //Este -10 deberia ser un calculo que tenga en cuenta una estadistica
        lanzador.Estadisticas.VidaActual.Modificar(-10);
        //Aqui puedes meter todo el codigo que quieras.
    }
}