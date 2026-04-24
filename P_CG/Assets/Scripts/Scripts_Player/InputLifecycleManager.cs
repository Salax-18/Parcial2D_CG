using UnityEngine;

public class InputLifecycleManager : MonoBehaviour
{
    public static InputLifecycleManager Instance;
    private PlayerControls controls;

    private void Awake()
    {
        // Singleton sencillo para que los jugadores lo encuentren fácil
        if (Instance == null)
        {
            Instance = this;
            controls = new PlayerControls();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Los jugadores llamarán a esto para saber hacia dónde moverse
    public Vector2 GetMovementValue()
    {
        return controls != null ? controls.Movement.Move.ReadValue<Vector2>() : Vector2.zero;
    }

    private void OnEnable()
    {
        if (controls != null) controls.Movement.Enable();
    }

    private void OnDisable()
    {
        // Aquí es donde "cerramos la manguera" para evitar el error de Finalize()
        if (controls != null) controls.Movement.Disable();
    }

    private void OnDestroy()
    {
        if (controls != null)
        {
            controls.Dispose();
            controls = null;
        }
    }
}