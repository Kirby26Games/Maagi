using System.Collections.Generic;
using UnityEngine;

public class GestorClases : MonoBehaviour
{
    public static GestorClases Instancia;
[SerializeReference] public Clase[] Clases;
private Efecto[] EfectosBase;
[SerializeReference] public Dictionary<string, Efecto> Efectos;


private void Awake()
{
    Instancia = this;
}
void Start()
{
    CrearEfectos();
    CrearClases();
}

public void CrearClases()
{
    Clases = new Clase[] {
        new Guerrero(),
        
        //a�ado las que quiera aqui con ,
        };
    for (int i = 0; i < Clases.Length; i++)
    {
        Clases[i].DefinirID(i + 1);
        Clases[i].Definir();
    }
}
public void CrearEfectos()
{
    EfectosBase = new Efecto[]
    {
            new Curacion(),
            new Da�oFisico(),
            //a�ado los que quiera igual
    };
    Efectos = new();
    for (int i = 0; i < EfectosBase.Length; i++)
    {
        EfectosBase[i].Definir();
            //a�ado los objetos al diccionario de efectos
        Efectos.Add(EfectosBase[i].Nombre, EfectosBase[i]);
    }
}
public Efecto ConseguirEfecto(string id)
{
    return Efectos[id];
}
}