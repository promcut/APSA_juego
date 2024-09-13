using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Reflection;
using UnityEngine.EventSystems;
using System;

public class ScrollView : MonoBehaviour, IEndDragHandler
{
    [SerializeField] int pages;
    int currentPage;
    Vector3 targetPos;
    [SerializeField] Vector3 pageStep;
    [SerializeField] RectTransform levelPagesRect;

    [SerializeField] float tweenTime;
    [SerializeField] Ease tweenType;
    float dragThreshould;

    private void Awake() {
        currentPage = 1;
        targetPos = levelPagesRect.localPosition;
        dragThreshould = Screen.width / 15;
    }

    public void Next()
    {
        if (currentPage < pages)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
        }
    }

    public void Previous()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep; // Cambiado para retroceder en lugar de avanzar
            MovePage();
        }
    }

    void MovePage()
    {
        levelPagesRect.DOLocalMove(targetPos, tweenTime).SetEase(tweenType);
    }

    public void OnEndDrag(PointerEventData eventData){
        if(Math.Abs(eventData.position.x - eventData.pressPosition.x) > dragThreshould){
            if(eventData.position.x  > eventData.pressPosition.x) Previous();
            else Next();
        }
        else{
            MovePage();
        }
    }
}
