using UnityEngine;

public class MovimientoBase : MonoBehaviour
{
    [Header("Componentes")]
    [HideInInspector] public Rigidbody Cuerpo;
    [HideInInspector] public Collider Colision;
    [Header("Velocidad")]
    public float VelocidadBase = 3;
    public float MultiplicadorAlCorrer = 2;
    public float VelocidadModificador = 1;
    public float VelocidadFinal;
    [Header("Salto")]
    public float DistanciaSalto = 5.5f;
    public int SaltosEnElAireMaximos = 1;
    [HideInInspector] public int SaltosEnElAire;
    [Header("Escaleras")]
    [HideInInspector] public bool CercaEscalera;
    [HideInInspector] public float PosicionEscalera;
    [HideInInspector] public float VelocidadSubirEscaleras;
    public bool EnEscalera;
    [Header("Externo")]
    public Vector3 FuerzasAlPersonaje;
    public Vector3 FuerzaInicial;
    public float DuracionFuerzasAZero = 3f;
    public float ContadorFuerzasAZero;

    public void CalcularVelocidad()
    {
        VelocidadFinal = VelocidadBase * VelocidadModificador;
    }
    public void Correr(bool Corriendo)
    {
        if (Corriendo)
        {
            VelocidadModificador = MultiplicadorAlCorrer;
        }
        else
        {
            VelocidadModificador = 1;
        }
        CalcularVelocidad();
    }

    public void LimpiarFuerza(bool instantaneo = false)
    {
        if(instantaneo || ContadorFuerzasAZero < 0f)
        {
            FuerzasAlPersonaje = Vector3.zero;
        }
        else
        {
            FuerzasAlPersonaje = ContadorFuerzasAZero / DuracionFuerzasAZero * FuerzaInicial;
            ContadorFuerzasAZero -= Time.deltaTime;
        }
    }
    public void AñadirFuerza(Vector3 nuevaDireccion, float magnitud, float duracion = 3f)
    {
        FuerzasAlPersonaje += nuevaDireccion.normalized * magnitud;
        DuracionFuerzasAZero = duracion;
        ContadorFuerzasAZero = DuracionFuerzasAZero;
        FuerzaInicial = FuerzasAlPersonaje;
    }

    public Vector3 FuerzasTotales()
    {
        LimpiarFuerza();
        return FuerzasAlPersonaje;
    }
}
