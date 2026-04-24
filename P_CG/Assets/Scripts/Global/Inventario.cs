using UnityEngine;
using System.Collections.Generic;

public class Inventario : MonoBehaviour
{
    public static Inventario Instance;

    [Header("Configuración UI")]
    public GameObject panelInventario; // Arrastra tu panel de UI aquí
    private bool estaAbierto = false;

    private Dictionary<string, int> items = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Update()
    {
        // Detecta la tecla Q para abrir/cerrar
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (panelInventario != null)
            {
                estaAbierto = !estaAbierto;
                panelInventario.SetActive(estaAbierto);
            }
        }
    }

    public void AgregarItem(string nombreItem)
    {
        var (nombre, cantidad) = ParsearItem(nombreItem);

        if (items.ContainsKey(nombre))
            items[nombre] += cantidad;
        else
            items[nombre] = cantidad;

        Debug.Log($"Inventario: +{cantidad} {nombre} (total: {items[nombre]})");
    }

    public int ObtenerCantidad(string nombre) => items.ContainsKey(nombre) ? items[nombre] : 0;
    public Dictionary<string, int> ObtenerTodo() => items;

    private (string nombre, int cantidad) ParsearItem(string input)
    {
        input = input.Trim();
        string[] partes = input.Split(' ');

        if (partes.Length >= 2 && int.TryParse(partes[0], out int cantidad))
        {
            string nombre = string.Join(" ", partes, 1, partes.Length - 1);
            return (nombre, cantidad);
        }
        return (input, 1);
    }
}