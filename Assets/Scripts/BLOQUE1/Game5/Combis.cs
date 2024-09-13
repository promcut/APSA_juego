using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;


public class Combis : MonoBehaviour, IPointerDownHandler
{
    //public GameObject miPrefab;
    [SerializeField] public Image imagenHija1;
    [SerializeField] public Image imagenHija2;
    [SerializeField] public Image imagenHija3;
    [SerializeField] private SceneController5 controller5;
    private Vector3 initialScale;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (transform.childCount >= 3)
        {
            imagenHija1 = transform.GetChild(0).GetComponent<Image>();
            imagenHija2 = transform.GetChild(1).GetComponent<Image>();
            imagenHija3 = transform.GetChild(2).GetComponent<Image>();
        }
        else
        {
            imagenHija1 = transform.GetChild(0).GetComponent<Image>();
            imagenHija2 = transform.GetChild(1).GetComponent<Image>();
        }
    }
    void Start()
    {
        // Instanciar el prefab y obtener referencias a las imágenes hijas
        //GameObject instanciaPrefab = Instantiate(miPrefab, transform.position, Quaternion.identity);
        controller5 = GameObject.FindObjectOfType<SceneController5>(); // Buscar y asignar dinámicamente el SceneController en la escena
        imagenHija1.gameObject.SetActive(true); // Ocultar la primera imagen
        imagenHija2.gameObject.SetActive(true); // Mostrar la segunda imagen
    }

    // Función para mostrar una imagen con un efecto de aparición usando DOTween
    void MostrarImagenConEfecto(Image imagen, float retraso)
    {
        //GridLayoutGroup gridLayout = transform.parent.GetComponent<GridLayoutGroup>();

        // Incrementar dinámicamente el tamaño de las celdas para hacer las imágenes más grandes
        //gridLayout.spacing = new Vector2(100, 100); //NO HACE NADAAAAAAAA
        //imagen.transform.localScale = imagen.transform.localScale * 2f;
        imagen.DOFade(1f, 0.5f).SetDelay(retraso);
    }

    public void OcultarImagenes()
    {
        Color colorActual = imagenHija1.color;
        Color colorActual2 = imagenHija2.color;
        colorActual.a = 0;
        colorActual2.a = 0;
        imagenHija1.color = colorActual;
        imagenHija2.color = colorActual2;
        if (transform.childCount >= 3)
        {
            Color colorActual3 = imagenHija3.color;
            colorActual3.a = 0;
            imagenHija3.color = colorActual3;
        }
    }

    public void OnPointerDown(PointerEventData eventData) //Para elementos de la interfaz detecta si han sido presionados
    {
        Debug.Log("I've been touched");
        if (controller5)
        {
            if (GetComponent<BoxCollider2D>().enabled != false)
            {
                controller5.CheckFruit(this);
            }
        }
    }

    public void Desparecer()
    {
        initialScale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);
    }

    public void Reaparecer()
    {
        Debug.Log("Reapareciendo");
        transform.DOScale(initialScale, 0.5f);
    }

    public IEnumerator ChangeSprite(Sprite new_sprite)
    {
        Color colorActual = GetComponent<Image>().color;
        colorActual.a = 0;
        GetComponent<Image>().color = colorActual; //para quitarle el color al fondo
        GridLayoutGroup gridLayout = GetComponent<GridLayoutGroup>();

        // Incrementar dinámicamente el tamaño de las celdas para hacer las imágenes más grandes
        gridLayout.cellSize = new Vector2(280, 280); //NO HACE NADAAAAAAAA

        MostrarImagenConEfecto(imagenHija1, 1f);
        yield return new WaitForSeconds(1);
        audioSource.PlayOneShot(sound);
        MostrarImagenConEfecto(imagenHija2, 3f);
        yield return new WaitForSeconds(3f);
        audioSource.PlayOneShot(sound);

        if (transform.childCount >= 3)
        {
            MostrarImagenConEfecto(imagenHija3, 3f);
            yield return new WaitForSeconds(3f);
            audioSource.PlayOneShot(sound);
            yield return new WaitForSeconds(5f);
        }
        else
        {
            yield return new WaitForSeconds(5f);
        }
        // Esperar un breve momento para asegurarse de que las imágenes se muestren completamente

        // Cambiar el sprite después de que las imágenes hayan aparecido
        OcultarImagenes();
        colorActual.a = 255;
        GetComponent<Image>().color = colorActual; //le devolvemos el color al fondo para que se pueda ver el interrogante
        this.GetComponent<Image>().sprite = new_sprite;
        //transform.localScale = new Vector3(1, 1, 1);

        // Mover el elemento después de cambiar el sprite
        MoverElemento(new Vector3(256f, -178f, 0f), 1f);
        Debug.Log("Terminé");
    }


    public void MoverElemento(Vector3 destino, float duracion)
    {
        Debug.Log("Moviendo el elemento padre");

        // Accede al componente Transform del padre
        RectTransform padreRect = transform.parent.GetComponent<RectTransform>(); //movemos la caja que lo contiene

        padreRect.DOAnchorPos3D(destino, duracion);

    }

    public void VolverAlCentro(float duracion)
    {
        RectTransform padreRect = transform.parent.GetComponent<RectTransform>(); //movemos la caja que lo contiene

        padreRect.DOAnchorPos3D(new Vector3(256f, -663f, 0), duracion);

    }

    public void DesactivarCollider()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
