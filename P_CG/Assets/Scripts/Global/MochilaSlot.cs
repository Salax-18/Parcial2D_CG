using UnityEngine;
using TMPro;

public class MochilaSlot : MonoBehaviour
{
    public string nombreDelHongo;
    public TextMeshProUGUI textoCantidad;

    public void ActualizarSlot()
    {
        if (Inventario.Instance != null)
        {
            // Buscamos en minúsculas para que no haya fallos de ortografía
            int cantidad = Inventario.Instance.ObtenerCantidad(nombreDelHongo);
            textoCantidad.text = cantidad.ToString();
        }
    }
}