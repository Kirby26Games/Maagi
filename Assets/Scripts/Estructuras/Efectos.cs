using System;
using UnityEngine;

[Serializable]
public class Da�oFisico : Efecto
{
    public override void Definir()
    {
        Nombre = IdiomaEfectos.Da�oFisico;
    }

    //Aqui habra que cambair sistemaspersonaje por algo que tenga todo lo que pueda lanzar habilidades, SistemasBase
    public override void Lanzar(SistemaBase lanzador)
    {
        //Aqui puedes meter todo el codigo que quieras.
        float da�oInflingido = lanzador.Estadisticas.Ataque.ValorFinal;
        Debug.Log(Nombre + " hace " + da�oInflingido + " da�o.");
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
    public override void Lanzar(SistemaBase lanzador)
    {
        //Este -10 deberia ser un calculo que tenga en cuenta una estadistica
        lanzador.Estadisticas.VidaActual.Modificar(-10);
        //Aqui puedes meter todo el codigo que quieras.
    }
}

[Serializable]
public class AreaDa�oRedonda: Efecto
{
    public override void Definir()
    {
        Nombre = IdiomaEfectos.AreaDa�o;
    }
    public override void Lanzar(SistemaBase lanzador)
    {
        // Escalado de las variables
        float da�oInflingido = (float)lanzador.Estadisticas.Ataque.ValorFinal / 2;
        float radioArea = (float)lanzador.Estadisticas.Nivel.ValorFinal / 5 + (float)lanzador.Estadisticas.Constitucion.ValorFinal / 2;
        radioArea = Mathf.Clamp(radioArea, 0f, 10f);
        float alcanceMaximo = (float)lanzador.Estadisticas.Fuerza.ValorFinal / 4 - radioArea;
        alcanceMaximo = Mathf.Clamp(alcanceMaximo, radioArea, 100);
        int duracion = 1;

        // Creaci�n del objeto que aplica el efecto
        AreaEfectoRedondaManager plantilla = GameObject.Instantiate(
            GestorClases.Instancia.PlantillasEfectos[0],
            lanzador.transform.position + (lanzador.ControlesBase.PosicionApuntado - lanzador.ControlesBase.PosicionEnPantalla).normalized * alcanceMaximo,
            lanzador.transform.rotation
            )
            .GetComponent<AreaEfectoRedondaManager>();
        plantilla.Radio = radioArea;
        plantilla.Duracion = duracion * 100; // segundos -> ms
        plantilla.Da�ar(da�oInflingido);

        // Debug
        Debug.Log(Nombre + " hace " + da�oInflingido + " da�o en radio " + radioArea + " a distancia " + alcanceMaximo);

        // Limpiar escena
        plantilla.Destruir();
    }
}

[Serializable]
public class Empuje : Efecto
{
    public override void Definir()
    {
        Nombre = IdiomaEfectos.Empuje;
    }
    public override void Lanzar(SistemaBase lanzador)
    {
        // Escalado de las variables

        // Debug
    }
}