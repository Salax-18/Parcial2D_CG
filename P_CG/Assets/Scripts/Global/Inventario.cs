using UnityEngine;
using System.Collections.Generic;

public class Inventario : MonoBehaviour
{
    public static Inventario Instance;

    [Header("Configuraci�n UI")]
    public GameObject panelInventario;
    private bool estaAbierto = false;

    private Dictionary<string, int> items = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // OJO: Esto asegura que el objeto sea ra�z para que DontDestroyOnLoad funcione
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            estaAbierto = !estaAbierto;
            panelInventario.SetActive(estaAbierto);
            if (estaAbierto) RefrescarUI();
        }
    }

    public void RefrescarUI()
    {
        if (panelInventario == null) return;
        MochilaSlot[] slots = panelInventario.GetComponentsInChildren<MochilaSlot>();
        foreach (MochilaSlot slot in slots) slot.ActualizarSlot();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            estaAbierto = !estaAbierto;
            panelInventario.SetActive(estaAbierto);
            if (estaAbierto) RefrescarUI();
        }
    }

    public void RefrescarUI()
    {
        if (panelInventario == null) return;
        MochilaSlot[] slots = panelInventario.GetComponentsInChildren<MochilaSlot>();
        foreach (MochilaSlot slot in slots) slot.ActualizarSlot();
    }

    public void AgregarItem(string nombreItem)
    {
        var (nombre, cantidad) = ParsearItem(nombreItem);
        // Guardamos siempre en min�sculas y sin espacios extra
        string clave = nombre.ToLower().Trim();

        if (items.ContainsKey(clave)) items[clave] += cantidad;
        else items[clave] = cantidad;

        Debug.Log($"Guardado con �xito: '{clave}' | Total: {items[clave]}");
        if (estaAbierto) RefrescarUI();
    }

    public int ObtenerCantidad(string nombre) => items.ContainsKey(nombre) ? items[nombre] : 0;
    public Dictionary<string, int> ObtenerTodo() => items;
    public int ObtenerCantidad(string nombre)
    {
        string clave = nombre.ToLower().Trim();
        return items.ContainsKey(clave) ? items[clave] : 0;
    }

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