using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Flecha_DropZone : MonoBehaviour, IDropHandler
{
    private SceneController3_1 sceneController3_1;

    void Awake()
    {
        sceneController3_1 = FindObjectOfType<SceneController3_1>();
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropeado");

        if (eventData.pointerDrag != null)
        {
            sceneController3_1.CheckObject(eventData.pointerDrag, gameObject);
        }
    }
}
