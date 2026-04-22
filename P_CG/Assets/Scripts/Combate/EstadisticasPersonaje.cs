using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EstadisticasPersonaje
{
    [Header("Información básica")]
    public string nombre;
    public bool esHeroe;     // true = héroe, false = enemigo
    public int fuerza;
    public int hpMaximo;
    public int hpActual;

    [Header("Ataques disponibles")]
    public List<DatosAtaque> ataques;

    [Header("Apariencia (opcional por ahora)")]
    public Sprite spriteIdle;

    // Propiedades de ayuda
    // Devuelve true si el personaje sigue vivo
    public bool EstaVivo()
    {
        return hpActual > 0;
    }
    // Le resta HP al personaje. Nunca baja de 0.
    public void RecibirDaño(int cantidad)
    {
        hpActual = hpActual - cantidad;
        if (hpActual < 0) hpActual = 0;
    }

    // Le sube HP al personaje. Nunca sube del máximo.
    public void CurarHP(int cantidad)
    {
        hpActual = hpActual + cantidad;
        if (hpActual > hpMaximo) hpActual = hpMaximo;
    }
}

// Cada ataque tiene un nombre y una fórmula de dados para mostrar en la UI
[System.Serializable]
public class DatosAtaque
{
    public string nombre;
    public string formula;  // Ejemplo: "1D10+1D4", "2D6"
}