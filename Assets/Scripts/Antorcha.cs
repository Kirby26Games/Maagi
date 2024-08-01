using UnityEngine;

public class Antorcha : MonoBehaviour
{
    public bool Encendida = false;
    bool _PuedoInteractuar;
    Renderer _MiRenderer;
    public Material MatEncendida, MatApagada;

    private void Awake()
    {
        _MiRenderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        if (Encendida)
        {
            Debug.Log("pp");
            //algo
        }
    }

    private void Update()
    {
        if (_PuedoInteractuar)
        {
            if (Input.GetKeyDown(Controles.Interactuar))
            {
                Interactuar();
            }
        }
    }
    public virtual void Interactuar()
    {
        Encendida = !Encendida;
        if (Encendida)
        {
            _MiRenderer.material = MatEncendida;
        }
        else
        {
            _MiRenderer.material = MatApagada;
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out SistemasPersonaje _))
        {
            _PuedoInteractuar = true;
        }
    } 
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out SistemasPersonaje _))
        {
            _PuedoInteractuar = false;
        }
    }
}
