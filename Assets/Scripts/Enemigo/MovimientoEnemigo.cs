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
    private SistemaGravedad _Gravedad;

    private void Awake()
    {
        _EstadoActual = GetComponent<EstadoEnemigo>();
        _Cuerpo = GetComponent<Rigidbody>();
        _Gravedad = GetComponent<SistemaGravedad>();
    }

    private void Update()
    {
        if(_EstadoActual.Estado == "Alerta")
        {
            Perseguir();
        }
        if(PuedeSaltar && _Gravedad.EnSuelo)
        {
            ReiniciarSaltos();
        }
    }

    private void Perseguir()
    {
        Vector3 velocidadFinal = Vector3.zero;
        velocidadFinal += (_EstadoActual.ObjetivoFijado.transform.position - transform.position).normalized.x * VelocidadMovimiento * Vector3.right;
        if(PuedeSaltar)
        {
            Saltar();
        }
        velocidadFinal.y += _Gravedad.EjeY;
        _Cuerpo.linearVelocity = velocidadFinal;
    }

    private void Saltar()
    {
        if (!PuedoSaltar())
        {
            return;
        }
        _Gravedad.EjeY = Mathf.Sqrt(DistanciaSalto * -2 * VariablesGlobales.Instancia.Gravedad);
    }

    private bool PuedoSaltar()
    {
        bool puedo = false;
        //Si estoy en el suelo, siempre puedo saltar
        if (_Gravedad.EnSuelo)
        {
            puedo = true;
        }
        //Si estoy en el aire, puedo saltar si no he llegado a los saltos maximos
        else if (_SaltosEnElAire < SaltosEnElAireMaximos)
        {
            puedo = true;
            _SaltosEnElAire++;
        }
        return puedo;
    }

    public void ReiniciarSaltos()
    {
        if(_Gravedad.EnSuelo)
        {
            _SaltosEnElAire = 0;
        }
    }
}
