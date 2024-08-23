using System.Collections.Generic;
using UnityEngine;

public class InventarioBase : MonoBehaviour
{
    [SerializeReference] public List<int> ObjetosInventario;
    public int _ObjetosMaximos = 16;



    //Crea una lista vacia de los objetos de inventario
    public void CrearInventario()
    {
        for (int i = 0; i < _ObjetosMaximos; i++)
        {
            ObjetosInventario.Add(0);
        }   
    }
}
