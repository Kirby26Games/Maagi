using UnityEngine;

public class Estadisticas : MonoBehaviour
{
    [SerializeReference] public Fuerza Fuerza;
    [SerializeReference] public Destreza Destreza;
    [SerializeReference] public Constitucion Constitucion;
    [SerializeReference] public Inteligencia Inteligencia;
    [SerializeReference] public Intuicion Intuicion;
    [SerializeReference] public Carisma Carisma;
    [SerializeReference] public Memoria Memoria;
    [SerializeReference] public Ataque Ataque;
    [SerializeReference] public Magia Magia;
    [SerializeReference] public VidaActual VidaActual;
    [SerializeReference] public VidaMaxima VidaMaxima;
    [SerializeReference] public ManaActual ManaActual;
    [SerializeReference] public ManaMaximo ManaMaximo;
    [SerializeReference] public Nivel Nivel;

    private void Awake()
    {
        //Esto se puede mejorar haciendo un constructor
        Fuerza = new Fuerza();
        Fuerza.Estadisticas = this;
        Destreza = new Destreza();
        Destreza.Estadisticas = this;
        Constitucion = new Constitucion();
        Constitucion.Estadisticas = this;
        Inteligencia = new Inteligencia();
        Inteligencia.Estadisticas = this;
        Intuicion = new Intuicion();
        Intuicion.Estadisticas = this;
        Carisma = new Carisma();
        Carisma.Estadisticas = this;
        Memoria = new Memoria();
        Memoria.Estadisticas = this;
        Ataque = new Ataque();
        Ataque.Estadisticas = this;
        Magia = new Magia();
        Magia.Estadisticas = this;
        VidaActual = new VidaActual();
        VidaActual.Estadisticas = this;
        VidaMaxima = new VidaMaxima();
        VidaMaxima.Estadisticas = this;
        ManaActual = new ManaActual();
        ManaActual.Estadisticas = this;
        ManaMaximo = new ManaMaximo();
        ManaMaximo.Estadisticas = this;
        Nivel = new Nivel();
        Nivel.Estadisticas = this;
    }
}
