using System.Collections.Generic;
using UnityEngine;

public class InventarioEnemigo : InventarioBase
{
    private void Start()
    {
        // Carga el inventario del enemigo
        // ObjetosInventario = new List<Objeto>(new Objeto[VariablesGlobales.Instancia.TamañoInventario]);
    }

    public bool AgregarAInventario(Objeto objetoTrigger)
    {
        for (int i = 0; i < ObjetosInventario.Count; i++)
        {
            if (ObjetosInventario[i].Nombre == null)
            {
                ObjetosInventario[i] = objetoTrigger;
                return true;
            }
        }

        return false;
    }
}
