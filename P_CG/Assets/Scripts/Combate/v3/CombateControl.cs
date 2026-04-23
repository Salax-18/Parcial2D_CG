using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class CombateControl : MonoBehaviour
{
    public int cantidadEnemigos = 4, cantidadPlayers = 4;
    public int EnemySelect, PlayerSelect;
    public GameObject enemigos, players;
    bool turno = true;

    private void Start()
    {
        Character stats = players.transform.GetChild(PlayerSelect).GetComponent<Character>();
        stats.Select(true);
    }

    private void Update()
    {
        if (cantidadEnemigos >= 0)
        {
            enemigos.transform.GetChild(EnemySelect).GetComponent<Character>().Select(false);
            if (Input.GetKeyDown(KeyCode.DownArrow))
                EnemySelect--;
            if (Input.GetKeyDown(KeyCode.UpArrow))
                EnemySelect++;
            EnemySelect = Mathf.Clamp(EnemySelect, 0, cantidadEnemigos);
            enemigos.transform.GetChild(EnemySelect).GetComponent<Character>().Select(true);
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
}