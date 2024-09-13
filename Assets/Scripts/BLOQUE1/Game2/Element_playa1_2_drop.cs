using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class Element_playa1_2_drop : MonoBehaviour, IDropHandler
{

    [SerializeField] private SceneController1_2 sceneController1_2;
    [SerializeField] private SceneController1_2_scene2 sceneController1_2_scene2;
    public Sprite blackImage;
    private Image imageComponent; // Componente Image para cambiar los sprites
    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    private Sprite originalSprite;

    void Awake()
    {
        imageComponent = GetComponent<Image>(); // obtenemos el componente Image
        originalSprite = imageComponent.sprite;
        
        if(blackImage != null) //escena 1, en la escena 2 se le asigna desde SetElement
        {
            imageComponent.sprite = blackImage;
        }
        
        sceneController1_2 = FindObjectOfType<SceneController1_2>(); // Buscar y asignar dinámicamente el SceneController en la escena
        sceneController1_2_scene2 = FindObjectOfType<SceneController1_2_scene2>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetElement(Sprite or_sprite, Vector3 nscale, String tag)
    {
        imageComponent.sprite = or_sprite;
        transform.localScale = nscale;
        this.tag = tag;
    }

    public void SetActive(bool v)
    {
        Debug.Log("Desactivando objeto");
        gameObject.SetActive(v);
    }


    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("dropeado");
        if (eventData != null)
        {
            Debug.Log(name + " " + eventData.pointerDrag.name);

            var draggedObject = eventData.pointerDrag.GetComponent<Element_playa1_2_drag>();
            if (draggedObject != null)
            {
                if (draggedObject.tag == tag)
                {
                    Debug.Log("holi, está bien");
                    eventData.pointerDrag.GetComponent<Element_playa1_2_drag>().SetDroppedInZone(true);
                    Destroy(eventData.pointerDrag);
                    imageComponent.sprite = originalSprite;
                    draggedObject.SetRaycastTarget(false);

                    if(sceneController1_2 != null) //primera parte del juego
                    {
                        sceneController1_2.SumarPuntos();
                    }else{ //segunda
                        sceneController1_2_scene2.SumarPuntos();
                    }
                    
                }
                else
                {
                    if(sceneController1_2 != null)
                    {
                        sceneController1_2.Fallo();
                    }
                    else{
                        sceneController1_2_scene2.Fallo();
                    }
                    
                    draggedObject.ReturnToPosition();
                }
            }
        }
    }

}
