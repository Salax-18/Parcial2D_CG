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
        Debug.Log("Ejecutando Recoger() para: " + nombreItem);

        // Solo enviamos el nombre porque tu inventario ya tiene el "ParsearItem"
        Inventario.Instance.AgregarItem(nombreItem);

        // Esta l�nea DEBE ejecutarse para que el objeto desaparezca
        Destroy(gameObject);
    }
}