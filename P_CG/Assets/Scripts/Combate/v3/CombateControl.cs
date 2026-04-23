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
    }

    public void IniciarCombate()
    {
        Debug.Log($"IniciarCombate llamado — turno:{turno} resolviendoTurno:{resolviendoTurno}");
        if (turno && !resolviendoTurno)
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
        do
        {
            dadoPlayer = Random.Range(1, 11);
            dadoEnemigo = Random.Range(1, 11);
            Debug.Log($"Iniciativa — Player: {dadoPlayer} | Enemigo: {dadoEnemigo}");
            yield return new WaitForSecondsRealtime(0.5f);
        }
        while (dadoPlayer == dadoEnemigo);

        if (dadoPlayer > dadoEnemigo)
        {
            Debug.Log("Player ataca primero");
            StartCoroutine(TurnoPlayer(0));
        }
        else
        {
            Debug.Log("Enemigo ataca primero");
            StartCoroutine(TurnoEnemigos(true));
        }

        resolviendoTurno = false;
    }

    IEnumerator TurnoPlayer(int indicePlayer)
    {
        if (indicePlayer >= cantidadPlayers + 1)
        {
            StartCoroutine(TurnoEnemigos(false));
            yield break;
        }

        Character player = players.transform.GetChild(indicePlayer).GetComponent<Character>();

        players.transform.GetChild(PlayerSelect).GetComponent<Character>().Select(false);
        PlayerSelect = indicePlayer;
        player.Select(true);

        int tiradaExito = Random.Range(1, 11);
        Debug.Log($"Player {indicePlayer} tirada de éxito: {tiradaExito}");
        yield return new WaitForSecondsRealtime(0.5f);

        if (tiradaExito <= 3)
        {
            Debug.Log($"Player {indicePlayer} falló la tirada de éxito");
            StartCoroutine(TurnoEnemigos(true));
            yield break;
        }

        int dado1 = Random.Range(1, 10);
        int dado2 = Random.Range(1, 10);
        int combinado = dado1 * 10 + dado2;
        Debug.Log($"Player {indicePlayer} tirada de ataque: {dado1} y {dado2} → {combinado}");
        yield return new WaitForSecondsRealtime(0.5f);

        if (combinado < 50)
        {
            Debug.Log($"Player {indicePlayer} falló el ataque ({combinado} < 50)");
        }
        else if (combinado > 90)
        {
            Debug.Log($"Player {indicePlayer} atacó pero sin daño ({combinado} > 90)");
            player.Atacar(sinDaño: true);
        }
        else
        {
            Debug.Log($"Player {indicePlayer} ataca con daño ({combinado})");
            yield return StartCoroutine(ElegirYAtacar(player)); ;
        }

        yield return new WaitForSecondsRealtime(1f);
        StartCoroutine(TurnoPlayer(indicePlayer + 1));
    }

    IEnumerator ElegirYAtacar(Character player)
    {
        int ataqueElegido = -1;

        CombateUI.Instance.MostrarAtaques(player.data, (indice) => {
            ataqueElegido = indice;
        });

        yield return new WaitUntil(() => ataqueElegido >= 0);

        var ataque = player.data.ataques[ataqueElegido];
        string log = $"<b>{ataque.nombreAtaque}</b>\n";
        int totalBase = 0;

        foreach (var dado in ataque.dados)
        {
            for (int i = 0; i < dado.cantidadDados; i++)
            {
                int resultado = Random.Range(1, dado.caras + 1);
                totalBase += resultado;
                log += $"d{dado.caras}: {resultado}\n";
            }
        }

        int totalConFuerza = Mathf.RoundToInt(totalBase * (1f + player.data.fuerza / 100f));
        log += $"Total base: {totalBase}\nCon fuerza ({player.data.fuerza}%): <b>{totalConFuerza}</b>";

        CombateUI.Instance.MostrarResultado(log);
        yield return new WaitForSecondsRealtime(2f);
        CombateUI.Instance.OcultarResultado();

        Character objetivo = enemigos.transform.GetChild(EnemySelect).GetComponent<Character>();
        objetivo.Damage(totalConFuerza);
    }

    IEnumerator TurnoEnemigos(bool volverAPlayer)
    {
        for (int i = 0; i < cantidadEnemigos; i++)
        {
            Character enemigo = enemigos.transform.GetChild(i).GetComponent<Character>();

            int tiradaExito = Random.Range(1, 11);
            Debug.Log($"Enemigo {i} tirada de éxito: {tiradaExito}");
            yield return new WaitForSecondsRealtime(0.5f);

            if (tiradaExito <= 3)
            {
                Debug.Log($"Enemigo {i} falló la tirada de éxito");
                if (volverAPlayer)
                {
                    StartCoroutine(TurnoPlayer(PlayerSelect + 1));
                    yield break;
                }
                continue;
            }

            int dado1 = Random.Range(1, 10);
            int dado2 = Random.Range(1, 10);
            int combinado = dado1 * 10 + dado2;
            Debug.Log($"Enemigo {i} tirada de ataque: {dado1} y {dado2} → {combinado}");
            yield return new WaitForSecondsRealtime(0.5f);

            if (combinado < 50)
                Debug.Log($"Enemigo {i} falló el ataque ({combinado} < 50)");
            else if (combinado > 90)
            {
                Debug.Log($"Enemigo {i} atacó pero sin daño ({combinado} > 90)");
                enemigo.Atacar(sinDaño: true);
            }
            else
            {
                Debug.Log($"Enemigo {i} ataca con daño ({combinado})");
                enemigo.Atacar(sinDaño: false);
            }

            yield return new WaitForSecondsRealtime(1f);
        }

        turno = true;
    }
}