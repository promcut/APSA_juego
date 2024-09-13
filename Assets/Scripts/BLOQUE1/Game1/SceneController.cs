using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneController : MonoBehaviour
{
    public const float offsetX = 2.5f; //distancia entre frutas x
    public const float offsetY = 2f; //distancia entre frutas y
    // Start is called before the first frame update
    public int colNum = 2;
    public int rowNum = 2;

    [SerializeField] private MainFruit originalFruit;
    [SerializeField] private GameObject[] mainFruits;
    private Vector2 scaleFruit;
    [SerializeField] private GameObject[] fruits;
    [SerializeField] private GameObject squareObject; // Objeto cuadrado
    [SerializeField] private GameObject[] nubes;
    [SerializeField] private GameObject nubeColorida;
    AudioSource audioSource;
    [SerializeField] public AudioClip win;
    [SerializeField] public AudioClip wrong;
    [SerializeField] private GameObject avioneta;

    private int[] fruitnumb;
    private int currentFruitIndex = 1;
    private int currentPoints = 0;
    private int finalPoints;
    
    private void Awake() {
        audioSource = GetComponent<AudioSource>(); //recuperamos el AudioSource de la nube
    }

    void Start()
    {
        finalPoints = mainFruits.Length;
        Debug.Log("num frutas: " + finalPoints);

        fruitnumb = new int[mainFruits.Length];
        for (int i = 0; i < mainFruits.Length; i++)
        {
            fruitnumb[i] = i;
        }
        GenerateMainFruit(0);
        GenerateInitialFruits(); //Generamos las primeras frutas
    }
    public void GenerateMainFruit(int i)
    {
        //PARA GENERAR LA MAINFRUIT
        Vector2 vectorinicial = new Vector2(squareObject.transform.position.x / 1.5f, squareObject.transform.position.y * -2.5f);
        originalFruit = mainFruits[i].GetComponent<MainFruit>();
        Debug.Log("La fruta obtenida es: " + mainFruits[i].tag);
        scaleFruit = originalFruit.transform.localScale; //guardamos la escala orginal para luego
        originalFruit.transform.localScale = scaleFruit * 1.5f; //aumentamos la escala de la 
        Instantiate(originalFruit, vectorinicial, Quaternion.identity);
    }
    public void GenerateInitialFruits()
    {
        //PARA GENERAR LAS FRUTAS CHIQUITAS
        Vector3 startPos = squareObject.transform.position; //Posicion del cuadrado que contendrá las frutas
        // Calcula el tamaño total de la cuadrícula
        float squareWidth = squareObject.transform.localScale.x; //coordenada x del cuadrado
        float squareHeight = squareObject.transform.localScale.y; //coordenada y del cuadrado
        Debug.Log("Las coordenadas son:" + squareHeight + ", " + squareWidth);

        // Ajusta la posición inicial para centrar la cuadrícula
        //startPos.x -= squareWidth / 4.5f;
        //startPos.y -= squareHeight / 3.5f;
        startPos.x = -1.16f;
        startPos.y = -3.23f;
        int[] numbers = new int[colNum * rowNum]; // Array NumeroTotal de frutas

        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = i; // Inicializar el array con los índices
        }

        numbers = ShuffleArray(numbers); //Mezclamos los números
        for (int i = 0; i < colNum; i++)
        {
            for (int j = 0; j < rowNum; j++)
            {
                int index = j * colNum + i;
                int id = numbers[index];

                GameObject fruitPrefab = fruits[id % fruits.Length];
                if (fruitPrefab.tag == originalFruit.tag)
                {
                    fruitPrefab.transform.localScale = scaleFruit; // Devolvemos la escala original
                }
                GameObject fruit = Instantiate(fruitPrefab, transform);

                float posX = (offsetX * i) + startPos.x;
                float posY = (offsetY * j) + startPos.y;

                // Asegurar que la fruta esté dentro de los límites del cuadrado
                posX = Mathf.Clamp(posX, startPos.x, startPos.x + squareWidth);
                posY = Mathf.Clamp(posY, startPos.y, startPos.y + squareHeight);

                fruit.transform.position = new Vector3(posX, posY, startPos.z);

            }
        }
    }

    public void DestroyAllFruits()
    {
        string[] fruitTags = { "tortilla", "jamon", "queso", "naranja", "churros", "aceite", "tapa", "huevo", "paella" }; //tags de las frutas
        foreach (string tag in fruitTags)
        {
            GameObject[] fruitsWithTag = GameObject.FindGameObjectsWithTag(tag); // Encuentra todos los objetos con el tag específico
            foreach (GameObject fruit in fruitsWithTag)
            {
                if (fruit != null)
                {
                    Destroy(fruit); // Destruye cada objeto de fruta encontrado con el tag específico
                }
            }
        }
    }

    public void RegenerateFruits()
    {
        GenerateMainFruit(currentFruitIndex);
        GenerateInitialFruits();
        currentFruitIndex++;
    }


    //Barajar números para que aparezcan de manera aleatoria
    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }

    public void CheckFruit(MainFruit fruitselected)
    {
        Debug.Log("Vamos a ver si adivinas la fruta");
        if (fruitselected.tag == originalFruit.tag)
        {
            Debug.Log("Adivinaste");
            if (currentFruitIndex < mainFruits.Length)
            {
                //FUNCIÓN DE SUMA DE PUNTOS
                SumarPuntos();
                DestroyAllFruits();
                RegenerateFruits(); // Llamar a la función para regenerar las frutas
            }
            else
            {
                nubes[currentPoints].GetComponent<nube>().brillar();//Última nube, es una chapuza hay que cambiarlo
                Debug.Log("TERMINASTE");
                avioneta.GetComponent<avionetaMovement>().Ganar();
                audioSource.PlayOneShot(win);
                Invoke("VolverEspaña", 3f);
            }
        }
        else
        {
            Debug.Log("Inténtalo de nuevo");
            audioSource.PlayOneShot(wrong);
            avioneta.GetComponent<avionetaMovement>().Fallo();

        }
    }

    public void SumarPuntos()
    {
        //nubes[currentPoints].SetActive(false);
        nubes[currentPoints].GetComponent<nube>().brillar(); //Llamamos al método brillar que hace la animación de la nube al pasar de nivel
        //Instantiate(nubeColorida, nubes[currentPoints].transform.position, Quaternion.identity);
        currentPoints++;
    }

    private void VolverEspaña()
    {
        // Cargar la nueva escena
        SceneManager.LoadScene(2);
    }

}
