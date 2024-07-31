using UnityEngine;

public class MovimientoEnemigo : MonoBehaviour
{
    public enum CriteriosSalto { Siempre, Nunca, DetectaBorde }
    [Header("Capacidades")]
    public CriteriosSalto CriterioSalto;
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
    [Header("Escaleras")]
    private bool _CercaEscalera;

    private void Awake()
    {
        _EstadoActual = GetComponent<EstadoEnemigo>();
        _Cuerpo = GetComponent<Rigidbody>();
        _Gravedad = GetComponent<SistemaGravedad>();
    }

    private void Update()
    {
        // Si está alerta persigue al objetivo
        if(_EstadoActual.Estado == EstadoEnemigo.Estados.Alerta)
        {
            Perseguir();
        }
        // Si está en el suelo y puede saltar recupera sus saltos
        if(CriterioSalto != CriteriosSalto.Nunca && _Gravedad.EnSuelo)
        {
            ReiniciarSaltos();
        }
    }

    private void Perseguir()
    {
        // A velocidadFinal se le van a añadir los distintos desplazamientos
        Vector3 velocidadFinal = Vector3.zero;
        // Añadir la velocidad correcta en la dirección correcta
        velocidadFinal += (_EstadoActual.ObjetivoFijado.transform.position - transform.position).normalized.x * VelocidadMovimiento * Vector3.right;

        // Revisar si debe saltar en caso de que sea capaz
        if(CriterioSalto != CriteriosSalto.Nunca)
        {
            Saltar();
        }

        // Revisar si debe usar escaleras en caso de que sea capaz
        if (PuedeUsarEscaleras)
        {
            UsarEscalera();
        }

        // Añadir la gravedad que le afecta
        velocidadFinal.y += _Gravedad.EjeY;

        // Mandar la velocidad resultante al cuerpo
        _Cuerpo.linearVelocity = velocidadFinal;
    }

    private void UsarEscalera()
    {
        // Si no está cerca de escaleras no las puede usar
        if(!_CercaEscalera)
        {
            return;
        }

        if(transform.position.y < _EstadoActual.ObjetivoFijado.transform.position.y)
        {
            // Subir escalera
        }
        if (transform.position.y > _EstadoActual.ObjetivoFijado.transform.position.y)
        {
            // Bajar escalera
        }
    }

    private void Saltar()
    {
        // Si no puede o no quiere saltar no lo hace
        if (!PuedoSaltar() || !QuieroSaltar())
        {
            return;
        }

        // En caso contrario, calcula lo que debe saltar
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

    private bool QuieroSaltar()
    {
        // Dependiendo del criterio, devuelve true si quiere saltar o false si no
        switch(CriterioSalto)
        {
            // Siempre devuelve true siempre
            case CriteriosSalto.Siempre:
                return true;

            // Detecta borde se fija en la distancia al obstáculo más cercano (si existe) y si el objetivo está por encima de su posición
            case CriteriosSalto.DetectaBorde:
                if(_EstadoActual.DistanciaAObstaculo < VelocidadMovimiento &&
                    _EstadoActual.DistanciaAObstaculo > -1f &&
                    _EstadoActual.ObjetivoFijado.transform.position.y > transform.position.y)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
        // Nunca devuelve false siempre
        return false;
    }

    public void ReiniciarSaltos()
    {
        if(_Gravedad.EnSuelo)
        {
            _SaltosEnElAire = 0;
        }
    }
}
