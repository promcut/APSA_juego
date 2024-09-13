using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TrazoUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public List<Transform> formasObjetivo; // Lista de Transform que representan las diferentes formas objetivo
    public List<GameObject> basesObjetivo; // Lista de GameObjects que contienen los puntos objetivo
    private TrailRenderer trailRenderer;
    private HashSet<GameObject> puntosTocados = new HashSet<GameObject>();
    private Vector3 posicionInicial; // Para almacenar la posición inicial
    private Forma forma; // Referencia al componente Forma
    public Color colorDelTrazo = new Color(0xB0 / 255f, 0x83 / 255f, 0xFF / 255f); // Color en formato RGB
    public float with = 0.4f;

    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.time = Mathf.Infinity; // Hacer que el trazo permanezca indefinidamente
        trailRenderer.startWidth = with;
        trailRenderer.endWidth = with;
        trailRenderer.material.color = colorDelTrazo;
        
        // Guardar la posición inicial
        posicionInicial = transform.position;
        forma = GetComponentInParent<Forma>(); // Coger el componente forma de su padre
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        puntosTocados.Clear();
        trailRenderer.Clear();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        foreach (Transform formaObjetivo in formasObjetivo)
        {
            RectTransform formaObjetivoRectTransform = formaObjetivo as RectTransform;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(formaObjetivoRectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
            {
                if (formaObjetivoRectTransform.rect.Contains(localPoint))
                {
                    Vector3 worldPoint = formaObjetivoRectTransform.TransformPoint(localPoint);
                    worldPoint.z = 0;
                    transform.position = worldPoint;

                    // Comprobar si se ha tocado algún punto objetivo
                    foreach (GameObject baseObjetivo in basesObjetivo)
                    {
                        foreach (Transform punto in baseObjetivo.transform)
                        {
                            if (!puntosTocados.Contains(punto.gameObject) && Vector2.Distance(worldPoint, punto.position) < 0.3f)
                            {
                                puntosTocados.Add(punto.gameObject);
                                //sDebug.Log("punto tocado");
                            }
                        }
                    }
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        int totalPuntos = 0;
        foreach (GameObject baseObjetivo in basesObjetivo)
        {
            totalPuntos += baseObjetivo.transform.childCount;
        }

        if (puntosTocados.Count == totalPuntos)
        {
            Debug.Log("¡Forma correcta!");
            // Llamamos a la comprobación de la forma
            forma.CheckForma();
        }
        else
        {
            Debug.Log("Forma incorrecta");
            // Volver a la posición inicial
            transform.position = posicionInicial;
            trailRenderer.Clear(); // Limpiar el trazo
        }
    }
}
