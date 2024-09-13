using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Nido : MonoBehaviour, IDropHandler, IPointerDownHandler
{
    private SceneController2_5 sceneController2_5;
    private Image image;

    void Awake()
    {
        sceneController2_5 = FindObjectOfType<SceneController2_5>();
        image = GetComponent<Image>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropeado");

        if (eventData.pointerDrag != null)
        {
            RectTransform droppedRectTransform = eventData.pointerDrag.GetComponent<RectTransform>();
            RectTransform dropZoneRectTransform = GetComponent<RectTransform>();

            // Change the parent to the drop zone's parent to handle different anchor points
            droppedRectTransform.SetParent(dropZoneRectTransform);
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            eventData.pointerDrag.GetComponent<Element_palabras_5>().SetDroppedInZone(true);
            StartCoroutine(sceneController2_5.CheckObject(eventData.pointerDrag));
            sceneController2_5.OnNidoDropped(this);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("PULSADO");
    }

    public void SetRaycastTarget(bool value)
    {
        if (image != null)
        {
            image.raycastTarget = value;
        }
    }
}
