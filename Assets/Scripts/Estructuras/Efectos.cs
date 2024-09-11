using System;
using UnityEngine;

[Serializable]
public class DañoFisico : Efecto
{
    public override void Definir()
    {
        Nombre = IdiomaEfectos.DañoFisico;
    }

    //Aqui habra que cambair sistemaspersonaje por algo que tenga todo lo que pueda lanzar habilidades, SistemasBase
    public override void Lanzar(SistemaBase lanzador)
    {
        //Aqui puedes meter todo el codigo que quieras.
        float dañoInflingido = lanzador.Estadisticas.Ataque.ValorFinal;
        Debug.Log(Nombre + " hace " + dañoInflingido + " daño.");
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
public class AreaDañoRedonda: Efecto
{
    public override void Definir()
    {
        Nombre = IdiomaEfectos.AreaDaño;
    }
    public override void Lanzar(SistemaBase lanzador)
    {
        float dañoInflingido = (float)lanzador.Estadisticas.Ataque.ValorFinal / 2;
        float radioArea = (float)lanzador.Estadisticas.Nivel.ValorFinal / 5 + (float)lanzador.Estadisticas.Constitucion.ValorFinal / 2;
        radioArea = Mathf.Clamp(radioArea,0f,10f);
        int duracion = 1;

        AreaEfectoRedondaManager plantilla = GameObject.Instantiate(
            GestorClases.Instancia.PlantillasEfectos[0],
            lanzador.transform.position + (lanzador.ControlesBase.PosicionApuntado - lanzador.ControlesBase.PosicionEnPantalla).normalized,
            lanzador.transform.rotation
            )
            .GetComponent<AreaEfectoRedondaManager>();
        Debug.Log(plantilla.transform.position);
        plantilla.Radio = radioArea;
        plantilla.Duracion = duracion * 100; // segundos -> ms
        plantilla.Dañar(dañoInflingido);

        Debug.Log(Nombre + " hace " + dañoInflingido + " daño en radio " + radioArea);

        plantilla.Destruir();
    }
}