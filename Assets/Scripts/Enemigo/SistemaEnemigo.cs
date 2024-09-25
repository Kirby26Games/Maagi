using NUnit.Framework.Constraints;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MovimientoEnemigo), typeof(RaycastEnemigo))]
[RequireComponent(typeof(EstadoEnemigo), typeof(InventarioEnemigo), typeof(ColisionesEnemigo))]
public class SistemaEnemigo : SistemaBase
{
    //Este script contendra un indice de todos los scripts en uso
    public MovimientoEnemigo Movimiento;
    public RaycastEnemigo Raycast;
    public DeteccionEnemigo Deteccion;
    public ColisionesEnemigo Colisiones;
    public InventarioEnemigo Inventario;
    public EstadoEnemigo Estado;

    [System.Serializable]
    public class Accion
    {
        public string Nombre;
        public int Duracion;    // en ms (1000 = 1s)
        public System.Func<bool> AlIniciarAccion;
        public System.Func<bool> CriterioParada;
        // Otros campos que pueda necesitar

        public Accion(string nombre, int duracion, System.Func<bool> inicio, System.Func<bool> criterio)
        {
            this.Nombre = nombre;
            this.Duracion = duracion;
            this.AlIniciarAccion = inicio;
            this.CriterioParada = criterio;
        }
    }

    [SerializeReference] public Dictionary<string, Accion> DiccionarioAcciones;

    private void Awake()
    {
        Movimiento = GetComponent<MovimientoEnemigo>();
        Gravedad = GetComponent<SistemaGravedad>();
        Raycast = GetComponent<RaycastEnemigo>();
        Deteccion = GetComponentInChildren<DeteccionEnemigo>();
        Colisiones = GetComponent<ColisionesEnemigo>();
        Inventario = GetComponent<InventarioEnemigo>();
        Estado = GetComponent<EstadoEnemigo>();
        ControlesBase = null;

        Accion idle = new("Idle", 100, SiempreTrue, SiempreTrue);
        Accion mover = new("Mover", 1000, Estado.RecordarPosicion, Estado.RecordarPosicion);
        Accion atacar = new("Atacar", 2000, SiempreTrue, SiempreTrue);
        Accion cura = new("Curar", 3000, SiempreTrue, SiempreTrue);
        Accion cogerObjeto = new("CogerObjeto", 1000, Estado.RecordarObjeto, Estado.RecordarObjeto);

        DiccionarioAcciones = new()
        {
            { idle.Nombre, idle },
            { mover.Nombre, mover },
            { atacar.Nombre, atacar },
            { cura.Nombre, cura },
            { cogerObjeto.Nombre, cogerObjeto }
        };
    }

    public bool SiempreTrue()
    {
        return true;
    }
}
