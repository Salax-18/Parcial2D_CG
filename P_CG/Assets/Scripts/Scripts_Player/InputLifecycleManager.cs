using UnityEngine;

public class InputLifecycleManager : MonoBehaviour
{
    private PlayerControls controls;

    private void Awake()
    {
        // Inicializamos los controles una sola vez
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        // Activamos el mapa de movimiento
        if (controls != null)
        {
            controls.Movement.Enable();
            Debug.Log("Controles activados correctamente.");
        }
    }

    private void OnDisable()
    {
        // Desactivamos para evitar el error de Finalize() y Memory Leak
        if (controls != null)
        {
            controls.Movement.Disable();
            Debug.Log("Controles desactivados para evitar fugas de memoria.");
        }
    }

    private void OnDestroy()
    {
        // Limpieza final al destruir el objeto o cambiar de escena
        if (controls != null)
        {
            controls.Dispose();
            controls = null;
        }
    }
}