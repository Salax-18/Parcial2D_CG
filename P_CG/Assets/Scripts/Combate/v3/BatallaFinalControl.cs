using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BatallaFinalControl : CombateControl
{
    private Character lupus;
    private Character helena;

    protected override void Inicializar()
    {
        // Buscar por nombre en lugar de índice fijo
        foreach (Transform hijo in enemigos.transform)
        {
            if (hijo.name == "Lupus")
                lupus = hijo.GetComponent<Character>();
            else if (hijo.name == "Helena")
                helena = hijo.GetComponent<Character>();
        }

        if (lupus == null) Debug.LogError("No se encontró Lupus en la jerarquía");
        if (helena == null) Debug.LogError("No se encontró Helena en la jerarquía");

        base.Inicializar();
    }

    private bool HelenaViva => helena != null && helena.gameObject != null;

    // ── Sobreescribir turno de enemigos ──────────────────────────────────────
    protected override IEnumerator AtaqueEnemigo(int idx)
    {
        yield return StartCoroutine(TurnoLupus());
    }

    // ── Lógica de Lupus ──────────────────────────────────────────────────────
    IEnumerator TurnoLupus()
    {
        if (lupus == null) yield break;

        int tirada = Random.Range(1, 11);
        Debug.Log($"Lupus — tirada de éxito: {tirada}");
        yield return new WaitForSecondsRealtime(0.5f);

        bool lupusPuedeAtacar;
        bool helenaAtacaExtra;

        if (tirada >= 8 && tirada <= 9)
        {
            if (HelenaViva)
            {
                // Helena viva + 8-9 → Lupus ataca normal, Helena no hace nada extra
                Debug.Log($"Lupus tirada {tirada} (8-9) — sigue orden, Helena no ataca extra");
                lupusPuedeAtacar = true;
                helenaAtacaExtra = false;
            }
            else
            {
                // Helena muerta + 8-9 → Lupus NO puede atacar
                Debug.Log($"Lupus tirada {tirada} (8-9) — Helena muerta, Lupus bloqueado");
                lupusPuedeAtacar = false;
                helenaAtacaExtra = false;
            }
        }
        else // 1-7 o 10
        {
            // Éxito — Lupus ataca y Helena ataca extra si vive
            Debug.Log($"Lupus tirada {tirada} — éxito{(HelenaViva ? " + Helena ataca extra" : "")}");
            lupusPuedeAtacar = true;
            helenaAtacaExtra = HelenaViva;
        }

        // ── Ataque de Lupus ──────────────────────────────────────────────────
        if (lupusPuedeAtacar)
        {
            int d1 = Random.Range(1, 10);
            int d2 = Random.Range(1, 10);
            int combinado = d1 * 10 + d2;
            Debug.Log($"Lupus — dados combinados: {d1} y {d2} → {combinado}");
            yield return new WaitForSecondsRealtime(0.5f);

            if (combinado > 50 && combinado < 90)
            {
                int ataqueAleatorio = Random.Range(0, lupus.data.ataques.Length);
                Debug.Log($"Lupus — ataque exitoso ({combinado}), usando {lupus.data.ataques[ataqueAleatorio].nombreAtaque}");
                yield return StartCoroutine(AplicarDanoEnemigo(lupus, ataqueAleatorio, PlayerSelect));
            }
            else
            {
                Debug.Log($"Lupus — fuera de rango ({combinado})");
            }
        }

        // ── Ataque extra de Helena (player aleatorio) ────────────────────────
        if (helenaAtacaExtra)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            int playerAleatorio = Random.Range(0, cantidadPlayers);
            Debug.Log($"Helena — ataque fijo al Player {playerAleatorio}");
            yield return StartCoroutine(AplicarDanoEnemigo(helena, 0, playerAleatorio));
        }
    }

    // ── Calcular y aplicar daño de enemigo a player ──────────────────────────
    IEnumerator AplicarDanoEnemigo(Character enemigo, int indiceAtaque, int indicePlayer)
    {
        if (enemigo == null || indiceAtaque >= enemigo.data.ataques.Length) yield break;
        if (indicePlayer >= cantidadPlayers) yield break;

        var ataque = enemigo.data.ataques[indiceAtaque];
        int totalBase = 0;
        string log = $"<b>{enemigo.data.nombrePersonaje}: {ataque.nombreAtaque}</b>\n";

        foreach (var dado in ataque.dados)
            for (int i = 0; i < dado.cantidadDados; i++)
            {
                int resultado = Random.Range(1, dado.caras + 1);
                totalBase += resultado;
                log += $"d{dado.caras}: {resultado}\n";
            }

        int totalConFuerza = Mathf.RoundToInt(totalBase * (1f + enemigo.data.fuerza / 1000f));
        log += $"Base: {totalBase} | Fuerza → <b>{totalConFuerza}</b>";

        CombateUI.Instance.MostrarResultado(log);
        yield return new WaitForSecondsRealtime(2f);
        CombateUI.Instance.OcultarResultado();

        players.transform.GetChild(indicePlayer).GetComponent<Character>().Damage(totalConFuerza);
    }

    // ── Fin de batalla final ─────────────────────────────────────────────────
    protected override void VerificarYAvanzar()
    {
        if (cantidadEnemigos <= 0)
        {
            Debug.Log("=== BATALLA FINAL GANADA ===");
            SceneManager.LoadScene("Victoria");
            return;
        }

        base.VerificarYAvanzar();
    }
}