using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Element_palabras_6 : MonoBehaviour
{
    public string elementName;
    public Image elementImage;
    public AudioSource audioSource;
    public AudioClip audioClip;
    private SceneController2_6 sceneController2_6;
    private int num_silb;
    private bool sound_playing = false;
    private Vector3 scale;


    private void Awake()
    {
        sceneController2_6 = FindObjectOfType<SceneController2_6>();
        elementImage = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
        transform.localScale = scale;
    }

    public void SetElement(Sprite sprite, string name, AudioClip sound, int n_silb, Vector3 lscale)
    {
        elementImage.sprite = sprite;
        elementName = name;
        audioClip = sound;
        num_silb = n_silb;
        scale = lscale;
        transform.localScale = lscale;
    }

    public IEnumerator play_sound() //rutina?l
    {
        if(!sound_playing){
            sound_playing = true;
            audioSource.PlayOneShot(audioClip);
            yield return new WaitForSeconds(audioClip.length);
            sound_playing = false; 
        }
    }

    public int get_num_silb(){
        return num_silb;
    }

}
