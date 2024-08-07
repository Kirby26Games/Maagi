using UnityEngine;

public class Antorcha : MonoBehaviour
{
    public bool Prendida = false;
    bool _PuedoInteractuar;
    SpriteRenderer _MiRenderer;
    public Sprite Encendida, Apagada;

    private void Awake()
    {
        _MiRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (Prendida)
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
        Prendida = !Prendida;
        if (Prendida)
        {
            _MiRenderer.sprite = Encendida;
        }
        else
        {
            _MiRenderer.sprite = Apagada;
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
