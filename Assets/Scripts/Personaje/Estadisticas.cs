using UnityEngine;

public class Estadisticas : MonoBehaviour
{
    public float Vida;
    public float Mana;
    public float Ataque;
    public float Peso;
    private SistemasPersonaje _Personaje;

    public bool Invulnerabilidad;

    private void Awake()
    {
        _Personaje = GetComponent<SistemasPersonaje>();
    }

    public void RecibirDano(float dano)
    {
        if (Invulnerabilidad) 
        {
            return;
        }

        Invulnerabilidad = true;
        Invoke("ResetVulnerabilidad", 0.5f);
        Vida -= dano;
        _Personaje.Movimiento.GetComponent<SpriteRenderer>().color = Color.red;
        
    }

    public void ResetVulnerabilidad()
    {
        Invulnerabilidad = false;
        _Personaje.Movimiento.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
