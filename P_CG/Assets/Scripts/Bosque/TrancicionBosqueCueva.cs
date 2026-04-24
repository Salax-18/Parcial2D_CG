using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class TrancicionBosqueCueva : MonoBehaviour, IInteractable
{
    [Header("Configuración de Escena")]
    public string nombreEscenaDestino = "Cueva"; // Asegúrate de que se llame igual que tu archivo de escena

    public void Interact()
    {
        Debug.Log("Cambiando a la escena: " + nombreEscenaDestino);
        CambiarEscena();
    }

    private void CambiarEscena() =>
        // Esto carga la nueva escena
        SceneManager.LoadScene(nombreEscenaDestino);
}