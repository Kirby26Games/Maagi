using UnityEngine;

public class ZonasGravedad : MonoBehaviour
{
    public bool Techo;
    public bool ParedDer;
    public bool ParedIzq;
    public Vector3 Direccion;
    
    public bool alternarGravedad;
    public int rotatingSpeed;
    public Quaternion targetRotation;
    public Quaternion originalRotation;
    Rigidbody rbObjetoAfectado;

    private void Start()
    {

    }

    private void Update()
    {

        if (rbObjetoAfectado != null) 
        {
            rbObjetoAfectado.gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotatingSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Jugador") 
        { 
            targetRotation = new Quaternion(180,0,0,0);
            originalRotation = other.gameObject.transform.rotation;
            rbObjetoAfectado = other.gameObject.GetComponent<Rigidbody>();

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Jugador")
        {
            alternarGravedad = false;
            targetRotation = originalRotation;
            rbObjetoAfectado = other.gameObject.GetComponent<Rigidbody>();
        }
    }

}
