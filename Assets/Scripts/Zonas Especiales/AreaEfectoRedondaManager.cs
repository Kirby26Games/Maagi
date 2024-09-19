using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class AreaEfectoRedondaManager : MonoBehaviour
{
    public float Radio;
    public int Duracion;

    private void ActualizarAspecto()
    {
        // Hacer invisible
        gameObject.GetComponent<Renderer>().enabled = false;
        // Actualizar radio
        gameObject.GetComponent<SphereCollider>().radius = Radio;
    }

    public async void Destruir()
    {
        await Task.Delay(Duracion);
        Destroy(gameObject);
    }

    public void Dañar(float daño)
    {
        ActualizarAspecto();

        Collider[] listaDetectados = Physics.OverlapSphere(transform.position, Radio);
        List<GameObject> listaObjetivos = new List<GameObject>();
        for (int i = 0; i < listaDetectados.Length; i++)
        {
            // TODO: Generalizar en un Efecto que se llame para poder quitar SistemaEnemigo
            if (listaDetectados[i].gameObject.GetComponent<SistemaEnemigo>() != null)
            {
                listaObjetivos.Add(listaDetectados [i].gameObject);
            }
        }

        Debug.Log("Dañados " + listaObjetivos.Count() + " enemigos.");
    }
}
