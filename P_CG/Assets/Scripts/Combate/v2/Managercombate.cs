using UnityEngine;

using System.Collections.Generic;

public class ManagerCombate : MonoBehaviour
{
    private LanzadorDados dados;

    // Los 4 héroes del juego
    public List<EstadisticasPersonaje> heroes = new List<EstadisticasPersonaje>();

    // Enemigos generados para este encuentro
    public List<EstadisticasPersonaje> enemigos = new List<EstadisticasPersonaje>();

    // Cola de turnos: orden de ataque por ronda
    private Queue<EstadisticasPersonaje> colaDeTurnos = new Queue<EstadisticasPersonaje>();

    // Stack (pila) con el log de los últimos eventos del combate
    private Stack<string> logCombate = new Stack<string>();

    private bool combateTerminado = false;

    void Start()
    {
        dados = GetComponent<LanzadorDados>();

        // Limpiar las listas por si Unity dejó elementos vacíos desde el Inspector
        heroes.Clear();
        enemigos.Clear();

        CrearHeroesDePrueba();
        GenerarEnemigosAleatorios();
        IniciarCombate();
    }

    // Crea los 4 héroes con sus stats exactos del GDD
    void CrearHeroesDePrueba()
    {
        // Héroe 1 — Fuerza 19, Vida 68, Dados: D4, D6, D10
        GameObject obj1 = new GameObject("Heroe1");
        EstadisticasPersonaje h1 = obj1.AddComponent<EstadisticasPersonaje>();
        h1.nombrePersonaje = "Héroe 1";
        h1.fuerza = 19;
        h1.resistencia = 100;
        h1.vidaMaxima = 68;
        h1.vidaActual = 68;
        h1.estaVivo = true;
        h1.nombresAtaques = new string[] { "Ataque Básico", "Ataque Doble" };
        h1.formulasAtaques = new string[] { "1D10+1D4", "2D6" };
        heroes.Add(h1);

        // Héroe 2 — Fuerza 23, Vida 87, Dados: D4, D10
        GameObject obj2 = new GameObject("Heroe2");
        EstadisticasPersonaje h2 = obj2.AddComponent<EstadisticasPersonaje>();
        h2.nombrePersonaje = "Héroe 2";
        h2.fuerza = 23;
        h2.resistencia = 100;
        h2.vidaMaxima = 87;
        h2.vidaActual = 87;
        h2.estaVivo = true;
        h2.nombresAtaques = new string[] { "Golpe Simple", "Golpe Fuerte", "Combo Triple" };
        h2.formulasAtaques = new string[] { "1D10", "1D10+1D4", "1D10+2D4" };
        heroes.Add(h2);

        // Héroe 3 — Fuerza 17, Vida 50, Dados: D6, D10
        GameObject obj3 = new GameObject("Heroe3");
        EstadisticasPersonaje h3 = obj3.AddComponent<EstadisticasPersonaje>();
        h3.nombrePersonaje = "Héroe 3";
        h3.fuerza = 17;
        h3.resistencia = 100;
        h3.vidaMaxima = 50;
        h3.vidaActual = 50;
        h3.estaVivo = true;
        h3.nombresAtaques = new string[] { "Estocada" };
        h3.formulasAtaques = new string[] { "1D10+1D6" };
        heroes.Add(h3);

        // Héroe 4 — Fuerza 16, Vida 38, Dados: D6, D8
        GameObject obj4 = new GameObject("Heroe4");
        EstadisticasPersonaje h4 = obj4.AddComponent<EstadisticasPersonaje>();
        h4.nombrePersonaje = "Héroe 4";
        h4.fuerza = 16;
        h4.resistencia = 100;
        h4.vidaMaxima = 38;
        h4.vidaActual = 38;
        h4.estaVivo = true;
        h4.nombresAtaques = new string[] { "Corte" };
        h4.formulasAtaques = new string[] { "1D6+1D8" };
        heroes.Add(h4);

        Debug.Log("4 héroes creados correctamente.");
    }

    // Genera entre n-1 y n+1 enemigos según los héroes vivos
    void GenerarEnemigosAleatorios()
    {
        int n = ContarHeroesVivos();
        int minimo = Mathf.Max(1, n - 1);
        int maximo = n + 1;
        int cantidad = Random.Range(minimo, maximo + 1);

        Debug.Log("Héroes activos: " + n + " → Enemigos a generar: " + cantidad + " (rango " + minimo + "-" + maximo + ")");

        for (int i = 0; i < cantidad; i++)
        {
            int tipo = Random.Range(1, 5);
            EstadisticasPersonaje enemigo = CrearEnemigoDeTipo(tipo, i + 1);
            enemigos.Add(enemigo);
        }
    }

    // Crea un enemigo del tipo indicado con sus stats del GDD
    EstadisticasPersonaje CrearEnemigoDeTipo(int tipo, int numero)
    {
        GameObject obj = new GameObject("Enemigo_" + numero);
        EstadisticasPersonaje e = obj.AddComponent<EstadisticasPersonaje>();
        e.resistencia = 100;
        e.estaVivo = true;

        if (tipo == 1)
        {
            e.nombrePersonaje = "Bestia del Bosque " + numero;
            e.fuerza = 15;
            e.vidaMaxima = 13;
            e.vidaActual = 13;
            e.nombresAtaques = new string[] { "Zarpazo" };
            e.formulasAtaques = new string[] { "1D6" };
        }
        else if (tipo == 2)
        {
            e.nombrePersonaje = "Sombra Oscura " + numero;
            e.fuerza = 18;
            e.vidaMaxima = 21;
            e.vidaActual = 21;
            e.nombresAtaques = new string[] { "Golpe Sombrío" };
            e.formulasAtaques = new string[] { "1D10" };
        }
        else if (tipo == 3)
        {
            e.nombrePersonaje = "Espíritu Errante " + numero;
            e.fuerza = 12;
            e.vidaMaxima = 23;
            e.vidaActual = 23;
            e.nombresAtaques = new string[] { "Toque Gélido" };
            e.formulasAtaques = new string[] { "1D4" };
        }
        else
        {
            e.nombrePersonaje = "Demonio Menor " + numero;
            e.fuerza = 19;
            e.vidaMaxima = 35;
            e.vidaActual = 35;
            e.nombresAtaques = new string[] { "Embestida" };
            e.formulasAtaques = new string[] { "1D10" };
        }

        return e;
    }

    void IniciarCombate()
    {
        Debug.Log("===========================================");
        Debug.Log("         EL DESPERTAR DE LA BESTIA        ");
        Debug.Log("            ¡COMBATE INICIADO!            ");
        Debug.Log("===========================================");

        MostrarEstadoActual();
        DeterminarOrdenDeTurnos();
        ProcesarSiguienteTurno();
    }

    void MostrarEstadoActual()
    {
        Debug.Log("--- HÉROES ---");
        foreach (EstadisticasPersonaje h in heroes)
        {
            if (h.estaVivo)
            {
                Debug.Log("  " + h.nombrePersonaje
                    + " | Fuerza: " + h.fuerza
                    + " | Resistencia: " + h.resistencia
                    + " | Vida: " + h.vidaActual + "/" + h.vidaMaxima);
            }
        }

        Debug.Log("--- ENEMIGOS ---");
        foreach (EstadisticasPersonaje e in enemigos)
        {
            if (e.estaVivo)
            {
                Debug.Log("  " + e.nombrePersonaje
                    + " | Fuerza: " + e.fuerza
                    + " | Resistencia: " + e.resistencia
                    + " | Vida: " + e.vidaActual + "/" + e.vidaMaxima);
            }
        }
    }

    void DeterminarOrdenDeTurnos()
    {
        Debug.Log("--- INICIATIVA (cada uno lanza 1D10) ---");

        List<EstadisticasPersonaje> todos = new List<EstadisticasPersonaje>();
        todos.AddRange(heroes);
        todos.AddRange(enemigos);

        Dictionary<EstadisticasPersonaje, int> iniciativas = new Dictionary<EstadisticasPersonaje, int>();

        foreach (EstadisticasPersonaje p in todos)
        {
            if (p.estaVivo)
            {
                int tirada = dados.LanzarD10();
                iniciativas[p] = tirada;
                Debug.Log("  " + p.nombrePersonaje + " sacó " + tirada);
            }
        }

        todos.Sort((a, b) =>
        {
            if (!iniciativas.ContainsKey(a)) return 1;
            if (!iniciativas.ContainsKey(b)) return -1;
            return iniciativas[b].CompareTo(iniciativas[a]);
        });

        colaDeTurnos.Clear();
        foreach (EstadisticasPersonaje p in todos)
        {
            if (p.estaVivo)
            {
                colaDeTurnos.Enqueue(p);
            }
        }

        Debug.Log("--- ORDEN DE TURNOS ---");
        foreach (EstadisticasPersonaje p in colaDeTurnos)
        {
            Debug.Log("  → " + p.nombrePersonaje);
        }
    }

    void ProcesarSiguienteTurno()
    {
        if (combateTerminado) return;

        if (colaDeTurnos.Count == 0)
        {
            Debug.Log("");
            Debug.Log("========== NUEVA RONDA ==========");
            MostrarEstadoActual();
            DeterminarOrdenDeTurnos();
        }

        EstadisticasPersonaje turnoActual = colaDeTurnos.Dequeue();

        if (!turnoActual.estaVivo)
        {
            ProcesarSiguienteTurno();
            return;
        }

        Debug.Log("");
        Debug.Log(">>> Turno de: " + turnoActual.nombrePersonaje
            + " (Fuerza: " + turnoActual.fuerza
            + " | Vida: " + turnoActual.vidaActual + "/" + turnoActual.vidaMaxima + ")");

        bool puedeAtacar = dados.TiradaDeExito();

        if (!puedeAtacar)
        {
            string msg = turnoActual.nombrePersonaje + " pierde su turno.";
            logCombate.Push(msg);
            Debug.Log(msg);
        }
        else
        {
            bool esHeroe = heroes.Contains(turnoActual);

            if (esHeroe)
            {
                EjecutarAtaqueHeroe(turnoActual);
            }
            else
            {
                EjecutarAtaqueEnemigo(turnoActual);
            }
        }

        if (VerificarFinCombate()) return;

        ProcesarSiguienteTurno();
    }

    void EjecutarAtaqueHeroe(EstadisticasPersonaje heroe)
    {
        EstadisticasPersonaje objetivo = ElegirEnemigoVivo();
        if (objetivo == null) return;

        // Elegir un ataque aleatorio del héroe
        int indiceAtaque = Random.Range(0, heroe.formulasAtaques.Length);
        string nombreAtaque = heroe.nombresAtaques[indiceAtaque];
        string formula = heroe.formulasAtaques[indiceAtaque];

        Debug.Log("  " + heroe.nombrePersonaje + " usa [" + nombreAtaque + "] contra " + objetivo.nombrePersonaje);
        Debug.Log("  Fuerza del atacante: " + heroe.fuerza + " (se usará en la fórmula de daño cuando el profesor la defina)");
        Debug.Log("  Lanzando dados propios (" + formula + "):");

        // Primero lanza los dados propios del personaje (su fórmula)
        int resultadoFormula = dados.LanzarFormula(formula);
        Debug.Log("  Resultado de la fórmula " + formula + ": " + resultadoFormula);

        // Luego la tirada combinada decide si es pifia, daño o fallo
        Debug.Log("  Lanzando tirada combinada para determinar resultado:");
        int combinado = dados.TiradaCombinada();
        string tipoGolpe = dados.EvaluarTiradaDanio(combinado);

        AplicarResultado(heroe, objetivo, tipoGolpe);
    }

    void EjecutarAtaqueEnemigo(EstadisticasPersonaje enemigo)
    {
        EstadisticasPersonaje objetivo = ElegirHeroeVivo();
        if (objetivo == null) return;

        string formula = enemigo.formulasAtaques[0];
        string nombreAtaque = enemigo.nombresAtaques[0];

        Debug.Log("  " + enemigo.nombrePersonaje + " usa [" + nombreAtaque + "] contra " + objetivo.nombrePersonaje);
        Debug.Log("  Fuerza del enemigo: " + enemigo.fuerza + " (se usará en la fórmula de daño cuando el profesor la defina)");
        Debug.Log("  Lanzando dados propios (" + formula + "):");

        int resultadoFormula = dados.LanzarFormula(formula);
        Debug.Log("  Resultado de la fórmula " + formula + ": " + resultadoFormula);

        Debug.Log("  Lanzando tirada combinada para determinar resultado:");
        int combinado = dados.TiradaCombinada();
        string tipoGolpe = dados.EvaluarTiradaDanio(combinado);

        AplicarResultado(enemigo, objetivo, tipoGolpe);
    }

    void AplicarResultado(EstadisticasPersonaje atacante, EstadisticasPersonaje objetivo, string tipoGolpe)
    {
        string mensaje = "";

        if (tipoGolpe == "danio")
        {
            // Daño fijo en 25 por ahora
            // Cuando el profesor defina la fórmula de fuerza y resistencia, se cambia aquí
            int danioFinal = 25;

            objetivo.RecibirDanio(danioFinal);

            mensaje = atacante.nombrePersonaje + " hizo " + danioFinal + " de daño a "
                + objetivo.nombrePersonaje + ". Vida restante: " + objetivo.vidaActual + "/" + objetivo.vidaMaxima;

            if (!objetivo.estaVivo)
            {
                mensaje += " — ¡DERROTADO!";
                if (enemigos.Contains(objetivo))
                {
                    MostrarLoot(objetivo);
                }
            }
        }
        else if (tipoGolpe == "pifia")
        {
            mensaje = "¡PIFIA! " + atacante.nombrePersonaje + " falló estrepitosamente. Sin daño.";
        }
        else
        {
            mensaje = atacante.nombrePersonaje + " FALLÓ el ataque. Sin daño.";
        }

        logCombate.Push(mensaje);
        Debug.Log("  " + mensaje);
    }

    void MostrarLoot(EstadisticasPersonaje enemigo)
    {
        string loot = "";

        if (enemigo.nombrePersonaje.Contains("Bestia del Bosque"))
            loot = "3 pociones + 100 de dinero";
        else if (enemigo.nombrePersonaje.Contains("Sombra Oscura"))
            loot = "1 cofre de tesoro";
        else if (enemigo.nombrePersonaje.Contains("Espíritu Errante"))
            loot = "4 pociones + 250 de dinero";
        else if (enemigo.nombrePersonaje.Contains("Demonio Menor"))
            loot = "1 poción + 350 de dinero";

        if (loot != "")
        {
            Debug.Log("  [LOOT] " + enemigo.nombrePersonaje + " dejó: " + loot);
            logCombate.Push("Loot de " + enemigo.nombrePersonaje + ": " + loot);
        }
    }

    EstadisticasPersonaje ElegirEnemigoVivo()
    {
        List<EstadisticasPersonaje> vivos = new List<EstadisticasPersonaje>();
        foreach (EstadisticasPersonaje e in enemigos)
        {
            if (e.estaVivo) vivos.Add(e);
        }
        if (vivos.Count == 0) return null;
        return vivos[Random.Range(0, vivos.Count)];
    }

    EstadisticasPersonaje ElegirHeroeVivo()
    {
        List<EstadisticasPersonaje> vivos = new List<EstadisticasPersonaje>();
        foreach (EstadisticasPersonaje h in heroes)
        {
            if (h.estaVivo) vivos.Add(h);
        }
        if (vivos.Count == 0) return null;
        return vivos[Random.Range(0, vivos.Count)];
    }

    int ContarHeroesVivos()
    {
        int contador = 0;
        foreach (EstadisticasPersonaje h in heroes)
        {
            if (h.estaVivo) contador++;
        }
        return contador;
    }

    bool VerificarFinCombate()
    {
        bool enemigosMuertos = true;
        bool heroesMuertos = true;

        foreach (EstadisticasPersonaje e in enemigos)
        {
            if (e.estaVivo) { enemigosMuertos = false; break; }
        }

        foreach (EstadisticasPersonaje h in heroes)
        {
            if (h.estaVivo) { heroesMuertos = false; break; }
        }

        if (enemigosMuertos)
        {
            combateTerminado = true;
            Debug.Log("");
            Debug.Log("===========================================");
            Debug.Log("  ¡VICTORIA! Todos los enemigos cayeron.  ");
            Debug.Log("===========================================");
            MostrarLogFinal();
            return true;
        }

        if (heroesMuertos)
        {
            combateTerminado = true;
            Debug.Log("");
            Debug.Log("===========================================");
            Debug.Log("   DERROTA. Todos los héroes cayeron.     ");
            Debug.Log("===========================================");
            MostrarLogFinal();
            return true;
        }

        return false;
    }

    void MostrarLogFinal()
    {
        Debug.Log("--- ÚLTIMOS EVENTOS DEL COMBATE ---");
        int mostrados = 0;
        while (logCombate.Count > 0 && mostrados < 12)
        {
            Debug.Log("  " + logCombate.Pop());
            mostrados++;
        }
    }
}