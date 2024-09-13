using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController3_3 : MonoBehaviour
{
    public Sprite flecha_izquierda; // Sprite para la flecha izquierda
    public Sprite flecha_derecha; // Sprite para la flecha derecha
    public GameObject flecha_prefab; // Prefab para el avión

    private GameObject mainElement;
    [SerializeField] private GameObject[] aviones; // Array de aviones
    public Transform[] spawnBasesIzq; // Puntos de generación a la izquierda (2 bases)
    public Transform[] spawnBasesDer; // Puntos de generación a la derecha (2 bases)
    public Transform mainBase; // Punto de generación para el elemento principal
    public Transform contenedor_aviones; // Contenedor de aviones

    public Canvas canvas; // Referencia al canvas
    [SerializeField] private GameObject[] estrellitas;

    public int pointsToWin = 10; // Puntos necesarios para ganar
    private int currentPoints = 0; // Puntos actuales del jugador

    [SerializeField] private AudioClip winLevel;
    [SerializeField] private AudioClip wrong;
    [SerializeField] public AudioClip instruction;
    [SerializeField] private GameObject avioneta;
    private AudioSource audioSource;
    public int num_scene = 0;
    private int direct_actual;
    private Transform spawnPoint;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Llamar a la función para generar objetos aleatorios cada segundo
        InvokeRepeating("SpawnRandomObject", 1f, 1f);

        // Configurar dirección inicial
        direct_actual = num_scene == 0 ? 1 : -1;
        if(num_scene == 0)
        {
            Invoke("PlayInstruct", 0.2f);
        }
    }

    void SpawnRandomObject()
    {
        // Elegir una base de generación aleatoria (dos bases a la izquierda y dos a la derecha)
        if (Random.value > 0.5f)
        {
            spawnPoint = spawnBasesIzq[Random.Range(0, spawnBasesIzq.Length)];
        }
        else
        {
            spawnPoint = spawnBasesDer[Random.Range(0, spawnBasesDer.Length)];
        }

        // Seleccionar un tipo de avión aleatorio
        int objectType = Random.Range(0, aviones.Length);
        GameObject spawnedObject = Instantiate(aviones[objectType], spawnPoint.position, Quaternion.identity);

        // Asignar el canvas como padre del objeto instanciado
        if (spawnedObject != null)
        {
            spawnedObject.transform.SetParent(contenedor_aviones, false);
            spawnedObject.transform.position = spawnPoint.position;

            // Configurar la dirección del objeto instanciado
            MovAvion flecha = spawnedObject.GetComponent<MovAvion>();
            flecha.SetDir(spawnPoint == spawnBasesIzq[0] || spawnPoint == spawnBasesIzq[1] ? 1 : -1);
        }
    }

    public void GenerateMainElement()
    {
        // Elegir la dirección y el sprite del mainElement
        int direction = Random.value > 0.5f ? 1 : -1;
        Sprite chosenSprite = direction == 1 ? flecha_derecha : flecha_izquierda;

        // Crear el objeto principal y configurarlo
        mainElement = Instantiate(flecha_prefab, mainBase);
        MovAvion movAvion = mainElement.GetComponent<MovAvion>();
        movAvion.canvas = canvas;
        movAvion.direct = direction;
        movAvion.isMainElement = true; // Especificar que es el objeto principal
        movAvion.SetDir(direction);

        // Asignar el sprite al objeto principal
        mainElement.GetComponent<Image>().sprite = chosenSprite;
    }

    public void CheckObject(GameObject droppedObject)
    {
        if (droppedObject.GetComponent<MovAvion>().direct == direct_actual)
        {
            Destroy(droppedObject);
            Debug.Log("Destruyendo objeto acertado");
            SumarPuntos();
        }
        else
        {
            Fallo();
        }
    }

    public void Fallo()
    {
        Debug.Log("Fallaste");
        audioSource.PlayOneShot(wrong);
        avioneta.GetComponent<avionetaMovement>().Fallo();
    }

    public void SumarPuntos()
    {
        estrellitas[currentPoints].GetComponent<nube>().brillar(); // Llamamos al método brillar que hace la animación de la nube al pasar de nivel
        currentPoints++;
        CheckPoints();
    }

    private void CheckPoints()
    {
        if (currentPoints == pointsToWin)
        {
            Invoke("DesaparecerAviones", 0.5f);
            if (num_scene == 1)
            {
                avioneta.GetComponent<avionetaMovement>().GanarDer();
                audioSource.PlayOneShot(winLevel);
                Debug.Log("¡Has ganado!");
                Invoke("VolverNorte", 3f);
            }
            else
            {
                // Cargar la siguiente escena
                Invoke("LoadNextScene", 1f);
            }
        }
        else
        {
            // Opcional: Si se usa `mainElement`, se puede destruir aquí si es necesario
            // Destroy(mainElement);
            // Invoke("GenerateMainElement", 1f);
        }
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
        num_scene++;
    }

     public void DesaparecerAviones()
    {
        CancelInvoke("SpawnRandomObject");
        foreach (Transform child in contenedor_aviones)
        {
            MovAvion avion = child.GetComponent<MovAvion>();
            if (avion != null)
            {
                avion.Desaparecer();
            }
        }
    }

    public void PlayInstruct()
    {
        audioSource.PlayOneShot(instruction);
    }

    public void VolverNorte()
    {
        SceneManager.LoadScene(19);
    }
}
