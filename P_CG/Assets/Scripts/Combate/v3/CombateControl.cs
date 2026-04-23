using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombateControl : MonoBehaviour
{
    public int cantidadEnemigos = 4, cantidadPlayers = 4;
    public int EnemySelect, PlayerSelect;
    public GameObject enemigos, players;
    bool turno = true;
    bool resolviendoTurno = false;

    private void Start()
    {
        Character stats = players.transform.GetChild(PlayerSelect).GetComponent<Character>();
        stats.Select(true);
    }

    private void Update()
    {
        if (cantidadEnemigos >= 0)
        {
            if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                enemigos.transform.GetChild(EnemySelect).GetComponent<Character>().Select(false);
                EnemySelect--;
                EnemySelect = Mathf.Clamp(EnemySelect, 0, cantidadEnemigos - 1);
                enemigos.transform.GetChild(EnemySelect).GetComponent<Character>().Select(true);
            }
            if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                enemigos.transform.GetChild(EnemySelect).GetComponent<Character>().Select(false);
                EnemySelect++;
                EnemySelect = Mathf.Clamp(EnemySelect, 0, cantidadEnemigos - 1);
                enemigos.transform.GetChild(EnemySelect).GetComponent<Character>().Select(true);
            }
        }

        if (turno && !resolviendoTurno && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            resolviendoTurno = true;
            StartCoroutine(ResolverIniciativa());
        }
    }

    public void Atacar()
    {
        if (turno && cantidadPlayers >= 0)
        {
            Character character = players.transform.GetChild(PlayerSelect).GetComponent<Character>();
            character.Atacar();

            if (PlayerSelect == cantidadPlayers)
            {
                PlayerSelect = 0;
                turno = false;
                StartCoroutine(AtaqueEnemigos());

            }
            else
            {
                PlayerSelect++;

                character.Select(false);
                character = players.transform.GetChild(PlayerSelect).GetComponent<Character>();
                character.Select(true);
            }
        }
    }

    IEnumerator AtaqueEnemigos()
    {
        yield return new WaitForSecondsRealtime(1f);
        for (int i = 0; i < cantidadEnemigos; i++)
        {
            enemigos.transform.GetChild(i).GetComponent<Character>().Atacar();
            yield return new WaitForSecondsRealtime(1f);
        }
        turno = true;
    }

    IEnumerator ResolverIniciativa()
    {
        int dadoPlayer, dadoEnemigo;

        // Repetir en caso de empate
        do
        {
            dadoPlayer = Random.Range(1, 11);
            dadoEnemigo = Random.Range(1, 11);
            Debug.Log($"Player tiró: {dadoPlayer} | Enemigo tiró: {dadoEnemigo}");
            yield return new WaitForSecondsRealtime(0.5f);
        }
        while (dadoPlayer == dadoEnemigo);

        if (dadoPlayer > dadoEnemigo)
        {
            Debug.Log("Player ataca primero");
            Atacar(); // turno del player primero
        }
        else
        {
            Debug.Log("Enemigo ataca primero");
            StartCoroutine(AtaqueEnemigos()); // enemigos van primero
                                              // después del ataque enemigo, el player ataca
        }

        resolviendoTurno = false;
    }
}