using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Flecha : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    private CanvasGroup canvasGroup;
    private float fallSpeed = 0.8f; // Velocidad de caída
    private bool dragging = false;
    private float fallStartTime;
    private Vector3 dragStartPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = FindFirstObjectByType<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        fallStartTime = Time.time;
    }

    void Update()
    {
        if (!dragging)
        {
            // Mover el objeto hacia abajo a una velocidad constante
            Vector3 fallMovement = Vector3.down * fallSpeed;
            rectTransform.position += fallMovement * Time.deltaTime;

            // Destruir el objeto si sale de la pantalla (opcional)
            if (transform.position.y < -6f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragging = true;
        dragStartPosition = rectTransform.position;
        fallStartTime = Time.time;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; // Dividir para adaptarse al tamaño del canvas
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Ajustar la posición para que continúe cayendo desde la posición actual
        float fallDuration = Time.time - fallStartTime;
        Vector3 fallMovement = Vector3.down * fallSpeed * fallDuration;
        rectTransform.position = dragStartPosition + fallMovement;
        fallStartTime = Time.time;
    }

    public void Desaparecer()
    {
        // Escalar el objeto a cero en 0.5 segundos
        rectTransform.DOScale(Vector3.zero, 0.5f)
            .OnComplete(() => Destroy(gameObject)); // Destruir el objeto después de la animación
    }
}
