using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SimonSaysGame : MonoBehaviour
{
    public Sprite[] originalSprites; // Array de sprites de las imágenes originales
    public Sprite[] litSprites; // Array de sprites de las imágenes encendidas
    public Image[] imageSlots; // Array de Image para mostrar las imágenes al jugador
    public float sequenceDelay = 1f; // Retraso entre la iluminación de cada imagen
    public float inputDelay = 1f; // Tiempo que el jugador tiene para pulsar las imágenes después de que se iluminen

    private List<int> sequence = new List<int>(); // Secuencia de índices de las imágenes iluminadas
    private int currentIndex = 0; // Índice actual en la secuencia que el jugador debe pulsar
    private bool waitingForInput = false; // Indica si el juego está esperando la entrada del jugador
    [SerializeField] private GameObject[] nubes;
    private int currentPoints = 0;
    AudioSource audioSource;
    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip wrong;
    [SerializeField] private AudioClip observa;
    [SerializeField] private AudioClip instruction;
    [SerializeField] private GameObject avioneta;
    private Sprite originalSprite; // Sprite original
    private Image imageComponent; // Componente Image para cambiar los sprites
    [SerializeField] private Sprite pressImg; // Sprite que se mostrará al presionar

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Invoke("PlayObserva", 0.2f);
        imageComponent = GetComponent<Image>(); // obtenemos el componente Image
        originalSprite = imageComponent.sprite; // guardamos el sprite original
        Invoke("StartFirstRound", 1f);
        Invoke("PlayInstruct", 12.5f);
    }

    void StartFirstRound()
    {
        sequence.Clear();
        waitingForInput = false;
        //Debug.Log("Vamos a crear una nueva secuencia");
        // Genera una nueva secuencia aleatoria de índices de imágenes
        for (int i = 0; i < imageSlots.Length; i++)
        {
            sequence.Add(i);
        }

        // Muestra la secuencia al jugador
        //Debug.Log("Vamos a mostrar a la secuenciaa");
        //Debug.Log(sequence);
        StartCoroutine(ShowSequence());
    }

    void StartNewRound()
    {
        sequence.Clear();
        currentIndex = 0;
        waitingForInput = false;
        //Debug.Log("Vamos a crear una nueva secuencia");
        // Genera una nueva secuencia aleatoria de índices de imágenes
        for (int i = 0; i < imageSlots.Length; i++)
        {
            int randomIndex = Random.Range(0, originalSprites.Length);
            sequence.Add(randomIndex);
        }

        // Muestra la secuencia al jugador
        //Debug.Log("Vamos a mostrar a la secuenciaa");
        //Debug.Log(sequence);
        StartCoroutine(ShowSequence());
    }

    IEnumerator ShowSequence()
    {
        Bloqueo_images();
        yield return new WaitForSeconds(1f);
        imageComponent.sprite = pressImg;
        ShowSpriteTurnoPlayer(); //ponemos las imagenes en color original
        currentIndex = 0; //reinciciamos el índice para que vuelva a introducir la secuencia desde 0
        //Debug.Log("Mostrando secuencia");
        yield return new WaitForSeconds(1.5f); //esperamos un poco para que no empiece todo instantaneamente
        foreach (int index in sequence)
        {
            //Debug.Log("Mostrando img");
            // Muestra la imagen encendida durante un segundo
            imageSlots[index].sprite = originalSprites[index];
            imageSlots[index].GetComponent<ImageHandler>().PlaySound();
            yield return new WaitForSeconds(1.5f); //sequenceDelay
            // Muestra la imagen original después de un segundo
            imageSlots[index].sprite = litSprites[index];
            yield return new WaitForSeconds(1f);//sequenceDelay
        }
        Desbloqueo_images();
        waitingForInput = true;
        //Debug.Log("Nos fuimos");
        imageComponent.sprite = pressImg;
        ShowSpriteTurnoPlayer(); //ponemos las imágenes en color del turno del jugador

    }

    public void OnImageClicked(int index)
    {
        if (!waitingForInput)
            return;

        Debug.Log("Has pulsado " + index + " y el correcto es " + sequence[currentIndex]);

        if (index == sequence[currentIndex])
        {
            currentIndex++;
            if (currentIndex >= sequence.Count)
            {
                // El jugador ha completado la secuencia correctamente
                Debug.Log("¡Secuencia completada correctamente!");
                SumarPuntos();
            }
        }
        else
        {
            // El jugador ha cometido un error
            audioSource.PlayOneShot(wrong);
            Debug.Log("¡Has cometido un error! Inténtalo de nuevo.");
            StartCoroutine(ShowSequence());
        }

    }

    private void SumarPuntos()
    {

        if (currentPoints >= nubes.Length - 1)
        {
            nubes[currentPoints].GetComponent<nube>().brillar();//Última nube, es una chapuza hay que cambiarlo
            //Debug.Log("TERMINASTE");
            avioneta.GetComponent<avionetaMovement>().Ganar();
            audioSource.PlayOneShot(win);
            Invoke("VolverEspaña", 3f);
        }
        else
        {
            nubes[currentPoints].GetComponent<nube>().brillar(); //Llamamos al método brillar que hace la animación de la nube al pasar de nivel
            currentPoints++;
            StartNewRound();
        }
    }

    private void VolverEspaña()
    {
        // Cargar la nueva escena
        SceneManager.LoadScene(2);
    }
    
    public void ShowSpriteTurnoPlayer()
    {
        foreach (var image in imageSlots)
        {
            var imageHandler = image.GetComponent<ImageHandler>();
            if (imageHandler != null)
            {
                imageHandler.SpriteTurnoPlayer();
            }
        }
    }

    public void ShowSpriteTurnoShowSeq()
    {
        foreach (var image in imageSlots)
        {
            var imageHandler = image.GetComponent<ImageHandler>();
            if (imageHandler != null)
            {
                imageHandler.SpriteOriginal();
            }
        }
    }

    public void Bloqueo_images(){
        foreach (var image in imageSlots){
            image.GetComponent<ImageHandler>().SetRayCast(false);
        }
    }

    public void Desbloqueo_images(){
        foreach (var image in imageSlots){
            image.GetComponent<ImageHandler>().SetRayCast(true);
        }
    }

    public void PlayInstruct()
    {
        audioSource.PlayOneShot(instruction);
    }

    public void PlayObserva()
    {
        audioSource.PlayOneShot(observa);
    }

}
