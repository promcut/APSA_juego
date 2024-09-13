using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bird : MonoBehaviour, IPointerDownHandler
{
    private SceneController22 sceneController22;
    private Vector3 originalScale;
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioClip piopio;
    public Animator animator;
    private RectTransform rectTransform;
    private int fly = 0;
    private Image image;


    private void Start() {
        sceneController22 = FindObjectOfType<SceneController22>();
        audioSource = GetComponent<AudioSource>();
        originalScale = transform.localScale;
        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        audioSource.volume = 0.2f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("tocado");
        transform.DOScale(originalScale * 0.9f, 0.1f).OnComplete(() => transform.DOScale(originalScale, 0.1f)); //efecto de pulsar
        audioSource.PlayOneShot(audioClip);
        image.raycastTarget = false;
        sceneController22.AddNumber();
        sceneController22.OnUserActivity(); //reseteamos el tiempo de inactividad pq se estan tocando pajaros
        fly = 1;
    }

    public void Volar(){
        //aniimación del pájarito volando
        if(fly == 1){
           Debug.Log("volando");
            animator.SetBool("fly", true);
            audioSource.PlayOneShot(piopio); 
        
        float desplazamientoX = 1000f; // ajusta este valor según sea necesario
        float desplazamientoY = 1000f; // ajusta este valor según sea necesario

        // Animar el desplazamiento usando DOAnchorPos
        rectTransform.DOAnchorPos(rectTransform.anchoredPosition + new Vector2(desplazamientoX, desplazamientoY), 3f)
            .SetEase(Ease.InOutSine); // ajusta la duración y la curva de animación según sea necesario
        }
    }

    public void RestoreValueBird(){
        fly = 0;
        image.raycastTarget = true;
    }

    public void SetRaycast(bool state)
    {
        image.raycastTarget = state;
    }
    
}
