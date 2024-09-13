using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class ItalyMenu : MonoBehaviour
{
    [SerializeField] GameObject extraMenu;
    //Vector2 pos = new Vector2(346.0f, 65.2f);
    private bool down = false;
    public void LoadScene1(){
        SceneManager.LoadScene(12); //primer nivel
    }
    
    public void LoadScene2(){
        SceneManager.LoadScene(13); //segundo nivel
    }

    public void LoadScene3(){
        SceneManager.LoadScene(14); //primer nivel
    }

    public void LoadScene4(){
        SceneManager.LoadScene(15); //primer nivel
    }

    public void LoadScene5(){
        SceneManager.LoadScene(16); //primer nivel
    }

    public void LoadScene6(){
        SceneManager.LoadScene(17); //primer nivel
    }

    public void LoadSpecialGame(){
        SceneManager.LoadScene(9); // simon dice
    }

    public void GoBack(){
        SceneManager.LoadScene(1); //volver atrás
    }

    public void GoHome()
    {
        SceneManager.LoadScene(0);
    }
    
    public void ShowMenu()
    {
         if (!down)
        {
            extraMenu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-220,-495),0.2f).SetEase(Ease.Linear);
            down = true;
        }
        else
        {
            extraMenu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-220,263),0.2f).SetEase(Ease.Linear);
            down = false;
        }
    }
}
