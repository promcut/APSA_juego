using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using System.Diagnostics.Tracing;

public class Element : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private int id;
    [SerializeField] private Sprite altavoz;
    [SerializeField] private Sprite originalSprite;
    [SerializeField] private Sprite altavoz_select;
    [SerializeField] private AudioClip sound;
    private AudioSource audioSource;
    private Image image;
    private SceneController2_2 sceneController2_2;
    private SceneController2_3 sceneController2_3;
    private int principal;
    private int initial_sound = 1;
    private Vector3 initialScale;
    private bool sound_playing = false;
    private int firsttime = 0;


    public void Init(int principalValue, int in_sound, int first_time)
    {
        principal = principalValue;
        initialScale = GetComponent<RectTransform>().localScale;
        initial_sound = in_sound;
        firsttime = first_time;
        Start();
    }

    void Start()
    {
        sceneController2_2 = FindFirstObjectByType<SceneController2_2>();
        sceneController2_3 = FindFirstObjectByType<SceneController2_3>(); 
        audioSource = GetComponent<AudioSource>();
        image = GetComponent<Image>();

        if(principal == 1){
            image.sprite = altavoz;
            transform.localScale = new Vector3(0.8f, 0.8f, 1);
            if(initial_sound == 1){
                 StartCoroutine(play_sound());
            }
        }
    }

    private void OcultarElementos()
    {
        Color color = image.color;
        color.a = 0f; // Establecer el alfa a 0 para hacerlo transparente
        image.color = color;
    }
    
    public void OcultarElementoFade()
    {
        image.DOFade(0f, 0.5f); // Animar el alfa de 0 a 1 en 0.5 segundos
        image.raycastTarget = true; //activamos de nuevo el raycast ya que ya no se ha acertado y podemos volverlo a usar
    }

    public void OcultarElementoInicialFade()
    {
        if(principal == 0){
            image.DOFade(0f, 0.5f); // Animar el alfa de 0 a 1 en 0.5 segundos
        }
       
    }

    public int get_id(){
        return id;
    }

    public void set_id(int newId){
        id = newId;
    }

    public IEnumerator play_sound()
    {
        if(!sound_playing){

            if(firsttime == 1)
            {
                yield return new WaitForSeconds(1.5f); //esperamos a que suente la instruccion

            }
            sound_playing = true;
            audioSource.PlayOneShot(sound);
            yield return new WaitForSeconds(sound.length);
            sound_playing = false; 
        }
    }

    public void Revelar(){
        DOTween.Sequence()
            .Append(image.DOFade(0f, 0.1f))  // Desvanecer
            .OnComplete(() =>
            {
                transform.localScale = initialScale;
                image.sprite = originalSprite;  // Cambiar el sprite            
                image.DOFade(1f, 0.1f);      // Aparecer
            });
        
    }

    public void ChangeAciertoAltavoz(){
        image.sprite = altavoz_select;
    }

    public void RemoveRaycast(){
        image.raycastTarget = false;
    }

    public void SetAltavozOriginal(){
        image.sprite = altavoz;
    }

    private void Reaparecer()
    {        
        image.raycastTarget = false; //desactivamos el raycast ya que ya se ha acertado y no lo vamos a pulsar más
        image.DOFade(1f, 0.5f); // Animar el alfa de 0 a 1 en 0.5 segundos
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(principal==1){
            StartCoroutine(play_sound());
            if(sceneController2_3 != null){
                ChangeAciertoAltavoz();
                string type = sceneController2_3.getSelected_Item();
                //sceneController2_3.setSelected_altavoz(id);
                if (type == "null"){
                    sceneController2_3.setSelected_altavoz(gameObject); // Pasar este objeto
                }else{
                    if(type == "altavoz"){
                        sceneController2_3.setSelected_altavoz(gameObject); // Pasar este objeto
                    }else{
                        bool acierto = sceneController2_3.CheckObject(id);
                        if(!acierto){
                            SetAltavozOriginal();
                        }
                    }
                    
                }
            }
        }else{
            if(sceneController2_2 != null){
                sceneController2_2.CheckObject(id);
            }else if(sceneController2_3 != null){
                string type = sceneController2_3.getSelected_Item();
                Reaparecer();
                if(type == "null"){
                    sceneController2_3.setSelected_Image(gameObject);
                }else{
                    if(type == "image"){
                         sceneController2_3.setSelected_Image(gameObject);
                    }else{
                        bool acierto = sceneController2_3.CheckObject(id);
                        if(!acierto){
                            Invoke("OcultarElementoFade", 1f);
                        }
                    }
                    
                }
            }
        }
    }


}
