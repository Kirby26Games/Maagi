using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ProyectilEfecto : MonoBehaviour
{
    public SistemaBase Lanzador;
    public float Radio;
    public int Duracion;
    private bool _Disparado = false;
    private Vector3 _Direccion;
    private float _Velocidad;
    private float _Daño;

    private void FixedUpdate()
    {
        if(!_Disparado)
        {
            return;
        }
        transform.position += _Direccion * _Velocidad * Time.fixedDeltaTime;
    }

    private void ActualizarAspecto()
    {
        // Hacer invisible
        gameObject.GetComponent<Renderer>().enabled = true;
        // Actualizar radio
        gameObject.GetComponent<SphereCollider>().radius = Radio;
    }

    public async void Destruir()
    {
        await Task.Delay(Duracion);
        Destroy(gameObject);
    }

    public void Disparar(Vector3 direccion, float velocidad, float daño)
    {
        ActualizarAspecto();

        _Direccion = direccion;
        _Velocidad = velocidad;
        _Daño = daño;
        _Disparado = true;

    }

    private void OnCollisionEnter(Collision collision)
    {
        SistemaBase receptor = collision.gameObject.GetComponent<SistemaBase>();
        if (receptor != null || receptor != Lanzador)
        {
            Dañar(receptor, _Daño);
        }
    }

    public void Dañar(SistemaBase receptor, float daño)
    {
        Debug.Log("Dañados " + receptor.name + " enemigos.");
    }
}
