using UnityEngine;

public class ColisionesPersonaje : MonoBehaviour
{
    public bool CercaEscalera;
    private SistemasPersonaje Personaje;
    private Rigidbody RBPersonaje;

    private void Awake()
    {
        Personaje = GetComponent<SistemasPersonaje>();
        RBPersonaje = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ZonasGravedad zonasGravedad))
        {
            zonasGravedad.AlternarGravedad = true;
            zonasGravedad.ObjetoAfectado = this.gameObject;
        }
        
        if (other.gameObject.tag == "Escalera")
        {
            CercaEscalera = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ZonasGravedad zonasGravedad))
        {
            zonasGravedad.AlternarGravedad = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        
        if (other.gameObject.tag == "Escalera")
        {
            CercaEscalera = false;
            Personaje.Movimiento.EnEscalera = false;
            Personaje.Movimiento.PuedoAtacar = true;
        }
    }
}
