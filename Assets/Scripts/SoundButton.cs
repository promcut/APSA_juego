using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite soundOn;
    public Sprite soundOff;
    public Button button;
    bool sound = true;
    private AudioSource audioSource;

    GameObject music;

    private void Awake()
    {
        music = GameObject.FindGameObjectWithTag("music"); //tenemos el audiosource en un gameobject con tag music
        audioSource = music.GetComponent<AudioSource>(); //cogemos el componente audiosource
    }

    void Start()
    {
        soundOn = button.image.sprite;
    }

    public void stopSound()
    {
        if (sound)
        {
            button.image.sprite = soundOff;
            audioSource.volume = 0;
            sound = false;
        }
        else
        {
            button.image.sprite = soundOn;
            audioSource.volume = 0.5f;
            sound = true;
        }
    }
}
