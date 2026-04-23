using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    GameObject objetivos;
    CombateControl combateControl;
    public GameObject select;
    public SpriteRenderer sr;

    public int vida;
    public int ataque;
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
    }


    public void Atacar()
    {
        StartCoroutine(AnimAtaque());
        if (tipo) objetivo = combateControl.EnemySelect;
        else objetivo = combateControl.PlayerSelect;
        if (combateControl.cantidadEnemigos >= 0 && combateControl.cantidadPlayers >= 0)
            objetivos.transform.GetChild(objetivo).GetComponent<Character>().Damage(ataque);
    }

    public void Damage(int damage)
    {
        vida -= damage;
        StartCoroutine(AnimDamage(ataque));
        if (vida <= 0)
        {
            if (tipo) combateControl.cantidadEnemigos--;
            else combateControl.cantidadPlayers--;
            Destroy(gameObject);
        }
    }


    public void Select(bool select)
    {
        this.select.SetActive(select);
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
        sr.enabled = !sr.enabled;
        yield return new WaitForSecondsRealtime(0.5f);
    }

}
