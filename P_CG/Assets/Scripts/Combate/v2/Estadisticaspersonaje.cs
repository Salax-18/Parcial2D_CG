using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EstadisticasPersonaje : MonoBehaviour
{
    public string nombrePersonaje = "Sin nombre";
    public int fuerza = 10;

    // La resistencia influirá en el daño recibido más adelante
    // Por ahora todos empiezan con 100 de vida máxima
    public int resistencia = 100;
    public int vidaMaxima = 100;
    public int vidaActual = 100;

    public bool estaVivo = true;

    // Nombres de los ataques que puede usar este personaje
    public string[] nombresAtaques;

    // Fórmulas de dados de cada ataque (ej: "1D10+1D4", "2D6")
    // Se usan para lanzar los dados propios del personaje
    public string[] formulasAtaques;

    void Start()
    {
        vidaActual = vidaMaxima;
        estaVivo = true;
    }

    public void RecibirDanio(int cantidad)
    {
        // Por ahora el daño es directo
        // Cuando el profesor defina la fórmula de resistencia, se modifica aquí
        vidaActual -= cantidad;

        if (vidaActual <= 0)
        {
            vidaActual = 0;
            estaVivo = false;
        }
    }

    public void CurarVida(int cantidad)
    {
        vidaActual += cantidad;

        if (vidaActual > vidaMaxima)
        {
            vidaActual = vidaMaxima;
        }
    }
}