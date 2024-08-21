using System.Collections.Generic;
using UnityEngine;
using static EstadoEnemigo;

public class InventarioEnemigo : InventarioBase
{
    public List<ContenedorObjeto> ContenedorObjetos;
    public List<ObjetoEscena> ObjetosCogibles;
    private SistemaEnemigo _Enemigo;


    private void Awake()
    {
        _Enemigo = GetComponent<SistemaEnemigo>();
    }

    private void Start()
    {
        //Crea una lista vacia de los objetos de inventario
        ObjetosMaximos = 3;
        CrearInventario();
    }

    private void Update()
    {
        // TODO:Incluir criterio para coger objeto
        if (_Enemigo.Estado.ColaDeAccion[0] == EstadoEnemigo.Acciones.CogerObjeto)
        {
            LogicaCogerObjetos();
        }
    }

    private void LogicaCogerObjetos()
    {
        if (ObjetosCogibles.Count > 0)
        {
            SortObjetosCogibles();
            ObjetoEscena objetoAgregado = ObjetosCogibles[0];

            if (AgregarAInventario(objetoAgregado))
            {
                Destroy(objetoAgregado.gameObject);
            }

            // Terminar la acción de coger objeto, forzando un Idle en la primera posición
            _Enemigo.Estado.ObjetivoFijado = null;
            _Enemigo.Estado.DestinoFijado = Vector3.zero;
            _Enemigo.Estado.InsertarAccion(EstadoEnemigo.Acciones.Idle, 0, true);
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

    public void SoltarObjetos()
    {
        // TODO:integrar y mejorar el código cuando el enemigo desaparezca
        for (int i = 0; i < ContenedorObjetos.Count; i++)
        {
            for (int j = 0; j < ContenedorObjetos[i].Cantidad; j++)
            {
                GameObject objetoSoltado = Instantiate(GestorObjetos.Instancia.DiccionarioObjeto[ContenedorObjetos[i].Nombre].Prefab.gameObject);
                objetoSoltado.transform.position = transform.position;
            }
            ContenedorObjetos[i].Cantidad = 0;
            ObjetosInventario[i] = 0;
        }
    }
}
