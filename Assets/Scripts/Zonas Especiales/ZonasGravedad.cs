using UnityEngine;

public class ZonasGravedad : MonoBehaviour
{
    public bool Techo;
    public bool ParedDer;
    public bool ParedIzq;
    
    public bool AlternarGravedad;
    private Quaternion RotacionObjetivo;
    private int VelocidadRotacion = 3;
    public GameObject ObjetoAfectado;

    private void Start()
    {
        RotacionObjetivo = VerificarPosicion();
    }

    private void Update()
    {
        if (AlternarGravedad) 
        {
            RotarPersonaje();
        }

    }

    private Quaternion VerificarPosicion()
    {
        Quaternion RotacionObjetivo = Quaternion.identity;

        if (Techo)
        {
            RotacionObjetivo = Quaternion.Euler(180, 0, 0);
        }
        else if (ParedDer)
        {
            RotacionObjetivo = Quaternion.Euler(0, 0, 90);
        }
        else if (ParedIzq)
        {
            RotacionObjetivo = Quaternion.Euler(0, 0, -90);
        }

        return RotacionObjetivo;

    }

    private void RotarPersonaje()
    {
        ObjetoAfectado.transform.rotation = RotacionObjetivo;
    }
    

}
