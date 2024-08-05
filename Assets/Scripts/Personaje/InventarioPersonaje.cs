using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventarioPersonaje : InventarioBase
{
    public GameObject InventarioInterfaz;
    public List<ContenedorObjeto> ContenedorObjetos;
    public Vector3 PosicionOculta;
    public Vector3 PosicionAbierta;

    private void Start()
    {
        ObjetosInventario = new List<Objeto>(new Objeto[ContenedorObjetos.Count]);
        PosicionOculta = InventarioInterfaz.transform.position;
    }

    public void ToggleInterfaz()
    {
        if (InventarioInterfaz.transform.position == PosicionAbierta)
        {
            ActivarInterfaz(PosicionOculta);
        }
        else
        {
            ActivarInterfaz(PosicionAbierta);
            ActualizarInterfaz();
        }

    }

    public void ActivarInterfaz(Vector3 posicion) 
    {
        InventarioInterfaz.transform.position = posicion;
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
