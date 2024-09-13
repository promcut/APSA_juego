using UnityEngine;

public class MovimientoNubes : MonoBehaviour
{
    private float scrollSpeed = 50f;  // Velocidad de desplazamiento en píxeles por segundo
    private RectTransform containerRectTransform;  // RectTransform del contenedor
    private RectTransform[] clouds;
    private float resetPositionX;
    private float startPositionX;
    private float cloudWidth;

    void Start()
    {
        // Obtener el RectTransform del contenedor
        containerRectTransform = GetComponent<RectTransform>();

        // Inicializar las nubes
        clouds = new RectTransform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            clouds[i] = transform.GetChild(i).GetComponent<RectTransform>();
            if (i == 0)
            {
                // Asumiendo que todas las nubes tienen el mismo tamaño
                cloudWidth = clouds[i].rect.width;
            }
        }

        // Calcular las posiciones X de inicio y reseteo
        resetPositionX = containerRectTransform.rect.width -20;
        startPositionX = -containerRectTransform.rect.width +20;
    }

    void Update()
    {
        // Mueve cada nube
        foreach (RectTransform cloud in clouds)
        {
            cloud.anchoredPosition += Vector2.right * scrollSpeed * Time.deltaTime;

            // Si la nube ha pasado el límite, resetea su posición
            if (cloud.anchoredPosition.x > resetPositionX)
            {
                float newY = cloud.anchoredPosition.y; // Mantener la misma posición Y
                cloud.anchoredPosition = new Vector2(startPositionX, newY);
            }
        }
    }
}
