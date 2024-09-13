using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;



public class MainFruit2 : MonoBehaviour, IPointerDownHandler //IPointerDownHandler is a pointer (such as a touch or mouse click) interacts with the UI element
{

    [SerializeField] private SceneController1_1 controller;
    [SerializeField] private SceneController1_4 controller4;
    [SerializeField] private SceneController1_2 controller2combis;
    private Vector3 initialScale;
    private bool isInterrogant = false;
    private Sprite originalImage;
    private Image imageComponent; // Componente Image para cambiar los sprites
    private Vector3 originalScale;
    //private bool acierto = false;
    private Coroutine showImageCoroutine;


    void Awake()
    {
        imageComponent = GetComponent<Image>(); // obtenemos el componente Image
        originalScale = transform.localScale;
        originalImage = imageComponent.sprite;

        //Debug.Log("Nombre del objeto: " + gameObject.name);
        controller = FindObjectOfType<SceneController1_1>();
        controller4 = FindObjectOfType<SceneController1_4>();
    }

    public void SetActive(bool v)
    {
        Debug.Log("Desactivando objeto");
        gameObject.SetActive(v);
    }

    public void OnPointerDown(PointerEventData eventData) //Para elementos de la interfaz detecta si han sido presionados
    {
        transform.DOScale(originalScale * 0.9f, 0.1f).OnComplete(() => transform.DOScale(originalScale, 0.1f)); //efecto de pulsar
        if (GetComponent<BoxCollider2D>().enabled != false)
        {

            Debug.Log("I've been touched");
            if (controller)
            {
                controller.CheckFruit(this);
            }
            if (controller4)
            {
                if (isInterrogant)
                {
                    StartShowImage();
                }
                else
                {
                    var tempColor = imageComponent.color; // Asumiendo que imageComponent es el componente Image del objeto
                    tempColor.a = 0.8f; // Ajusta el alfa a 50% para hacer el objeto semi-transparente
                    imageComponent.color = tempColor;
                    controller4.CheckObject(gameObject);
                }
            }

        }
    }

    public IEnumerator Parpadeo(Sprite new_sprite)
    {

        /* transform.DOPunchScale(new Vector3(0.2f,0.2f,1), 1, 0, 1F).SetLoops(5).OnComplete(() =>{
             transform.DOScale(new Vector3(1,1,1), 1f).SetEase(Ease.OutSine);
         }); //vector al q escala, punch, vibrato, elasticidad*/
        SetRaycastTarget(false);
        initialScale = GetComponent<RectTransform>().localScale;
        //originalImage = this.GetComponent<Image>().sprite;
        DOTween.Sequence()
            .Append(transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), 1, 0, 1F).SetLoops(5)) //vector al que escala, punch, vibrato, elasticidad
            .AppendCallback(() =>
            {
                // Cambiar el sprite
                imageComponent.sprite = new_sprite;
                transform.localScale = new Vector3(0.8f, 1, 1); //la escala real de los prefabs está modificada en función de su forma, por tanto al poner el interrogante debemos escalarlo a 1x1
            })
            .Append(transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), 1, 0, 1F).SetLoops(1)); //vector al que escala, punch, vibrato, elasticidad
        isInterrogant = true;
        yield return new WaitForSeconds(6f);
        Debug.Log("Terminé");
        SetRaycastTarget(true);
    }

    public IEnumerator ShowImage()
    {
        imageComponent.raycastTarget = false;
        Sprite interrogante = GetComponent<Image>().sprite;
        GetComponent<RectTransform>().localScale = initialScale;
        imageComponent.sprite = originalImage;
        yield return new WaitForSeconds(1f);
        transform.localScale = new Vector3(0.8f, 1, 1);
        imageComponent.sprite = interrogante;
        imageComponent.raycastTarget = true;
    }

    public void OriginalSprite()
    {
        //acierto = true;
        imageComponent.sprite = originalImage;
        transform.localScale = initialScale;

        // Detener la corutina si está en ejecución
        if (showImageCoroutine != null)
        {
            StopCoroutine(showImageCoroutine);
            showImageCoroutine = null; // Limpiar la referencia
        }
    }

    public void StartShowImage()
    {
        // Iniciar la corutina y guardar la referencia
        showImageCoroutine = StartCoroutine(ShowImage());
    }

    public void Desparecer()
    {
        initialScale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);
    }

    public void Reaparecer()
    {
        Debug.Log("Reapareciendo");
        transform.DOScale(initialScale, 0.5f);
    }

    public void SetRaycastTarget(bool status)
    {
        imageComponent.raycastTarget = status;
    }

    public void ReturnColor()
    {
        var tempColor = imageComponent.color; // Asumiendo que imageComponent es el componente Image del objeto
        tempColor.a = 1f; // Ajusta el alfa a 50% para hacer el objeto semi-transparente
        imageComponent.color = tempColor;
    }

}
