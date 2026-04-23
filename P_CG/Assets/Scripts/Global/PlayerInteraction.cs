using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    public float radioInteraccion = 1.5f;
    public LayerMask capaInteractuable; // Solo detectará objetos en esta capa
    public KeyCode teclaInteraccion = KeyCode.E;

    void Update()
    {
        // Detecta la tecla de forma independiente al script de movimiento
        if (Input.GetKeyDown(teclaInteraccion))
        {
            EjecutarInteraccion();
        }
    }

    private void EjecutarInteraccion()
    {
        // Crea un círculo invisible alrededor del player para buscar interactuables
        Collider2D objetoEncontrado = Physics2D.OverlapCircle(transform.position, radioInteraccion, capaInteractuable);

        if (objetoEncontrado != null)
        {
            // Intentamos obtener la interfaz que ya configuramos en los otros scripts
            IInteractable interactuable = objetoEncontrado.GetComponent<IInteractable>();

            if (interactuable != null)
            {
                interactuable.Interact();
            }
        }
    }

    // Esto es para que puedas ver el alcance en la ventana de Escena (Gizmo amarillo)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioInteraccion);
    }
}