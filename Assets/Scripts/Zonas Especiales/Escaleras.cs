using UnityEngine;

public class Escaleras : MonoBehaviour
{
    public int VelocidadSubirEscalera = 2;
    public float YLimiteSuperior;
    public float YLimiteInferior;

    private void Start()
    {
        YLimiteSuperior = gameObject.GetComponent<Renderer>().bounds.max.y;
        YLimiteInferior = gameObject.GetComponent<Renderer>().bounds.min.y;

    }
}
