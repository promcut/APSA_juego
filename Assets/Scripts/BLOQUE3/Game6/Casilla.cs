using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class Casilla : MonoBehaviour, IPointerDownHandler
{
    public int position;
    public Sprite sprite_correct;
    public Sprite sprite_wrong;
    private SceneController3_6 sceneController3_6;
    private Image imageComponent;
    private bool canTouch = true;

    void Start()
    {
        sceneController3_6 =  FindObjectOfType<SceneController3_6>();
        imageComponent = GetComponent<Image>();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("pulsado");
        sceneController3_6.CheckObjet(gameObject);
    }
    public IEnumerator ChangeSpriteCorrect()
    {

        GameObject childImageObject = new GameObject("ChildImageCorrect");
        childImageObject.transform.SetParent(transform, false);
        
        //tamaño del hijo para que coincida con el padre
        RectTransform childRectTransform = childImageObject.AddComponent<RectTransform>();
        childRectTransform.anchorMin = new Vector2(0f, 0f);
        childRectTransform.anchorMax = new Vector2(1f, 1f);
        childRectTransform.offsetMin = Vector2.zero;
        childRectTransform.offsetMax = Vector2.zero;
        
        Image childImage = childImageObject.AddComponent<Image>();
        childImage.raycastTarget = false;
        childImage.sprite = sprite_correct;
        //childImage.color = new Color(childImage.color.r, childImage.color.g, childImage.color.b, 0);
        imageComponent.DOFade(1, 0.5f).WaitForCompletion();
        yield return new WaitForSeconds(1);
        yield return childImage.DOFade(0, 0.5f).WaitForCompletion();

        Destroy(childImageObject); // Remove the child object
        
        // Scale down the original image
        yield return imageComponent.DOFade(0, 0.5f).WaitForCompletion();
        imageComponent.transform.localScale = new Vector3(0,0,0);
    }

    public IEnumerator ChangeSpriteWrong()
    {
        GameObject childImageObject = new GameObject("ChildImageCorrect");
        childImageObject.transform.SetParent(transform, false);
        
        //tamaño del hijo para que coincida con el padre
        RectTransform childRectTransform = childImageObject.AddComponent<RectTransform>();
        childRectTransform.anchorMin = new Vector2(0f, 0f);
        childRectTransform.anchorMax = new Vector2(1f, 1f);
        childRectTransform.offsetMin = Vector2.zero;
        childRectTransform.offsetMax = Vector2.zero;
        
        Image childImage = childImageObject.AddComponent<Image>();
        childImage.raycastTarget = false;
        childImage.sprite = sprite_wrong;
        //childImage.color = new Color(childImage.color.r, childImage.color.g, childImage.color.b, 0);
        
        // Fade in
        yield return new WaitForSeconds(1);
        yield return childImage.DOFade(0, 0.5f).WaitForCompletion();
        Destroy(childImageObject); // Remove the child object
    }

    public void RestartRaycast()
    {
        imageComponent.raycastTarget = true;
    }

    public void BlockRaycast()
    {
        imageComponent.raycastTarget = false;
    }

    public void SetCanTouch(bool state)
    {
        canTouch = state;
    }

    public bool GetCanTouch()
    {
        return canTouch;
    }
}
