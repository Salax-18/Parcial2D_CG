using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    GameObject objetivos;
    CombateControl combateControl;

    public CharacterData data;
    public int vidaActual;
    int objetivo;
    public bool tipo;

    public BarraVida barraVida; // asignar en el Inspector

    private void Start()
    {
        combateControl = GameObject.Find("CombateControl").GetComponent<CombateControl>();
        objetivos = tipo ? GameObject.Find("enemigos") : GameObject.Find("players");

        if (data != null)
        {
            vidaActual = data.vidaMaxima;
            barraVida?.Actualizar(vidaActual, data.vidaMaxima); // inicializar barra
        }
    }

    public void Atacar(bool sinDaño = false)
    {
        // Sin animación de movimiento
        if (tipo) objetivo = combateControl.EnemySelect;
        else objetivo = combateControl.PlayerSelect;

        if (combateControl.cantidadEnemigos > 0 && combateControl.cantidadPlayers > 0)
        {
            if (!sinDaño)
                objetivos.transform.GetChild(objetivo).GetComponent<Character>().Damage(TirarAtaque(0));
            else
                Debug.Log("Ataque sin daño");
        }
    }


    public void Damage(int damage)
    {
        int dañoFinal = Mathf.RoundToInt(damage * (1f - data.defensa / 1000f));
        vidaActual -= dañoFinal;
        vidaActual = Mathf.Max(vidaActual, 0); // nunca menor a 0
        Debug.Log($"{data.nombrePersonaje} recibió {damage} → {dañoFinal} tras defensa ({data.defensa}%)");

        barraVida?.Actualizar(vidaActual, data.vidaMaxima); // actualizar barra
        StartCoroutine(AnimDamage());

        if (vidaActual <= 0)
        {
            if (tipo) combateControl.cantidadEnemigos--;
            else combateControl.cantidadPlayers--;
            if (tipo && data != null) SoltarLoot();
            Destroy(gameObject);
        }
    }

    public int TirarAtaque(int indiceAtaque)
    {
        if (data == null || data.ataques.Length == 0) return 0;
        indiceAtaque = Mathf.Clamp(indiceAtaque, 0, data.ataques.Length - 1);

        int total = 0;
        foreach (var dado in data.ataques[indiceAtaque].dados)
            for (int i = 0; i < dado.cantidadDados; i++)
                total += Random.Range(1, dado.caras + 1);

        int totalConFuerza = Mathf.RoundToInt(total * (1f + data.fuerza / 100f));
        Debug.Log($"{data.nombrePersonaje} usó {data.ataques[indiceAtaque].nombreAtaque}: {total} → {totalConFuerza}");
        return totalConFuerza;
    }

    void SoltarLoot()
    {
        foreach (var item in data.lootPosible)
            if (Random.value <= item.probabilidad)
                Inventario.Instance.AgregarItem(item.nombreItem);
    }

    Coroutine titilarCoroutine;
    public void Select(bool activo)
    {
        if (activo)
        {
            titilarCoroutine = StartCoroutine(TitilarLoop());
        }
        else
        {
            if (titilarCoroutine != null) StopCoroutine(titilarCoroutine);
            foreach (var r in GetComponentsInChildren<SpriteRenderer>())
                r.enabled = true;
        }
    }

    IEnumerator TitilarLoop()
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        while (true)
        {
            foreach (var r in renderers) r.enabled = !r.enabled;
            yield return new WaitForSecondsRealtime(0.3f);
        }
    }

    IEnumerator AnimDamage()
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (var r in renderers) r.enabled = false;
        yield return new WaitForSecondsRealtime(0.5f);
        foreach (var r in renderers) r.enabled = true;
    }
}