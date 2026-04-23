using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    GameObject objetivos;
    CombateControl combateControl;

    public CharacterData data;
    public int vidaActual;
    int objetivo;

    public bool tipo;

    private void Start()
    {
        combateControl = GameObject.Find("CombateControl").GetComponent<CombateControl>();
        if (tipo)
        {
            objetivos = GameObject.Find("Enemigos");

        }
        else
        {
            objetivos = GameObject.Find("Players");
        }
        if (data != null)
            vidaActual = data.vidaMaxima;
    }


    public void Atacar()
    {
        StartCoroutine(AnimAtaque());
        if (tipo) objetivo = combateControl.EnemySelect;
        else objetivo = combateControl.PlayerSelect;
        if (combateControl.cantidadEnemigos >= 0 && combateControl.cantidadPlayers >= 0)
            objetivos.transform.GetChild(objetivo).GetComponent<Character>().Damage(TirarAtaque(0));
    }

    public void Atacar(bool sinDaño = false)
    {
        StartCoroutine(AnimAtaque());
        if (tipo) objetivo = combateControl.EnemySelect;
        else objetivo = combateControl.PlayerSelect;
        if (combateControl.cantidadEnemigos >= 0 && combateControl.cantidadPlayers >= 0)
        {
            if (!sinDaño)
                objetivos.transform.GetChild(objetivo).GetComponent<Character>().Damage(TirarAtaque(0));
            else
                Debug.Log("Ataque sin daño — solo animación");
        }
    }

    public void Damage(int damage)
    {
        // Aplicar defensa
        int dañoFinal = Mathf.RoundToInt(damage * (1f - data.defensa / 100f));
        vidaActual -= dañoFinal;
        Debug.Log($"{data.nombrePersonaje} recibió {damage} → {dañoFinal} tras defensa ({data.defensa}%)");

        StartCoroutine(AnimDamage(dañoFinal));
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

        // Aplicar fuerza
        int totalConFuerza = Mathf.RoundToInt(total * (1f + data.fuerza / 100f));
        Debug.Log($"{data.nombrePersonaje} usó {data.ataques[indiceAtaque].nombreAtaque}: {total} base → {totalConFuerza} con fuerza");
        return totalConFuerza;
    }

    void SoltarLoot()
    {
        foreach (var item in data.lootPosible)
        {
            if (Random.value <= item.probabilidad)
                Inventario.Instance.AgregarItem(item.nombreItem);
        }
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
            // Restaurar todos los renderers al apagar
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

    IEnumerator AnimAtaque()
    {
        float mov = 0.3f;
        if (!tipo) mov *= 1;
        transform.position = new Vector3(transform.position.x + mov, transform.position.y, transform.position.z);
        yield return new WaitForSecondsRealtime(0.2f);
        transform.position = new Vector3(transform.position.x - mov, transform.position.y, transform.position.z);
    }

    IEnumerator AnimDamage(float damage)
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (var r in renderers) r.enabled = false;
        yield return new WaitForSecondsRealtime(0.5f);
        foreach (var r in renderers) r.enabled = true;
    }

}
