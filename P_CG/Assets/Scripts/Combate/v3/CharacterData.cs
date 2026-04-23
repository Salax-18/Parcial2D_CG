using UnityEngine;

[System.Serializable]
public class AtaqueDado
{
    public string nombreAtaque;
    public DadoTiro[] dados;
}

[System.Serializable]
public class DadoTiro
{
    public int cantidadDados;
    public int caras;
}

[CreateAssetMenu(fileName = "NuevoPersonaje", menuName = "Combate/CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("Info")]
    public string nombrePersonaje;
    public Sprite icono;

    [Header("Stats Base")]
    public int vidaMaxima;
    [Range(0, 100)] public int defensa;
    [Range(0, 100)] public int fuerza;

    [Header("Ataques")]
    public AtaqueDado[] ataques;

    [Header("Loot (solo enemigos)")]
    public bool esEnemigo;
    public LootItem[] lootPosible;
}

[System.Serializable]
public class LootItem
{
    public string nombreItem;
    [Range(0f, 1f)]
    public float probabilidad;
}