using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;


public class ImageHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private int index; // Índice de la imagen en el array imageSlots
    private SimonSaysGame simonSaysGame;
    private Vector3 originalScale;
    public AudioClip audioClip;
    AudioSource audioSource;
    [SerializeField] private Sprite pressImg; // Sprite que se mostrará al presionar
    private Sprite originalSprite; // Sprite original
    private Image imageComponent; // Componente Image para cambiar los sprites


    void Start()
    {
        // Busca el script SimonSaysGame en el objeto que lo contiene
        simonSaysGame = FindObjectOfType<SimonSaysGame>();
        originalScale = transform.localScale;
        audioSource = GetComponent<AudioSource>(); //recuperamos el AudioSource de la nube
        imageComponent = GetComponent<Image>(); // obtenemos el componente Image
        originalSprite = imageComponent.sprite; // guardamos el sprite original

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        audioSource.PlayOneShot(audioClip); //sonidito de subir de nivel
        imageComponent.sprite = originalSprite; // cambiamos al sprite presionado
        transform.DOScale(originalScale * 0.9f, 0.1f).OnComplete(() => transform.DOScale(originalScale, 0.1f)); //efecto de pulsar
        simonSaysGame.OnImageClicked(index);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        imageComponent.sprite = pressImg; // volvemos al sprite original
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(audioClip); //sonidito de subir de nivel
    }

    public void SpriteTurnoPlayer(){
        imageComponent.sprite = pressImg; // cambiamos al sprite presionado
    }

    public void SpriteOriginal(){
        imageComponent.sprite = originalSprite; // cambiamos al sprite original
    }

    public void SetRayCast(bool state){
        imageComponent.raycastTarget = state;
    }
}
