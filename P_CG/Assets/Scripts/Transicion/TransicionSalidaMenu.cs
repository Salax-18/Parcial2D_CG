using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransicionSalidaMenu : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private AnimationClip animacionFinal; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //CambieEscena
            StartCoroutine(CambiarEscena());
        }
    }


    IEnumerator CambiarEscena(){
        animator.SetTrigger("Iniciar");

       yield return new WaitForSeconds(animacionFinal.length);

        SceneManager.LoadScene(3);
    }

}
