using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;



public class Element_playa1_2_scene2: MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private SceneController1_2_scene2 sceneController1_2;
    private Image imageComponent; // Componente Image para cambiar los sprites
    private Vector3 originalScale;
    [SerializeField] private Canvas canvas;    
    private bool isBlack;
    private Sprite blackSprite;
    private Sprite originalSprite;
    private int id;

    public void Init(bool black)
    {
        isBlack = black;
    }

    void Start()
    {
        imageComponent = GetComponent<Image>(); // obtenemos el componente Image
        if(isBlack)
        {
            imageComponent.sprite = blackSprite;
        }else
        {
            imageComponent.sprite = originalSprite;
        }
        sceneController1_2 = FindObjectOfType<SceneController1_2_scene2>(); // Buscar y asignar dinámicamente el SceneController en la escena
        originalScale = transform.localScale;
    }

    
    public void SetElement(Sprite or_sprite, Sprite black, Vector3 nscale, int id)
    {
        originalSprite = or_sprite;
        blackSprite = black;
        transform.localScale = nscale;
        this.id = id;
    }

    public void SetActive(bool v)
    {
        Debug.Log("Desactivando objeto");
        gameObject.SetActive(v);
    }

    public void OnPointerDown(PointerEventData eventData) //Para elementos de la interfaz detecta si han sido presionados
    {
        if(isBlack)
        {
            transform.DOScale(originalScale * 0.9f, 0.1f).OnComplete(() => transform.DOScale(originalScale, 0.1f)); //efecto de pulsar
            SetRaycastTarget(false);
            sceneController1_2.CheckFruit(this);
        }

    }

    public void SetRaycastTarget(bool state)
    {
        imageComponent.raycastTarget = state;
    }

    public int GetId()
    {
        return id;
    }
}
