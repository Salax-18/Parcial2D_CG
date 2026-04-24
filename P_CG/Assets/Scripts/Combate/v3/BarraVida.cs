using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image relleno;

    public void Actualizar(int vidaActual, int vidaMaxima)
    {
        relleno.fillAmount = (float)vidaActual / vidaMaxima;
    }
}