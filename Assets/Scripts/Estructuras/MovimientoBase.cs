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
    public SistemaBase Sistema;
    public float FuerzasAlPersonajeX;
    public float FuerzaInicialX;
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

    public void LimpiarFuerza()
    {
        if(ContadorFuerzasAZero < 0f)
        {
            FuerzasAlPersonajeX = 0f;
        }
        else
        {
            FuerzasAlPersonajeX = ContadorFuerzasAZero / DuracionFuerzasAZero * FuerzaInicialX;
            ContadorFuerzasAZero -= Time.deltaTime;
        }
    }
    public void AñadirFuerza(Vector3 nuevaDireccion, float magnitud)
    {
        Vector3 fuerzasAlPersonaje = nuevaDireccion.normalized * magnitud + Vector3.right * FuerzasAlPersonajeX;
        DuracionFuerzasAZero = 2 * (nuevaDireccion.normalized * magnitud).y / -Sistema.Gravedad.Gravedad;
        ContadorFuerzasAZero = DuracionFuerzasAZero;
        FuerzaInicialX = fuerzasAlPersonaje.x;
        Sistema.Gravedad.EjeY += fuerzasAlPersonaje.y;
    }
}
