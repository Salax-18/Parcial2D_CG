using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombateControl : MonoBehaviour
{
    public int cantidadEnemigos = 4, cantidadPlayers = 4;
    public int EnemySelect, PlayerSelect;
    public GameObject enemigos, players;

    private bool esperandoInputJugador = false;
    private bool resolviendoTurno = false;
    private bool combateIniciado = false;
    private bool playerFuePrimero = false; // resultado de iniciativa de este turno

    // ── Run ──────────────────────────────────────────────────────────────────
    private void Start()
    {
        // Titila el player 0 y el enemigo 0 al inicio
        players.transform.GetChild(PlayerSelect).GetComponent<Character>().Select(true);
        enemigos.transform.GetChild(EnemySelect).GetComponent<Character>().Select(true);
        esperandoInputJugador = true; // listo para elegir enemigo
    }

    private void Update()
    {
        if (!esperandoInputJugador || cantidadEnemigos <= 0) return;

        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            enemigos.transform.GetChild(EnemySelect).GetComponent<Character>().Select(false);
            EnemySelect = Mathf.Clamp(EnemySelect - 1, 0, cantidadEnemigos - 1);
            enemigos.transform.GetChild(EnemySelect).GetComponent<Character>().Select(true);
        }
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            enemigos.transform.GetChild(EnemySelect).GetComponent<Character>().Select(false);
            EnemySelect = Mathf.Clamp(EnemySelect + 1, 0, cantidadEnemigos - 1);
            enemigos.transform.GetChild(EnemySelect).GetComponent<Character>().Select(true);
        }
    }

    // ── Botón Atacar ─────────────────────────────────────────────────────────
    public void Atacar()
    {
        if (!esperandoInputJugador || resolviendoTurno) return;

        // Apagar titileo de ambos mientras se resuelve
        players.transform.GetChild(PlayerSelect).GetComponent<Character>().Select(false);
        enemigos.transform.GetChild(EnemySelect).GetComponent<Character>().Select(false);

        esperandoInputJugador = false;
        resolviendoTurno = true;

        if (!combateIniciado)
        {
            combateIniciado = true;
            StartCoroutine(ResolverIniciativaYTurno());
        }
        else
        {
            StartCoroutine(TurnoDelPlayer());
        }
    }

    // ── Iniciativa (solo primer turno de la ronda) ───────────────────────────
    IEnumerator ResolverIniciativaYTurno()
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

        playerFuePrimero = dadoPlayer > dadoEnemigo;
        Debug.Log(playerFuePrimero ? "Player va primero" : "Enemigo va primero");

        if (playerFuePrimero)
        {
            // Player ataca → luego enemigo responde
            yield return StartCoroutine(AtaquePlayer());
            yield return StartCoroutine(AtaqueEnemigo(EnemySelect));
        }
        else
        {
            // Enemigo ataca → luego player responde
            yield return StartCoroutine(AtaqueEnemigo(EnemySelect));
            yield return StartCoroutine(AtaquePlayer());
        }

        resolviendoTurno = false;
        VerificarYAvanzar();
    }

    // ── Turno normal (sin tirar iniciativa) ──────────────────────────────────
    IEnumerator TurnoDelPlayer()
    {
        if (playerFuePrimero)
        {
            yield return StartCoroutine(AtaquePlayer());
            yield return StartCoroutine(AtaqueEnemigo(EnemySelect));
        }
        else
        {
            yield return StartCoroutine(AtaqueEnemigo(EnemySelect));
            yield return StartCoroutine(AtaquePlayer());
        }

        resolviendoTurno = false;
        VerificarYAvanzar();
    }

    // ── Lógica de ataque del player ──────────────────────────────────────────
    // Diagrama: lanza 1 dado (1-10). Si dado >= 4 y <= 10 → puede atacar. Si no → Falla.
    // Si puede atacar: lanza 2 dados, concatena como texto → si >50 y <90 → realiza daño.
    IEnumerator AtaquePlayer()
    {
        Character player = players.transform.GetChild(PlayerSelect).GetComponent<Character>();

        int dado = Random.Range(1, 11);
        Debug.Log($"Player {PlayerSelect} — dado de éxito: {dado}");
        yield return new WaitForSecondsRealtime(0.5f);

        if (dado < 4) // Falla (1, 2 o 3)
        {
            Debug.Log($"Player {PlayerSelect} falló (dado {dado} < 4)");
            yield break;
        }

        // Lanza 2 dados y los concatena como texto (ej: 7 y 3 → 73)
        int d1 = Random.Range(1, 10);
        int d2 = Random.Range(1, 10);
        int combinado = d1 * 10 + d2;
        Debug.Log($"Player {PlayerSelect} — dados: {d1} y {d2} → {combinado}");
        yield return new WaitForSecondsRealtime(0.5f);

        if (combinado > 50 && combinado < 90)
        {
            Debug.Log($"Player {PlayerSelect} — ataque exitoso ({combinado}), eligiendo ataque...");
            yield return StartCoroutine(ElegirYAtacar(player));
        }
        else
        {
            Debug.Log($"Player {PlayerSelect} — ataque fallido por rango ({combinado})");
        }
    }

    // ── Lógica de ataque del enemigo ─────────────────────────────────────────
    IEnumerator AtaqueEnemigo(int idx)
    {
        if (idx >= cantidadEnemigos) yield break;

        Transform t = enemigos.transform.GetChild(idx);
        if (t == null) yield break;
        Character enemigo = t.GetComponent<Character>();
        if (enemigo == null) yield break;

        int dado = Random.Range(1, 11);
        Debug.Log($"Enemigo {idx} — dado de éxito: {dado}");
        yield return new WaitForSecondsRealtime(0.5f);

        if (dado < 4)
        {
            Debug.Log($"Enemigo {idx} falló (dado {dado} < 4)");
            yield break;
        }

        int d1 = Random.Range(1, 10);
        int d2 = Random.Range(1, 10);
        int combinado = d1 * 10 + d2;
        Debug.Log($"Enemigo {idx} — dados: {d1} y {d2} → {combinado}");
        yield return new WaitForSecondsRealtime(0.5f);

        if (combinado > 50 && combinado < 90)
        {
            Debug.Log($"Enemigo {idx} — ataque exitoso ({combinado})");
            enemigo.Atacar(sinDaño: false);
            yield return new WaitForSecondsRealtime(1f);
        }
        else
        {
            Debug.Log($"Enemigo {idx} — ataque fallido por rango ({combinado})");
        }
    }

    // ── UI: elegir ataque y aplicar daño ────────────────────────────────────
    IEnumerator ElegirYAtacar(Character player)
    {
        int ataqueElegido = -1;
        CombateUI.Instance.MostrarAtaques(player.data, (indice) => { ataqueElegido = indice; });
        yield return new WaitUntil(() => ataqueElegido >= 0);

        var ataque = player.data.ataques[ataqueElegido];
        string log = $"<b>{ataque.nombreAtaque}</b>\n";
        int totalBase = 0;

        foreach (var dado in ataque.dados)
            for (int i = 0; i < dado.cantidadDados; i++)
            {
                int resultado = Random.Range(1, dado.caras + 1);
                totalBase += resultado;
                log += $"d{dado.caras}: {resultado}\n";
            }

        int totalConFuerza = Mathf.RoundToInt(totalBase * (1f + player.data.fuerza / 100f));
        log += $"Total base: {totalBase}\nCon fuerza ({player.data.fuerza}%): <b>{totalConFuerza}</b>";

        CombateUI.Instance.MostrarResultado(log);
        yield return new WaitForSecondsRealtime(2f);
        CombateUI.Instance.OcultarResultado();

        if (EnemySelect < cantidadEnemigos)
            enemigos.transform.GetChild(EnemySelect).GetComponent<Character>().Damage(totalConFuerza);
    }

    // ── Verificar fin de combate o avanzar al siguiente player ───────────────
    // Diagrama: ¿Hay enemigos? → No → Terminar. Sí → ¿Es el último player? → No → Pasar al siguiente
    void VerificarYAvanzar()
    {
        // ¿Hay enemigos?
        if (cantidadEnemigos <= 0)
        {
            Debug.Log("=== COMBATE TERMINADO — sin enemigos ===");
            // Aquí cambiar de escena
            return;
        }

        // ¿Es el último player?
        if (PlayerSelect >= cantidadPlayers - 1)
        {
            // Fin de ronda — volver al primero, nueva iniciativa
            Debug.Log("=== FIN DE RONDA ===");
            players.transform.GetChild(PlayerSelect).GetComponent<Character>().Select(false);
            PlayerSelect = 0;
            combateIniciado = false; // próximo Atacar tirará iniciativa de nuevo
        }
        else
        {
            // Pasar al siguiente player
            players.transform.GetChild(PlayerSelect).GetComponent<Character>().Select(false);
            PlayerSelect++;
        }

        // Preparar siguiente turno
        PlayerSelect = Mathf.Clamp(PlayerSelect, 0, cantidadPlayers - 1);
        EnemySelect = Mathf.Clamp(EnemySelect, 0, cantidadEnemigos - 1);

        players.transform.GetChild(PlayerSelect).GetComponent<Character>().Select(true);
        enemigos.transform.GetChild(EnemySelect).GetComponent<Character>().Select(true);

        esperandoInputJugador = true;
        Debug.Log($">>> Player {PlayerSelect} listo — selecciona enemigo y presiona Atacar");
    }
}