using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContenedorObjeto : MonoBehaviour
{
    public int ID;
    public string Nombre;
    public int Cantidad;
    public TMP_Text CantidadTxt;
    public Image Imagen;

    public void Actualizar()
    {   
        CantidadTxt.text = Cantidad.ToString();

        //Si el contenedor del inventario no tiene un objeto asignado pues se desactiva
        Imagen.gameObject.SetActive(ID != 0);
        CantidadTxt.gameObject.SetActive(ID != 0);
    }
}
