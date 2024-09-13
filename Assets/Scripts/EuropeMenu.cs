using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Unity.VisualScripting;


public class EuropeMenu : MonoBehaviour
{
    [SerializeField] GameObject extraMenu;
    //Vector2 pos = new Vector2(346.0f, 65.2f);
    private bool down = false;

    public void LoadGameSpain()
    {
        SceneManager.LoadScene(2); //primer juego
    }
    public void LoadGameItaly()
    {
        SceneManager.LoadScene(10); //primer juego
    }
    public void LoadGameNorth()
    {
        SceneManager.LoadScene(19); //primer juego
    }
    public void LoadGame4()
    {
        SceneManager.LoadScene(2); //primer juego
    }
    public void LoadGame5()
    {
        SceneManager.LoadScene(2); //primer juego
    }
    public void GoBack()
    {
        SceneManager.LoadScene(0); //volver atrás
    }
    public void GoHome()
    {
        SceneManager.LoadScene(0);
    }
    public void ShowMenu()
    {
        //extraMenu.transform.DOMoveY(extraMenu.GetComponent<RectTransform>().anchoredPosition.y, 0.5f).SetEase(Ease.InQuad);
        if (!down)
        {
            extraMenu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-220, -495), 0.2f).SetEase(Ease.Linear);
            down = true;
        }
        else
        {
            extraMenu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-220, 263), 0.2f).SetEase(Ease.Linear);
            down = false;
        }
    }
}
