using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContenedorObjeto : MonoBehaviour
{
    public Objeto Objeto;
    public TMP_Text Cantidad;
    public Image Imagen;

    public void Actualizar()
    {
        Imagen.gameObject.SetActive(!Objeto.Nombre.IsNullOrWhitespace());
        Cantidad.gameObject.SetActive(!Objeto.Nombre.IsNullOrWhitespace());
    }
}
