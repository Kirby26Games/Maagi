using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject MenuPausa;
    bool _Pausado = false;
    public bool PuedoPausar;

    private void Start()
    {
        MenuPausa.SetActive(false);
        _Pausado = false;
        Debug.Log($"Puedo Pausar es {PuedoPausar}");
    }

    void Update()
    {
        if (Input.GetKeyDown(Controles.Menu) && PuedoPausar)
        {
            Pausar();
        }
    }

    void Pausar()
    {
        MenuPausa.SetActive(!_Pausado);
        _Pausado = !_Pausado;
    }
}
