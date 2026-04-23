using UnityEngine;
using System.Collections.Generic;

public class ItemRecolectable : MonoBehaviour, IInteractable
{
    public string nombreItem;
    public int cantidad = 1;

    public void Interact()
    {
        Recoger();
    }

    private void Recoger()
    {
        Debug.Log($"Has recolectado: {nombreItem} x{cantidad}");

        // Aquí conectamos con el sistema de inventario (ver punto 3)
        Inventario.Instance.AgregarItem(nombreItem);

        // El objeto desaparece del mundo
        Destroy(gameObject);
    }
}