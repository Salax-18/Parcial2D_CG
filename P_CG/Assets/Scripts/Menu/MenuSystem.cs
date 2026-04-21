using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Update is called once per frame
    public void Salir()
    {
        Debug.Log("Saliendo...");
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
