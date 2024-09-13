using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using TMPro;



public class Elem_Busc_Wally : MonoBehaviour, IPointerDownHandler //IPointerDownHandler is a pointer (such as a touch or mouse click) interacts with the UI element
{

    [SerializeField] private SceneController_Busc_Wally controller;
    public int id;
    [SerializeField] private GameObject final_Base;
    private Image image;


    void Awake()
    {
        controller = FindObjectOfType<SceneController_Busc_Wally>(); // Buscar y asignar dinámicamente el SceneController en la escena
        final_Base = GameObject.FindWithTag("Finish");
        image = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData) //Para elementos de la interfaz detecta si han sido presionados
    {
        controller.CheckObject(gameObject);
    }

    public void setId(int id2){
        id = id2;
    }

    public int getId(){
        return id;
    }

    public void go_to_finalbase(){
        RectTransform finalBaseRect = final_Base.GetComponent<RectTransform>();
         RectTransform thisRect = GetComponent<RectTransform>();
         thisRect.DOMove(finalBaseRect.position, 1.0f).SetEase(Ease.InOutQuad).OnComplete(() => {
                        // Cambia el parent del objeto a finalBaseRect después de que el movimiento haya terminado
                        thisRect.SetParent(finalBaseRect);
                    });
        Set_Raycast_Target(false);
    }

    public void Set_Raycast_Target(bool value){
        image.raycastTarget = value;
    }   

}
