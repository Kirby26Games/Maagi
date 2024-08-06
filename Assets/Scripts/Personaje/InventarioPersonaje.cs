using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventarioPersonaje : InventarioBase
{
    public GameObject InventarioInterfaz;
    public List<ContenedorObjeto> ContenedorObjetos;
    public Vector3 PosicionOculta;
    public Vector3 PosicionAbierta;
    public bool InterfazAbierta;

    private void Start()
    {
        ObjetosInventario = new List<Objeto>(new Objeto[ContenedorObjetos.Count]);
        PosicionOculta = Vector3.up * Screen.height * 4;
        InventarioInterfaz.transform.localPosition = PosicionOculta;
    }

    private void Update()
    {
        ActualizarInterfaz();
    }

    public void ToggleInterfaz()
    {
        if (InterfazAbierta)
        {
            InventarioInterfaz.transform.localPosition = PosicionOculta;
            InterfazAbierta = false;
        }
        else
        {
            InventarioInterfaz.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
            InterfazAbierta = true;
        }

    }

    public bool AgregarAInventario(Objeto objetoTrigger)
    {
        for (int i = 0; i < ObjetosInventario.Count; i++)
        {
            if (ObjetosInventario[i].Nombre == objetoTrigger.Nombre)
            {
                int number = int.Parse(ContenedorObjetos[i].Cantidad.text);
                number++;

                if (number <= objetoTrigger.MaximoAcumulable)
                {
                    ContenedorObjetos[i].Cantidad.text = number.ToString();
                    return true;
                }
                else
                {
                    continue;
                }

            }
        }

        for (int i = 0; i < ObjetosInventario.Count; i++)
        {
            if (ObjetosInventario[i].Nombre == null)
            {
                ObjetosInventario[i] = objetoTrigger;
                int number = 1;
                ContenedorObjetos[i].Cantidad.text = number.ToString();
                return true;
            }
        }

        return false;
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
