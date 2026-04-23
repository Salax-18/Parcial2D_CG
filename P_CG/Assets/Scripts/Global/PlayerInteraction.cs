using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float radioInteraccion = 1.5f;
    public LayerMask capaInteractuable;
    public KeyCode teclaInteraccion = KeyCode.E;

    void Update()
    {
        if (Input.GetKeyDown(teclaInteraccion))
        {
            Debug.Log("1. Tecla presionada"); // Si no sale esto, el Input sigue fallando
            EjecutarInteraccion();
        }
    }

    private void EjecutarInteraccion()
    {
        Collider2D objetoEncontrado = Physics2D.OverlapCircle(transform.position, radioInteraccion, capaInteractuable);

        if (objetoEncontrado != null)
        {
            Debug.Log("2. Objeto detectado: " + objetoEncontrado.name);

            IInteractable interactuable = objetoEncontrado.GetComponent<IInteractable>();

            if (interactuable != null)
            {
                Debug.Log("3. Interfaz IInteractable encontrada. Ejecutando Interact()...");
                interactuable.Interact();
            }
            else
            {
                Debug.LogWarning("¡Cuidado! El objeto tiene la Layer correcta pero NO tiene el script de Recolección o Enemigo.");
            }
        }
        else
        {
            Debug.Log("2. No se encontró nada en el radio de interacción.");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radioInteraccion);
    }
}