using UnityEngine;

[System.Serializable]
public class AtaqueDado
{
    public string nombreAtaque;
    public DadoTiro[] dados; // ej: 1d10 + 1d4
}

[System.Serializable]
public class DadoTiro
{
    public int cantidadDados;
    public int caras; // d4, d6, d8, d10, d12, d20
}

[CreateAssetMenu(fileName = "NuevoPersonaje", menuName = "Combate/CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("Info")]
    public string nombrePersonaje;
    public Sprite icono;

    [Header("Stats Base")]
    public int vidaMaxima;
    [Range(0, 100)] public int defensa;   // % de reducción de daño
    [Range(0, 100)] public int fuerza;    // % de aumento de daño

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
    public float probabilidad; // 0.0 a 1.0
}