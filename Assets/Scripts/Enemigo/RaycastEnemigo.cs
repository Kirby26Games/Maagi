using UnityEngine;

public class RaycastEnemigo : MonoBehaviour
{
    [Header("Referencias")]
    public RaycastHit DatosPendiente;
    private Collider Colision;
    [Header("Interacciones")]
    private float _RangoDeteccionSuelo;
    public Color ColorDeteccionSuelo;
    public float AnguloEscaladaMaximo;
    private float DistanciaRayoEscalada;
    private float AngulacionSuelo;
    [Header("Medidas")]
    private float Alto;
    private float Ancho;
    [HideInInspector] public float Radio;
    private SistemaGravedad Gravedad;
    private void Awake()
    {
        Gravedad = GetComponent<SistemaGravedad>();
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
        if (Physics.SphereCast(transform.position, Radio, -transform.up, out Datos, _RangoDeteccionSuelo))
        {
            Debug.DrawRay(transform.position, -transform.up * (Datos.distance + Radio), ColorDeteccionSuelo);
            //Comprobamos si estamos en una pendiente
            if (Physics.Raycast(transform.position, -transform.up, out DatosPendiente, DistanciaRayoEscalada))
            {
                //Cogemos el angulo de la pendiente usando su normal
                AngulacionSuelo = Vector3.Angle(Vector3.up, DatosPendiente.normal);

            }
            //Estamos en el suelo si AngulacionSuelo es menor al angulo de escalada maximo
            Gravedad.EnSuelo = AngulacionSuelo <= AnguloEscaladaMaximo;
            //Dibujo el rayo de escalada
            Debug.DrawRay(transform.position + Vector3.forward * 0.01f, Vector3.down * DistanciaRayoEscalada, Color.magenta);

        }
        else
        {
            Gravedad.EnSuelo = false;
            Debug.DrawRay(transform.position, -transform.up * (_RangoDeteccionSuelo + Radio), Color.green);
        }
    }

    public void CalcularMedidas()
    {
        Alto = Colision.bounds.size.y;
        Ancho = Colision.bounds.size.x;
        Radio = Ancho / 2;
        _RangoDeteccionSuelo = Alto / 2 - Radio + 0.001f;
        DistanciaRayoEscalada = Radio / Mathf.Sin((90 - AnguloEscaladaMaximo) * Mathf.PI / 180) + Alto / 2 - Radio + 0.001f;
    }
}
