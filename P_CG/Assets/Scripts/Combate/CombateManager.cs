using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;

public class CombateManager : MonoBehaviour
{
    [Header("Eventod que escucha la UI")]
    public UnityEvent<string> escribirEnLog; //conecta el UIManager

    public UnityEvent<EstadisticasPersonaje> iniciarTurno; // Resalta al personaje

    public UnityEvent<EstadisticasPersonaje, int, string> hacerDaño; // Muestra daño o fallos

    public UnityEvent<bool> terminarCombate; //Si gana o pierde cambia la pantalla

    //Combate interno

    private Queue<EstadisticasPersonaje> colaDeTurnos = new Queue<EstadisticasPersonaje>();

    //Listas
    private List<EstadisticasPersonaje> heroes = new List<EstadisticasPersonaje>();
    private List<EstadisticasPersonaje> enemigos = new List<EstadisticasPersonaje>();

    private bool combateActivo = false;

    //Entrada
    public void IniciarCombate(List<EstadisticasPersonaje> listaHeroes,
        List<EstadisticasPersonaje> listaEnemigos)
    {
        heroes=listaHeroes;
        int cantidadHeroesVivos = 0;
        for (int i = 0; i < listaHeroes.Count; i++)
        {
            if (listaHeroes[i].EstaVivo())
            {
                cantidadHeroesVivos++; 
            }
        }

        int minimoEnemigos = cantidadHeroesVivos - 1;
        int maximoEnemigos = cantidadHeroesVivos + 1;

        if (minimoEnemigos < 1)
        {
            minimoEnemigos = 1;
        }
    }
}
