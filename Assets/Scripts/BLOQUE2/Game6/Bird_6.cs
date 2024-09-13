using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bird_6 : MonoBehaviour, IPointerDownHandler
{
    private SceneController2_6 sceneController2_6;
    private Vector3 originalScale;
    private Sprite originalSprite;
    private AudioSource audioSource;
    [SerializeField] private Sprite numberSprite;
    private Image elementImage;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioClip piopio;


    private void Awake() {
        sceneController2_6 = FindObjectOfType<SceneController2_6>();
        audioSource = GetComponent<AudioSource>();
        elementImage = GetComponent<Image>();
        originalSprite = elementImage.sprite;
        originalScale = transform.localScale;

    }

    public void SetElement(Sprite sprite)
    {
        numberSprite = sprite;
    }    

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("tocado");
        Sequence sequence = DOTween.Sequence();

        // Añadir animación de escala a la secuencia
        sequence.Append(transform.DOScale(originalScale * 0.9f, 0.1f));
        sequence.Append(transform.DOScale(new Vector3(0.8f, 1, 1), 0.1f));

        // Añadir una acción de cambio de sprite a la secuencia
        sequence.AppendCallback(() => {
            if (elementImage != null && numberSprite != null) {
                elementImage.sprite = numberSprite;
            }
        });        
        audioSource.PlayOneShot(audioClip);
        sceneController2_6.AddNumber();
        sceneController2_6.OnBirdClicked(this);
        elementImage.raycastTarget = false;
    }

    public void RestoreOriginalState()
    {
        transform.localScale = originalScale;
        if (elementImage != null) {
            elementImage.sprite = originalSprite;
        }
        elementImage.raycastTarget = true;
    }       

    public void SetRaycastTarget(bool value)
    {
        if (elementImage != null)
        {
            elementImage.raycastTarget = value;
        }
    }
}
