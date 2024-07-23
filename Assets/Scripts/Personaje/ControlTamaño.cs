using UnityEngine;

public class TamañoPersonaje : MonoBehaviour
{
    private SpriteRenderer Imagen;
    private float Xtemporal;
    private CapsuleCollider Colision;
    public float Tamaño=1.5f;
    private SistemasPersonaje Personaje;

    private void Awake()
    {
        Personaje= GetComponent<SistemasPersonaje>();
        Imagen = GetComponent<SpriteRenderer>();
        Colision = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        Cambiar();
    }
    public void Cambiar()
    {
        Xtemporal = (float)Imagen.size.x / Imagen.size.y;
        Imagen.size = new Vector2(Xtemporal * Tamaño, 1 * Tamaño);
        // print(Ficha.Perfil.Tamaño+" "+ Imagen.size.x+" "+ Imagen.size.y);
        AjustarTamaño(Tamaño, Colision.height);
        Colision.radius = Mathf.Clamp(Tamaño * 0.3125f, 0.05f, 0.95f);
        Colision.height = Tamaño;
        Personaje.Raycast.CalcularMedidas();
    }


    public void AjustarTamaño(float tamaño, float tamañoAnterior)
    {
        //print("ajusto tamaño");
        transform.position += transform.up * (tamaño - tamañoAnterior) / 2;
    }
}
