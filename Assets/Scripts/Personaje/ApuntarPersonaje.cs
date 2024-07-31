using System.Collections.Generic;
using UnityEngine;

public class ApuntarPersonaje : MonoBehaviour
{
    [Header("Apuntado")]
    public Camera CamaraPrincipal;
    public List<Transform> ObjetivosEnPantalla = new List<Transform>();
    public List<Transform> ObjetivosDetectados = new List<Transform>();
    public int IndiceObjetivos = 0;
    [Header("Sistemas")]
    private ControlesPersonaje Controles;
    private SistemasPersonaje Personaje;

    void Start()
    {
        CamaraPrincipal = Camera.main;
    }

    void Update()
    {
        ActualizarObjetivosEnPantalla();
        DetectarObjetivos();
    }
    bool EstaEnPantalla(GameObject objetivoEnPantalla)
    {
        // Comprueba si el transform del GameObject está en pantalla (MainCamera)
        Vector3 puntoEnPantalla = CamaraPrincipal.WorldToViewportPoint(objetivoEnPantalla.transform.position);
        return puntoEnPantalla.z > 0 && puntoEnPantalla.x > 0 && puntoEnPantalla.x < 1 && puntoEnPantalla.y > 0 && puntoEnPantalla.y < 1;
    }

    void ActualizarObjetivosEnPantalla()
    {
        ObjetivosEnPantalla.Clear();

        // Buscamos enemigos
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemigo");
        foreach (var enemigo in enemigos)
        {
            //Si los enemigos estan en pantalla, los añado a la lista
            if (EstaEnPantalla(enemigo))
            {
                ObjetivosEnPantalla.Add(enemigo.transform);
            }
        }

        // Buscamos aliados
        GameObject[] aliados = GameObject.FindGameObjectsWithTag("Aliado");
        foreach (var aliado in aliados)
        {
            //Si los aliados estan en pantalla, los añado a la lista
            if (EstaEnPantalla(aliado))
            {
                ObjetivosEnPantalla.Add(aliado.transform);
            }
        }
    }

    void DetectarObjetivos()
    {
        ObjetivosDetectados.Clear();
        foreach (Transform objetivo in ObjetivosEnPantalla)
        {
            Vector3 direccionObjetivo = (objetivo.position - transform.position).normalized;
            float anguloDeteccion = Vector3.Angle(transform.right, direccionObjetivo);
            float anguloApuntar = 22.5f;

            RaycastHit detectado;
            //Hago un raycast a los ObjetivosEnPantalla, y si los golpea los añado a la lista ObjetivosDetectados,
            //así nos aseguramos de que solo podemos CambiarObjetivo() entre esos objetivos y no entre todos los que haya en pantalla
            if (Physics.Raycast(transform.position, objetivo.position - transform.position, out detectado))
            {
                if (detectado.transform == objetivo)
                {
                    Debug.DrawRay(transform.position, objetivo.position - transform.position, Color.red);
                    Debug.Log("DETECTADO " + detectado.transform.name);
                    if (anguloDeteccion <= anguloApuntar)
                    {
                        // Agrega a la lista de enemigos detectados
                        ObjetivosDetectados.Add(objetivo);
                    }
                }
            }
        }
    }

    public void CambiarObjetivo()
    {
        Transform objetivoActual = ObjetivosDetectados[IndiceObjetivos];

        //Contador para cambiar de objetivos
        IndiceObjetivos++;
        if (IndiceObjetivos >= ObjetivosDetectados.Count)
        {
            IndiceObjetivos = 0;
        }

        // Activamos el indicador para el nuevo objetivo y desactivar el de los demás
        foreach (var objetivo in ObjetivosDetectados)
        {
            GameObject indicadorObjetivo = objetivo.transform.Find("Target").gameObject;
            if (indicadorObjetivo != null)
            {
                indicadorObjetivo.SetActive(objetivo == objetivoActual);
            }
        }
    }

}
