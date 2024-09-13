using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    private Vector3 escalaOriginal;
    private AudioSource audioSource;

    public void jugar()
    {
        escalaOriginal = transform.localScale;
        // Escalar el botón un poco más grande durante 0.5 segundos
        transform.DOScale(escalaOriginal * 1.2f, 0.5f)
            .SetEase(Ease.InSine).OnComplete(LoadSceneAfterAnimation);
        SceneManager.LoadScene(1);
    }
    private void LoadSceneAfterAnimation()
    {
        // Cargar la escena después de que la animación termine
        SceneManager.LoadScene(1);
    }

    public void exit()
    {
        Debug.Log("Salir..");
        Application.Quit();
    }
}
