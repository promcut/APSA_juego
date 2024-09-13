using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    private SceneController21 sceneController21;
    public Animator animator;


    private void Awake() {
        sceneController21 = FindObjectOfType<SceneController21>();
        animator = GetComponent<Animator>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropeado");
        if (eventData.pointerDrag != null)
        {
            if(eventData.pointerDrag.tag == tag){
                play_animation();
                Debug.Log("Acertaste");
                sceneController21.SumarPuntos();
                eventData.pointerDrag.SetActive(false);
            }else{
                Debug.Log("Fallaste");
                sceneController21.Fallo();
            }
            /*RectTransform droppedRectTransform = eventData.pointerDrag.GetComponent<RectTransform>();
            RectTransform dropZoneRectTransform = GetComponent<RectTransform>();

            // Change the parent to the drop zone's parent to handle different anchor points
            droppedRectTransform.SetParent(dropZoneRectTransform);

            // Set the position relative to the drop zone
            droppedRectTransform.anchoredPosition = dropZoneRectTransform.anchoredPosition;

            // Inform the MainElementDrop that the object was dropped in a zone
            MainElementDrop mainElementDrop = eventData.pointerDrag.GetComponent<MainElementDrop>();
            if (mainElementDrop != null)
            {
                mainElementDrop.SetDroppedInZone(true);
            }*/
        }
    }

    private void play_animation()
    {
        animator.SetTrigger("acierto");
    }
}
