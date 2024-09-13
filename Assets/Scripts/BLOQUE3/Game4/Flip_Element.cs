using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Flip_Element : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int direct;
    private SceneController3_4 sceneController;
    private SceneController3_4_scene1 sceneController3_4_Scene1;
    public bool isMainElement = false; //si es el objeto principal
    private Image image;
    private Color originalColor;
    private Vector3 originalScale;

    void Awake()
    {
        sceneController = FindObjectOfType<SceneController3_4>(); 
        sceneController3_4_Scene1 = FindObjectOfType<SceneController3_4_scene1>();
        image = GetComponent<Image>();
        originalColor = image.color; //color original
        originalScale = transform.localScale; 
        // rotar z en función de la dirección
        SetDir(direct);
        Debug.Log("mi rotación: " + direct);

        if (isMainElement)
        {
            GetComponent<Image>().raycastTarget = false;
        }
    }

    public void Init(bool main, int rotation)
    {
        isMainElement = main;
        direct = rotation;
    }

    public void SetElement(Sprite sprite, Vector3 nscale)
    {
        image.sprite = sprite;
        transform.localScale = nscale;
    }

     public void OnPointerDown(PointerEventData eventData)
    {
        if(!isMainElement){
            Color pressedColor = originalColor;
            pressedColor.a = 0.8f;
            image.color = pressedColor;
            image.raycastTarget = false;

            //simular el efecto de presión
            transform.localScale = originalScale * 0.9f;
            if(sceneController3_4_Scene1 != null)
            {
                sceneController3_4_Scene1.CheckObject(gameObject); 
            }else
            {
                sceneController.CheckObject(gameObject); 
            }
        }

    }

    public void SetOriginalColor()
    {
        image.color = originalColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //color original
        //image.color = originalColor;
        transform.localScale = originalScale;
    }


    public void SetDir(int direction)
    {
        direct = direction;
        Quaternion newRotation = transform.rotation;
        newRotation = Quaternion.Euler(newRotation.eulerAngles.x, newRotation.eulerAngles.y, direction);
        transform.rotation = newRotation;
    }
}

