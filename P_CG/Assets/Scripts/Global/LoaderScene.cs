using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderScene : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {


        SceneManager.LoadScene(sceneName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Players"))
        {
            SceneManager.LoadScene("Bosque");
        }
    }

}