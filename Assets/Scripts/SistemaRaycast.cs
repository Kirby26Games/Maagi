using UnityEngine;

public class SistemaRaycast : MonoBehaviour
{
    [Header("Referencias")]
    public RaycastHit DatosPendiente; 
    private Collider Colision;
    [Header("Interacciones")]
    public float RangoDeteccionSuelo;
    public Color ColorDeteccionSuelo;
    public float AnguloEscaladaMaximo;
    private float DistanciaRayoEscalada;
    private float AngulacionSuelo;
    [Header("Medidas")]
    private float Alto;
    private float Ancho;
    [HideInInspector] public float Radio;
    private SistemasPersonaje SistemaPersonaje;
    private void Awake()
    {
        SistemaPersonaje = GetComponent<SistemasPersonaje>();
        Colision = GetComponent<Collider>(); //Cogemos el collider del objeto
    }
    private void Start()
    {
        CalcularMedidas();
    }
    private void Update()
    {

        DetectarSuelo();
    }


    public void DetectarSuelo()
    {

        RaycastHit Datos;
        if (Physics.SphereCast(transform.position, Radio, -transform.up, out Datos, RangoDeteccionSuelo))
        {
            Debug.DrawRay(transform.position, -transform.up * (Datos.distance + Radio), ColorDeteccionSuelo);
            //Comprobamos si estamos en una pendiente
            if (Physics.Raycast(transform.position, -transform.up, out DatosPendiente, DistanciaRayoEscalada))
            {
                //Cogemos el angulo de la pendiente usando su normal
                AngulacionSuelo = Vector3.Angle(Vector3.up, DatosPendiente.normal);

            }
            //Estamos en el suelo si AngulacionSuelo es menor a el angolo de escalada maximo
            SistemaPersonaje.Gravedad.EnSuelo = AngulacionSuelo <= AnguloEscaladaMaximo;
            //Dibujo el rayo de escalada
            Debug.DrawRay(transform.position + Vector3.forward * 0.01f, Vector3.down * DistanciaRayoEscalada, Color.magenta);

        }
        else
        {
            SistemaPersonaje.Gravedad.EnSuelo = false;
            Debug.DrawRay(transform.position, -transform.up * (RangoDeteccionSuelo + Radio), Color.green);
        }
    }

    public void CalcularMedidas()
    {
        Alto = Colision.bounds.size.y;
        Ancho = Colision.bounds.size.x;
        Radio = Ancho / 2;
        RangoDeteccionSuelo = Alto / 2 - Radio + 0.001f;
        DistanciaRayoEscalada = Radio / Mathf.Sin((90 - AnguloEscaladaMaximo) * Mathf.PI / 180) + Alto / 2 - Radio + 0.001f;
        //DistanciaRayoEscalada = Radio / Mathf.Sin(Mathf.Deg2Rad * (90 - AnguloEscaladaMaximo)) + Alto / 2 - Radio + 0.001f;
    }
}