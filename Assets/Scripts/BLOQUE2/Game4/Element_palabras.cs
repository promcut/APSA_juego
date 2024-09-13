using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using System.Diagnostics.Tracing;

public class Element_palabras : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Sprite altavoz;
    public string elementName;
    public Image elementImage;
    public AudioSource audioSource;
    public AudioClip audioClip;
    private SceneController2_4 sceneController2_4;
    private int principal;
    private int initial_sound = 1;
    private Vector3 scale;
    public bool sound_playing = false;


    public void Init(int principalValue, int in_sound)
    {
        principal = principalValue;
        initial_sound = in_sound;
        Awake();
    }

    private void Awake()
    {
        sceneController2_4 = FindObjectOfType<SceneController2_4>();
        elementImage = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();

        if (principal == 1)
        {
            elementImage.sprite = altavoz;
            transform.localScale = new Vector3(1f, 1f, 1);
            if (initial_sound == 1)
            {
                //StartCoroutine(play_sound()); lo gestionaremos después del sonido del audio general de la instruccion
            }
        }
        else
        {
            transform.localScale = scale;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (principal == 1)
        {
            StartCoroutine(play_sound());
        }
        else
        {
            Debug.Log("Name: " + elementName);
            sceneController2_4.CheckObject(gameObject);
        }
    }

    public void SetElement(Sprite sprite, string name, AudioClip sound, Vector3 nscale)
    {
        elementImage.sprite = sprite;
        elementName = name;
        audioClip = sound;
        scale = nscale;
    }

    public IEnumerator play_sound()
    {
        Debug.Log("Playing sound");
        if(!sound_playing){
            sound_playing = true;
            audioSource.PlayOneShot(audioClip);
            yield return new WaitForSeconds(audioClip.length);
            sound_playing = false; 
        }
    }

    public void AnimateElement()
    {
        Vector3 originalScale = transform.localScale;  // Guardamos la escala original

        Color originalColor = elementImage.color;  // Guardamos el color original

        // Crear una secuencia de animación
        Sequence mySequence = DOTween.Sequence();

        // Añadir una animación para agrandar el elemento
        mySequence.Append(transform.DOScale(new Vector3(originalScale.x * 1.2f, originalScale.y * 1.2f, originalScale.z), 0.2f));

        // Añadir una animación para hacer brillar el elemento (cambiar el color temporalmente)
        mySequence.Join(elementImage.DOColor(Color.yellow, 0.2f));

        // Añadir una animación para volver al tamaño original y al color original
        mySequence.Append(transform.DOScale(originalScale, 0.2f));
        mySequence.Join(elementImage.DOColor(originalColor, 0.2f));

        // Iniciar la secuencia
        mySequence.Play();
    }

}
