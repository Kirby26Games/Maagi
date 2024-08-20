using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;


public class GestorObjetos : MonoBehaviour
{
    public static GestorObjetos Instancia;
    [SerializeReference] public Dictionary<string, Objeto> DiccionarioObjeto;
    public ListaDeObjetos Objetos;

    private void Awake()
    {
        Instancia = this;
    }

    private void Start()
    {
        CrearObjetos();
    }

    public void CrearObjetos()
    {
        //Forma un diccionario a partir de los objetos añadidos a ListaDeObjetos, accesible desde todos los scripts.
        DiccionarioObjeto = new();

        for (int i = 0; i < Objetos.Objetos.Length; i++)
        {
            DiccionarioObjeto.Add(Objetos.Objetos[i].Nombre, Objetos.Objetos[i]);
        }
    }

}
