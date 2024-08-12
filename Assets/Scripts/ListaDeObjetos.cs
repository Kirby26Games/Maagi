using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ListaDeObjetos", menuName = "Creaciones/ListaDeObjetos", order = 0)]

public class ListaDeObjetos : ScriptableObject
{
    [SerializeReference] public Objeto[] Objetos;

    //Lista de Objetos es donde todos los objetos en escena se agregan.

    //COMO AGREGAR OBJETOS
    //1. Crear Prefab del objeto con ObjetoEscena(script) a�adido
    //2. Ir a Objetos(script) y a�adir una clase nueva de tu objeto. Recordar modificar su nombre en Idioma(script) y poner su MaximoAcumulable
    //3. En ListaDeObjetos(scriptableObject) a�adirlo en la lista de Objetos en CrearObjetos
    //4. Presionar el boton CrearObjetos en el inspector, a�adir el Prefab de tu objeto y presionar el boton DefinirObjeto

    [Button]
    public void CrearObjetos()
    {
        Objetos = new Objeto[] {
            new ObjetoVacio(),
            new Moneda(),
            new Antorcha(),
            new EspadaCorta(),
        };


        for (int i = 0; i < Objetos.Length; i++)
        {
            Objetos[i].Definir();
            Objetos[i].DefinirID(i);
        }
    }

    [Button]
    public void DefinirObjeto()
    {
        for (int i = 0; i < Objetos.Length; i++)
        {
            Objetos[i].DefinirObjeto();
        }
    }
}
