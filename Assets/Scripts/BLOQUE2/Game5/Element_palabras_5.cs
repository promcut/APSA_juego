using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public class Element_palabras_5 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string elementName;
    public Image elementImage;
    public AudioSource audioSource;
    public AudioClip audioClip;
    private SceneController2_4 sceneController2_4;
    private Vector3 scale;

    private Vector3 startPosition;
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    [SerializeField] private Canvas canvas;
    private bool droppedInZone = false;
    private CanvasGroup canvasGroup;


    private void Awake()
    {
        sceneController2_4 = FindObjectOfType<SceneController2_4>();
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
        canvas = FindFirstObjectByType<Canvas>();
        elementImage = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
        canvasGroup = GetComponent<CanvasGroup>();
    }


    public void SetElement(Sprite sprite, string name, AudioClip sound, Vector3 nscale)
    {
        elementImage.sprite = sprite;
        elementName = name;
        audioClip = sound;
        scale = nscale;
        transform.localScale = scale;
    }

    public void play_sound()
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.anchoredPosition;
        droppedInZone = false;
        Debug.Log("Empieza el arrastre");
        canvasGroup.alpha = 0.6f;   
        canvasGroup.blocksRaycasts = false;
    }
        
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Arrastrando");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; //dividimos para que se adapte al tamaño del canvas
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Arrastre terminado");
        canvasGroup.alpha = 1f;   
        canvasGroup.blocksRaycasts = true;
        Debug.Log(droppedInZone);
        if (!droppedInZone)
        {
            rectTransform.anchoredPosition = startPosition; // Devolver el objeto a su lugar original
        }
    }

    public void SetDroppedInZone(bool value)
    {
        droppedInZone = value;
    }

    public void ReturnToBox(){
        rectTransform.SetParent(parentRectTransform);
        rectTransform.anchoredPosition = startPosition;
    }

    public void AllowRaycast(){
        elementImage.raycastTarget = true;
    }

    public void NoRaycast(){
        elementImage.raycastTarget = false;
    }

}
