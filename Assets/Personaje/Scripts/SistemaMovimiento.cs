using Unity.VisualScripting;
using UnityEngine;

public class SistemaMovimiento : MonoBehaviour
{
    [Header("Variables")]
    public float velocidad = 1;
    public float potenciaSalto = 2;
    public int saltosMaximos;
    public float multiplicadorCorrer = 2;

    [Header("Movimiento")]
    public float ejeX;
    public float ejeY;
    public float ejeZ;
    public Vector3 vectorMovimiento;
    
    //Privadas
    private float velocidadBase;
    int saltosActuales;
    Rigidbody rbMovimiento;

    private void Awake()
    {
        rbMovimiento = GetComponent<Rigidbody>();
        Physics.simulationMode = SimulationMode.Update; //Simular las fisicas durante la fase de UPDATE 
    }


    void Start()
    {
        velocidadBase = velocidad;
    }

    void Update()
    {
        Inputs();
        Movimiento();
    }

    void Inputs()
    {
        //Movimiento
        ejeX = Input.GetAxis("Horizontal") * velocidad;
        ejeZ = Input.GetAxis("Vertical") * velocidad;
        
        
        //Correr
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            CambioVelocidad(velocidadBase * multiplicadorCorrer);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            CambioVelocidad(velocidadBase);
        }

        //Salto
        if (Input.GetKeyDown(KeyCode.Space) && SistemaPersonaje.Instancia.SistemaRaycast.EnSuelo)
        {
            saltosActuales = 0;
            Salto();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && saltosActuales < saltosMaximos) //Esto sirve por si queremos añadir doble saltos
        {
            Salto();
        }
    }

    void CambioVelocidad(float modificaciones)
    {
        velocidad = modificaciones;
    }

    void Movimiento()
    {
        //Aplicamos el movimiento despues de convertirlo en un vector3
        vectorMovimiento = new Vector3(ejeX, ejeY, ejeZ);
        //Objeto.transformdirection aplica un vector en la direccion de el transform de un objeto(su z)
        rbMovimiento.linearVelocity = transform.TransformDirection(vectorMovimiento);
    }

    public void Salto()
    {
        //Personaje salta y se guarda cuantos saltos han pasado sin tocar el suelo
        ejeY = Mathf.Sqrt(potenciaSalto * 2f * -SistemaPersonaje.Instancia.SistemaGravedad.gravedad);
        saltosActuales++;
    }
}
