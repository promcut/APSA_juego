using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TrazoUIOrden : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public List<Transform> formasObjetivo; // Lista de Transform que representan las diferentes formas objetivo
    public List<GameObject> basesObjetivo; // Lista de GameObjects que contienen los puntos objetivo
    private TrailRenderer trailRenderer;
    private List<GameObject> puntosTocados = new List<GameObject>();
    private Vector3 posicionInicial; // Para almacenar la posición inicial
    private Forma forma; // Referencia al componente Forma
    public Color colorDelTrazo = new Color(0xB0 / 255f, 0x83 / 255f, 0xFF / 255f); // Color en formato RGB
    private int indiceSiguientePunto = 0; // Índice del siguiente punto objetivo a tocar
    private List<GameObject> puntosEnOrden = new List<GameObject>(); // Lista de puntos en orden a tocar
    public float with = 0.2f;
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

        // Inicializar puntosEnOrden con los puntos hijos de basesObjetivo en el orden correcto
        foreach (GameObject baseObjetivo in basesObjetivo)
        {
            foreach (Transform punto in baseObjetivo.transform)
            {
                puntosEnOrden.Add(punto.gameObject);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        puntosTocados.Clear();
        indiceSiguientePunto = 0;
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

                    // Comprobar si se ha tocado el punto objetivo actual en el orden correcto
                    if (indiceSiguientePunto < puntosEnOrden.Count)
                    {
                        GameObject puntoObjetivo = puntosEnOrden[indiceSiguientePunto];
                        if (!puntosTocados.Contains(puntoObjetivo) && Vector2.Distance(worldPoint, puntoObjetivo.transform.position) < 0.3f)
                        {
                            puntosTocados.Add(puntoObjetivo);
                            indiceSiguientePunto++;
                        }
                    }
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (puntosTocados.Count == puntosEnOrden.Count)
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
