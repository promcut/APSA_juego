using UnityEngine;
using UnityEngine.EventSystems;

public class MainElementDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    private Vector3 startPosition;
    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    private bool droppedInZone = false;
    private AudioSource audioSource;
    public AudioClip sound;
    public Animator animator;


    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvas = FindFirstObjectByType<Canvas>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        play_animation();
        audioSource.PlayOneShot(sound);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        audioSource.PlayOneShot(sound);
        play_animation();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.anchoredPosition;
        droppedInZone = false;
        Debug.Log("Empieza el arrastre");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Arrastrando");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; //dividimos para que se adapte al tamaño del canvas
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Arrastre terminado");
        if (!droppedInZone)
        {
            rectTransform.anchoredPosition = startPosition; // Devolver el objeto a su lugar original
        }
    }

    public void SetDroppedInZone(bool value)
    {
        droppedInZone = value;
    }

    private void play_animation()
    {
        animator.SetTrigger("play_sound");
    }
}
