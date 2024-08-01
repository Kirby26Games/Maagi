using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventarioPersonaje : InventarioBase
{
    public GameObject InventarioInterfaz;
    public List<ContenedorObjeto> ContenedorObjetos;

    private void Start()
    {
        ObjetosInventario = new List<Objeto>(new Objeto[ContenedorObjetos.Count]);
    }

    public void ToggleInterfaz()
    {
        if (InventarioInterfaz.activeSelf)
        {
            InventarioInterfaz.SetActive(false);
        }
        else
        {
            InventarioInterfaz.SetActive(true);
            ActualizarInterfaz();
        }

    }

    public void ActualizarInterfaz() 
    {
        for (int i = 0; i < ObjetosInventario.Count; i++)
        {
            ContenedorObjetos[i].Objeto = ObjetosInventario[i];
            ContenedorObjetos[i].Actualizar();
        }
    }
}
