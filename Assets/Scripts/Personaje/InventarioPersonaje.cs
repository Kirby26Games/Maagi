using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventarioPersonaje : InventarioBase
{
    public GameObject InventarioInterfaz;
    public List<ContenedorObjeto> ContenedorObjetos;
    public List<ObjetoEscena> ObjetosCogibles;
    public GameObject MenuInventario;
    private Vector3 _PosicionOculta;
    public bool InterfazAbierta;
    public string ObjetoSeleccionadoNombre;
    public int ContenedorSeleccionado;

    private void Start()
    {
        //Crea una lista vacia de los objetos de inventario
        CrearInventario();
        _PosicionOculta = Vector3.up * Screen.height * 4;
        InventarioInterfaz.transform.localPosition = _PosicionOculta;
        MenuInventario.SetActive(false);
    }

    private void Update()
    {
        ActualizarInterfaz();
    }

    public void ActualizarInterfaz()
    {
        //Por cada objeto a�adido al inventario, el ID del objeto se le pasa al contenedor
        for (int i = 0; i < ObjetosInventario.Count; i++)
        {
            ContenedorObjetos[i].ID = ObjetosInventario[i];
            ContenedorObjetos[i].Actualizar();
        }
    }

    public void GestorObjetosCogibles(ObjetoEscena objetoTrigger)
    {
        if (ObjetosCogibles.Contains(objetoTrigger))
        {
            ObjetosCogibles.Remove(objetoTrigger);
        }
        else
        {
            ObjetosCogibles.Add(objetoTrigger);
        }
    }

    public void SortObjetosCogibles()
    {
        ObjetosCogibles.Sort((a, b) =>
        {
            float distanciaA = Vector3.Distance(a.gameObject.transform.position, transform.position);
            float distanciaB = Vector3.Distance(b.gameObject.transform.position, transform.position);
            return distanciaA.CompareTo(distanciaB);
        });
    }

    public bool AgregarAInventario(ObjetoEscena objetoTrigger)
    {
        //Al agregar un objeto nuevo primero verifica si ya existe un contenedor con su objeto y si aun no ha llegado a su limite de espacio
        for (int i = 0; i < ObjetosInventario.Count; i++)
        {
            if (ObjetosInventario[i] == objetoTrigger.ID)
            {
                if (ContenedorObjetos[i].Cantidad + objetoTrigger.Cantidad <= GestorObjetos.Instancia.DiccionarioObjeto[objetoTrigger.Nombre].MaximoAcumulable)
                {
                    ContenedorObjetos[i].Cantidad += objetoTrigger.Cantidad;
                    ObjetosCogibles.Remove(ObjetosCogibles[0]);
                    return true;
                }
                else
                {
                    continue;
                }
            }
        }

        //Si no hay un contenedor con espacio para el, pues se agrega al siguiente contenedor vacio
        for (int i = 0; i < ObjetosInventario.Count; i++)
        {
            if (ObjetosInventario[i] == 0)
            {
                ObjetosInventario[i] = objetoTrigger.ID;
                ContenedorObjetos[i].Nombre = objetoTrigger.Nombre;
                ContenedorObjetos[i].Cantidad = objetoTrigger.Cantidad;
                ObjetosCogibles.Remove(ObjetosCogibles[0]);
                return true;
            }
        }

        return false;
    }

    public void SoltarObjeto()
    {
        //Al pulsar soltar, el ID del objeto seleccionado se compara con el diccionario de objetos. Se intancia el prefab con el ID que concuerde con el seleccionado.
        GameObject objetoSoltado = Instantiate(GestorObjetos.Instancia.DiccionarioObjeto[ObjetoSeleccionadoNombre].Prefab.gameObject);
        objetoSoltado.transform.position = transform.position;

        ContenedorObjetos[ContenedorSeleccionado].Cantidad--;

        //Si ya no hay mas de ese objeto en el inventario pues se elimina de la lista de ObjetosInventario
        if (ContenedorObjetos[ContenedorSeleccionado].Cantidad == 0)
        {
            ObjetosInventario[ContenedorSeleccionado] = 0;
            MenuInventario.SetActive(false);
        }
    }

    public void ToggleInterfaz()
    {
        //Definir posicion de la interfaz de inventario segun el tama�o de pantalla
        if (InterfazAbierta)
        {
            InventarioInterfaz.transform.localPosition = _PosicionOculta;
            InterfazAbierta = false;
            MenuInventario.SetActive(false);
        }
        else
        {
            InventarioInterfaz.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            InterfazAbierta = true;
        }

    }

    public void ActivarMenu(int buttonIndex)
    {
        if (ContenedorObjetos[buttonIndex].ID != 0)
        {
            MenuInventario.SetActive(true);
            ObjetoSeleccionadoNombre = ContenedorObjetos[buttonIndex].Nombre;
            ContenedorSeleccionado = buttonIndex;
        }
        else
        {
            MenuInventario.SetActive(false);
        }
        
    }

}
