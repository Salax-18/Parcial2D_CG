using UnityEngine;
using UnityEngine.InputSystem;

public class NpcJorge : MonoBehaviour
{
    public string npcName = "Jorge";
    public string[] dialogLines = {
        "Bienvenidos viajeros, hace frío esta noche...",
        "¿Conocen la leyenda de Moloch?",
        "Dicen que un mago fue encerrado en un espejo hace siglos...",
        "Tengan cuidado en estos caminos."
    };

    private bool playerNearby = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerNearby = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerNearby = false;
    }

    void Update()
    {
        if (playerNearby && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("Presionó E!");
            DialogoManager.Instance.StartDialogue(npcName, dialogLines);
        }
    }
}