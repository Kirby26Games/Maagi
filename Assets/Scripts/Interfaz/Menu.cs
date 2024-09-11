using UnityEngine;

public class Menu : MonoBehaviour
{
    public RectTransform MenuPausa;
    bool _Pausado = false;
    public bool PuedoPausar;

    private void Start()
    {
        _Pausado = false;
        MoverMenu();
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
        _Pausado = !_Pausado;
        MoverMenu();
    }

    void MoverMenu()
    {
        if (_Pausado)
        {
            MenuPausa.anchoredPosition = Vector3.zero;
        }
        else
        {
            MenuPausa.anchoredPosition = 4 * Screen.height * Vector3.up;
        }
    }
}
