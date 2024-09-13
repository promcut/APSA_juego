using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;



public class Element_playa1_2_drag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private Image imageComponent; // Componente Image para cambiar los sprites
    private Vector3 initialPosition;
    private RectTransform rectTransform;
    private Vector3 originalScale;
    [SerializeField] private Canvas canvas;    
    private bool droppedInZone = false;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    
    void Awake()
    {
        canvas = FindFirstObjectByType<Canvas>();
        imageComponent = GetComponent<Image>(); // obtenemos el componente Image
        originalScale = transform.localScale;
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition;
        canvasGroup = GetComponent<CanvasGroup>();
        originalParent = transform.parent;
    }

    public void SetElement(Sprite or_sprite, Vector3 nscale, String tag)
    {
        imageComponent.sprite = or_sprite;
        originalScale = nscale;
        transform.localScale = nscale;
        this.tag = tag;
    }

    public void SetActive(bool v)
    {
        Debug.Log("Desactivando objeto");
        gameObject.SetActive(v);
    }

    public void OnPointerDown(PointerEventData eventData) //Para elementos de la interfaz detecta si han sido presionados
    {

        transform.DOScale(originalScale * 0.9f, 0.1f).OnComplete(() => transform.DOScale(originalScale, 0.1f)); //efecto de pulsar

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        droppedInZone = false;
        initialPosition = rectTransform.anchoredPosition;
        Debug.Log("Empieza el arrastre");
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = .8f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Arrastrando");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; //dividimos para que se adapte al tamaño del canvas
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Arrastre terminado");
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        if (!droppedInZone)
        {
            rectTransform.anchoredPosition = initialPosition; // Devolver el objeto a su lugar original
        }
    }

    public IEnumerator ReturnToPosition()
    {
        yield return new WaitForSeconds(0.5f);
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = initialPosition;
    }

    public void SetRaycastTarget(bool state)
    {
        imageComponent.raycastTarget = state;
    }

    public void SetDroppedInZone(bool value)
    {
        droppedInZone = value;
    }
}
