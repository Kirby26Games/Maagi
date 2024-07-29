using UnityEngine;

public class MovimientoEnemigo : MonoBehaviour
{
    [Header("Capacidades")]
    public bool PuedeSaltar;
    public bool PuedeUsarEscaleras;
    [Header("Memoria")]
    private EstadoEnemigo _EstadoActual;
    [Header("Propiedades")]
    private Rigidbody _Cuerpo;
    public float VelocidadMovimiento;
    [Header("Salto")]
    public float DistanciaSalto;
    public int SaltosEnElAireMaximos;
    private int _SaltosEnElAire;

    private void Awake()
    {
        _EstadoActual = GetComponent<EstadoEnemigo>();
        _Cuerpo = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Perseguir(_EstadoActual.Estado == "Alerta");
    }

    private void Perseguir(bool debePerseguir)
    {
        Vector3 velocidadFinal = Vector3.zero;
        if(debePerseguir)
        {
            velocidadFinal += (_EstadoActual.ObjetivoFijado.transform.position - transform.position).normalized.x * VelocidadMovimiento * Vector3.right;
            if(PuedeSaltar)
            {
                velocidadFinal += Salto();
            }
        }
        else
        {
            return;
        }
        _Cuerpo.linearVelocity = velocidadFinal;
    }

    private Vector3 Salto()
    {
        if(true) // Aquí debe ir la comprobación de si no está en el suelo o le quedan saltos en el aire 
        {
            return Vector3.zero;
        }
        return Vector3.up * VariablesGlobales.Instancia.FuerzaSalto;
    }
}
