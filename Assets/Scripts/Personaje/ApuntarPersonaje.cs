using System.Collections.Generic;
using UnityEngine;

public class ApuntarPersonaje : MonoBehaviour
{
    [Header("Apuntado")]
    public List<Transform> ObjetivosDetectados = new List<Transform>();
    public int IndiceObjetivos = 0;
    public float RangoVision = 20f;
    public float AnguloApuntar = 35f;
    public SphereCollider VisionCollider;
    public GameObject Target; //el prefab del indicador target
    GameObject _TargetScene; //el target que esta en la scena

    //esto no lo veo necesario asi que lo comento
    //[Header("Referencias")]
    //private ControlesPersonaje Controles;
    //private SistemasPersonaje Personaje;

    void Start()
    {
        VisionCollider.radius = RangoVision;
        if (_TargetScene == null)
        {
            _TargetScene = Instantiate(Target);
            _TargetScene.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other) //Incluir enemigos y aliados por colisión en la lista de posibles objetivos
    {
        if (other.TryGetComponent(out Fijable fijable))
        {
            ObjetivosDetectados.Add(fijable.transform);

        }
    }

    void OnTriggerExit(Collider other) //Eliminar enemigos y aliados por colisión de la lista de posibles objetivos
    {
        if (other.TryGetComponent(out Fijable fijable))
        {
            ObjetivosDetectados.Remove(fijable.transform);

        }
    }

    public void DetectarObjetivos()
    {
        //mejor prevenir que lamentar, confirma que existe un target en la escena
        if (_TargetScene == null)
        {
            _TargetScene = Instantiate(Target);
            _TargetScene.SetActive(false);
        }

        if (IndiceObjetivos >= ObjetivosDetectados.Count)
        {
            _TargetScene.SetActive(false);
            // Resetear al principio de la lista
            IndiceObjetivos = 0;
            //aquí desfijo
            return;
        }

        Transform objetivoActual = ObjetivosDetectados[IndiceObjetivos];
        IndiceObjetivos++;
        Vector3 direccionObjetivo = (objetivoActual.position - transform.position).normalized;
        float anguloDeteccion = Vector3.Angle(transform.right, direccionObjetivo);
        float distaciaObjetivo = Vector3.Distance(objetivoActual.position, transform.position);

        if (anguloDeteccion > AnguloApuntar)
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
        //Movemos el indicador de objetivo del objetivo actual
        //la resta con el foward es para que se ponga en frente
        _TargetScene.transform.position = objetivoActual.position - Vector3.forward;
        if (_TargetScene.activeInHierarchy==false)
        {
            _TargetScene.SetActive(true);
        }
    }
}
