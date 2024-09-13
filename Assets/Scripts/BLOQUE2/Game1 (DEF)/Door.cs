using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class Door : MonoBehaviour, IPointerDownHandler
{
    // Start is called before the first frame update
    private SceneController22 sceneController22;
    private SceneController2_6 sceneController2_6;
    [SerializeField] private Sprite wrongDoor;
    [SerializeField] private Sprite rightDoor;
    [SerializeField] private Sprite originalSprite;
    private Vector3 originalScale;
    private Image image;
    

    void Awake()
    {   
        sceneController22 = FindFirstObjectByType<SceneController22>();
        sceneController2_6 = FindFirstObjectByType<SceneController2_6>();
        originalScale = transform.localScale;
        image = GetComponent<Image>();
        originalSprite = image.sprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(originalScale * 0.9f, 0.1f).OnComplete(() => transform.DOScale(originalScale, 0.1f)); //efecto de pulsar
        bool respuesta;
        if(sceneController22 != null){
            respuesta = sceneController22.CheckSounds();
        }else { //2_6
            respuesta = sceneController2_6.CheckObject();
        }
        Sprite nuevoSprite = respuesta ? rightDoor : wrongDoor;

        // Cambiar el sprite con animación usando DOTween
        DOTween.Sequence()
            .Append(image.DOFade(0f, 0.1f))  // Desvanecer
            .OnComplete(() =>
            {
                image.sprite = nuevoSprite;  // Cambiar el sprite
                image.DOFade(1f, 0.1f);      // Aparecer
            });
    }

    public IEnumerator OriginalSprite(){
        Debug.Log("volviendo al sprite original");
        image.raycastTarget = false;
        yield return new WaitForSeconds(1.5f);
                DOTween.Sequence()
            .Append(image.DOFade(0f, 0.1f))  // Desvanecer
            .OnComplete(() =>
            {
                image.sprite = originalSprite;  // Cambiar el sprite
                image.DOFade(1f, 0.1f);      // Aparecer
            });
        //image.raycastTarget = true;
    }

    public void SetRaycast(bool state)
    {
        image.raycastTarget = state;
    }
}
