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
        // Escalado de las variables
        float dañoInflingido = (float)lanzador.Estadisticas.Ataque.ValorFinal / 2;
        float radioArea = (float)lanzador.Estadisticas.Nivel.ValorFinal / 5 + (float)lanzador.Estadisticas.Constitucion.ValorFinal / 2;
        radioArea = Mathf.Clamp(radioArea, 0f, 10f);
        float alcanceMaximo = (float)lanzador.Estadisticas.Fuerza.ValorFinal / 4 - radioArea;
        alcanceMaximo = Mathf.Clamp(alcanceMaximo, radioArea, 100);
        int duracion = 1;

        // Creación del objeto que aplica el efecto
        AreaEfectoRedondaManager plantilla = GameObject.Instantiate(
            GestorClases.Instancia.PrefabEfectos[0],
            lanzador.transform.position + (lanzador.ControlesBase.PosicionApuntado - lanzador.ControlesBase.PosicionEnPantalla).normalized * alcanceMaximo,
            lanzador.transform.rotation
            )
            .GetComponent<AreaEfectoRedondaManager>();
        plantilla.Radio = radioArea;
        plantilla.Duracion = duracion * 100; // segundos -> ms
        plantilla.Dañar(dañoInflingido);

        // Debug
        Debug.Log(Nombre + " hace " + dañoInflingido + " daño en radio " + radioArea + " a distancia " + alcanceMaximo);

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
        Vector3 direccion = (lanzador.ControlesBase.PosicionEnPantalla - lanzador.ControlesBase.PosicionApuntado).normalized;
        float fuerza = lanzador.Estadisticas.Fuerza.ValorFinal * 3;

        // Efecto
        lanzador.MovimientoBase.AñadirFuerza(direccion, fuerza);

        // Debug
        Debug.Log("Empujado en direccion " + direccion + " con fuerza " + fuerza);
    }
}

[Serializable]
public class Proyectil : Efecto
{
    public override void Definir()
    {
        Nombre = IdiomaEfectos.Proyectil;
    }
    public override void Lanzar(SistemaBase lanzador)
    {
        // Escalado de las variables
        Vector3 direccion = (lanzador.ControlesBase.PosicionApuntado - lanzador.ControlesBase.PosicionEnPantalla).normalized;
        float velocidad = lanzador.Estadisticas.Destreza.ValorFinal * 2;
        float daño = lanzador.Estadisticas.Ataque.ValorFinal * 0.75f;
        // Efecto
        // TODO

        // Debug
        Debug.Log("Lanzado proyectil en dirección " + direccion + " a velocidad " + velocidad + " con daño " + daño);
    }
}