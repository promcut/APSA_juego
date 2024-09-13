using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone1_3 : MonoBehaviour, IDropHandler
{
    private SceneController1_3 sceneController1_3;
    private int num_hijos;
    private int num_hijos_final;

    void Awake()
    {
        sceneController1_3 = FindObjectOfType<SceneController1_3>();
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
            eventData.pointerDrag.GetComponent<Element_playa1_2_drag>().SetDroppedInZone(true);
            StartCoroutine(sceneController1_3.CheckObject(eventData.pointerDrag));
        }
    }

    public void SetNum_hijos_final(int num) 
    {
        num_hijos_final = num;
    }
}
