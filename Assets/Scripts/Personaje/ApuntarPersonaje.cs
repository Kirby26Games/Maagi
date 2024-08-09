using System.Collections.Generic;
using UnityEngine;

public class ApuntarPersonaje : MonoBehaviour
{
    [Header("Apuntado")]
    public List<Transform> ObjetivosDetectados = new List<Transform>();
    public int IndiceObjetivos = 0;
    public float RangoVision = 20f;

    void Start()
    {
        GetComponent<SphereCollider>().radius = RangoVision;
    }

    void OnTriggerEnter(Collider other) //Incluir enemigos y aliados por colisión en la lista de posibles objetivos
    {
        Fijable fijable = other.GetComponent<Fijable>();
        if (fijable != null)
        {
            ObjetivosDetectados.Add(fijable.transform);
        }
    }

    void OnTriggerExit(Collider other) //Eliminar enemigos y aliados por colisión de la lista de posibles objetivos
    {
        Fijable fijable = other.GetComponent<Fijable>();
        if (fijable != null)
        {
            ObjetivosDetectados.Remove(fijable.transform);
        }
    }

    public void DetectarObjetivos()
    {
        if (IndiceObjetivos >= ObjetivosDetectados.Count)
        {
            // Resetear al principio de la lista
            IndiceObjetivos = 0;
            //aquí desfijo
            return;
        }

        Transform objetivoActual = ObjetivosDetectados[IndiceObjetivos];
        IndiceObjetivos++;
        Vector3 direccionObjetivo = (objetivoActual.position - transform.position).normalized;
        float anguloDeteccion = Vector3.Angle(transform.right, direccionObjetivo);
        float anguloApuntar = 35f;
        float distaciaObjetivo = Vector3.Distance(objetivoActual.position, transform.position);

        if (anguloDeteccion > anguloApuntar)
        {
            //intento detectar el siguiente
            DetectarObjetivos();
            return;
        }

        Ray rayo = new();
        rayo.origin = transform.position;
        rayo.direction = direccionObjetivo;

        RaycastHit[] datos = Physics.RaycastAll(rayo, distaciaObjetivo);
        Debug.DrawRay(rayo.origin, rayo.direction * datos[0].distance, Color.red, 10.0f);
        
        for (int i = 0; i < datos.Length; i++) 
        { 
            if (!datos[i].transform.GetComponent<Fijable>())
            {
                DetectarObjetivos();
                return;
            }
        }
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
            GameObject selectorObjetivos = objetivo.transform.Find("Target").gameObject;
            if (selectorObjetivos != null)
            {
                selectorObjetivos.SetActive(false);
            }
        }
        indicadorObjetivo.SetActive(true);
    }
}
