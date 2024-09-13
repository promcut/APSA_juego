using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Animal : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private int num_sounds;
    private AudioSource audioSource;
    public AudioClip sound;
    public Animator animator;
    public float time;
    private Door door;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        door = FindFirstObjectByType<Door>();
        //StartCoroutine(play_sound());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(play_sound());
    }
    
    public int get_num_sound(){
        return num_sounds;
    }

    private void play_animation()
    {
        animator.SetTrigger("play_sound");
    }

    public IEnumerator play_sound()
    {
        GetComponent<Image>().raycastTarget = false; //desactiva la detección de clics del box bloqueador
        door.SetRaycast(false);
        for(int i = 0; i<num_sounds; i++){
            audioSource.PlayOneShot(sound);
            animator.SetTrigger("play_sound");
            yield return new WaitForSeconds(time);
        }
        GetComponent<Image>().raycastTarget = true; //activa la detección de clics
        door.SetRaycast(true);
        
    }

    public void SetRaycast(bool state)
    {
        GetComponent<Image>().raycastTarget = state;
    }
}