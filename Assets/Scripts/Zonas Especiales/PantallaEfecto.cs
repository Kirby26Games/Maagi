using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;

public class PantallaEfecto : MonoBehaviour
{
    public Vector3 Tamaños = new Vector3(55f, 25f, 5f);
    private void ActualizarAspecto()
    {
        // Hacer invisible
        gameObject.GetComponent<Renderer>().enabled = false;
    }

    public async void Destruir(int duracion)
    {
        await Task.Delay(duracion);
        Destroy(gameObject);
    }

    public void Dañar(float daño)
    {
        ActualizarAspecto();

        Collider[] listaDetectados = Physics.OverlapBox(transform.position, Tamaños);
        List<GameObject> listaObjetivos = new List<GameObject>();
        for (int i = 0; i < listaDetectados.Length; i++)
        {
            // TODO: Generalizar en un Efecto que se llame para poder quitar SistemaEnemigo
            if (listaDetectados[i].gameObject.GetComponent<SistemaEnemigo>() != null)
            {
                listaObjetivos.Add(listaDetectados[i].gameObject);
            }
        }

        Debug.Log("Dañados " + listaObjetivos.Count() + " enemigos.");
    }
}
