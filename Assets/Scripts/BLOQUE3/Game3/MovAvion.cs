using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovAvion : MonoBehaviour, IPointerDownHandler
{
    private RectTransform rectTransform;
    [SerializeField] public Canvas canvas;
    public CanvasGroup canvasGroup;
    private float speed = 2.5f;
    public int direct; // 1 para izquierda a derecha, -1 para derecha a izquierda
    private SceneController3_3 sceneController;
    public bool isMainElement = false; // Indica si es el objeto principal
    

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = FindFirstObjectByType<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        sceneController = FindObjectOfType<SceneController3_3>(); // Encontrar el controlador de la escena

        // Escala en x en función de la dirección
        Debug.Log(direct);
        if(!isMainElement){ //version2, para la 1 no hace falta ya que las flechas las pongo en la dirección correcta desde el editor
            Vector3 newScale = transform.localScale;
            newScale.x *= direct;
            transform.localScale = newScale;
        }
        
        if(isMainElement){
            GetComponent<Image>().raycastTarget = false;
        }
    }

    void Update()
    {
        // Si no es el objeto principal, mover el objeto horizontalmente dependiendo del valor de direct
        if (!isMainElement)
        {
            Vector3 horizontalMovement = Vector3.right * direct * speed;
            rectTransform.position += horizontalMovement * Time.deltaTime;

            // Destruir el objeto si sale de la pantalla
            if ((direct == 1 && transform.position.x > 7f) || (direct == -1 && transform.position.x < -7f))
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        sceneController.CheckObject(gameObject);
    }

    public void SetDir(int direction)
    {
        direct = direction;
        Vector3 newScale = transform.localScale;
        newScale.x *= direct;
        transform.localScale = newScale;
    }

    public void Desaparecer()
    {
        // Escalar el objeto a cero en 0.5 segundos
        rectTransform.DOScale(Vector3.zero, 0.5f)
            .OnComplete(() => Destroy(gameObject)); // Destruir el objeto después de la animación
    }
}
