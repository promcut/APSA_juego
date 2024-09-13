using UnityEngine;
using UnityEngine.EventSystems; // Necesario para el uso de IDragHandler
using UnityEngine.UI;

public class Bolita : MonoBehaviour
{
    public RectTransform uiElement; // El elemento de UI que quieres mover
    public float sensitivity = 100.0f; // Sensibilidad del movimiento
    private SceneController3_7 sceneController3_7;
    private Rigidbody2D rb;


    void Start()
    {
        sceneController3_7 = FindObjectOfType<SceneController3_7>();
        uiElement = GetComponent<RectTransform>();
        rb = uiElement.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("El elemento de UI necesita un Rigidbody2D.");
        }
    }

    /*public void OnDrag(PointerEventData eventData)
    {
        // Mueve el RectTransform basado en el movimiento del ratón o toque
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)uiElement.parent, 
            eventData.position, 
            eventData.pressEventCamera, 
            out localPoint);

        uiElement.anchoredPosition = localPoint;
    }*/

    void Update()
    {
        if (rb != null)
        {
            // Obtener los datos del acelerómetro
            Vector3 acceleration = Input.acceleration;

            // Aplicar fuerza al Rigidbody2D basado en los datos del acelerómetro
            Vector2 force = new Vector2(acceleration.x, acceleration.y) * sensitivity;
            rb.AddForce(force);
            //Debug.Log("x: " + acceleration.x + " y: " + acceleration.y);
            //AdjustGravity(acceleration);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            //sceneController para pasar de nivel
            Debug.Log("win");
            Debug.Log("holi");
            sceneController3_7.SumarPuntos();
        }
    }
}
