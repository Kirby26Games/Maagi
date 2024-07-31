using System.Collections.Generic;
using UnityEngine;

public class ApuntarPersonaje : MonoBehaviour
{
    [Header("Apuntado")]
    public List<Transform> ObjetivosDetectados = new List<Transform>();
    public int IndiceObjetivos = 0;
    public float RangoVision = 20f;
    [Header("Referencias")]
    private ControlesPersonaje Controles;
    private SistemasPersonaje Personaje;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other) //Incluir enemigos y aliados por colisi�n en la lista de posibles objetivos
    {
        ScriptEnemigo enemigo = other.GetComponent<ScriptEnemigo>();
        ScriptAliado aliado = other.GetComponent<ScriptAliado>();
        if (enemigo != null)
        {
            ObjetivosDetectados.Add(enemigo.transform);
        }
        if (aliado != null)
        {
            ObjetivosDetectados.Add(aliado.transform);
        }
    }

    void OnTriggerExit(Collider other) //Eliminar enemigos y aliados por colisi�n de la lista de posibles objetivos
    {
        ScriptEnemigo enemigo = other.GetComponent<ScriptEnemigo>();
        ScriptAliado aliado = other.GetComponent<ScriptAliado>();
        if (enemigo != null)
        {
            ObjetivosDetectados.Remove(enemigo.transform);
        }
        if (aliado != null)
        {
            ObjetivosDetectados.Remove(aliado.transform);
        }
    }


    //public void DetectarObjetivos()
    //{
    //    bool objetivoEncontrado = false;

    //    while (!objetivoEncontrado && IndiceObjetivos < ObjetivosDetectados.Count)
    //    {
    //        Transform objetivoActual = ObjetivosDetectados[IndiceObjetivos];
    //        Vector3 direccionObjetivo = (objetivoActual.position - transform.position).normalized;
    //        float anguloDeteccion = Vector3.Angle(transform.right, direccionObjetivo);
    //        float anguloApuntar = 22.5f;

    //        Ray rayo = new Ray(transform.position, direccionObjetivo);
    //        RaycastHit datos;

    //        if (Physics.Raycast(rayo, out datos, RangoVision) && datos.transform == objetivoActual && anguloDeteccion <= anguloApuntar)
    //        {
    //            Debug.DrawRay(rayo.origin, rayo.direction * datos.distance, Color.red, 10.0f);
    //            GameObject indicadorObjetivo = objetivoActual.transform.Find("Target").gameObject;
    //            if (indicadorObjetivo != null)
    //            {
    //                foreach (var objetivo in ObjetivosDetectados)
    //                {
    //                    GameObject SelectorObjetivos = objetivo.transform.Find("Target").gameObject;
    //                    if (SelectorObjetivos != null)
    //                    {
    //                        SelectorObjetivos.SetActive(false);
    //                    }
    //                }
    //                indicadorObjetivo.SetActive(true);
    //            }
    //            objetivoEncontrado = true;
    //        }
    //        else
    //        {
    //            IndiceObjetivos++;
    //        }
    //    }

    //    if (IndiceObjetivos >= ObjetivosDetectados.Count)
    //    {
    //        IndiceObjetivos = 0;  // Resetear al principio de la lista
    //        Debug.Log("Todos los posibles objetivos fueron comprobados y el rayo no los golpea");
    //    }
    //}


    public void DetectarObjetivos()
    {
        if (IndiceObjetivos >= ObjetivosDetectados.Count)
        {
            IndiceObjetivos = 0;  // Resetear al principio de la lista
            //aqu� desfijo
            return;
        }

        Transform objetivoActual = ObjetivosDetectados[IndiceObjetivos];
        IndiceObjetivos++;
        Vector3 direccionObjetivo = (objetivoActual.position - transform.position).normalized;
        float anguloDeteccion = Vector3.Angle(transform.right, direccionObjetivo);
        float anguloApuntar = 22.5f;

        Ray rayo = new();
        rayo.origin = transform.position;
        rayo.direction = direccionObjetivo;

        RaycastHit datos;
        if (!Physics.Raycast(rayo, out datos, RangoVision))
        {
            //Debug.Log("No se detect� objetivo");
            //El rayo no choca con nada
            //intento detectar el siguiente
            DetectarObjetivos();
            return;
        }
        if (datos.transform != objetivoActual)
        {
            //intento detectar el siguiente
            DetectarObjetivos();
            return;
        }
        if (anguloDeteccion > anguloApuntar)
        {
            //intento detectar el siguiente
            DetectarObjetivos();
            return;
        }
        Debug.DrawRay(rayo.origin, rayo.direction * datos.distance, Color.red, 10.0f);
        //Encontramos el indicador de objetivo del objetivo actual
        GameObject indicadorObjetivo = objetivoActual.transform.Find("Target").gameObject;
        //Si no tiene el indicador no hace nada
        if (indicadorObjetivo == null)
        {
            return;
        }
        //Desactivamos todos los indicadores y se activa solo el de objetivo actual
        foreach (var objetivo in ObjetivosDetectados)
        {
            GameObject SelectorObjetivos = objetivo.transform.Find("Target").gameObject;
            if (SelectorObjetivos != null)
            {
                SelectorObjetivos.SetActive(false);
            }
        }
        indicadorObjetivo.SetActive(true);
    }
}
