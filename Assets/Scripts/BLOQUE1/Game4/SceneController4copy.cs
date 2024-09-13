using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SceneController4copy : MonoBehaviour
{

    private MainFruit2 originalFruit;
    [SerializeField] private GameObject[] mainFruits;
    private Vector2 scaleFruit;
    [SerializeField] private GameObject[] fruits;
    [SerializeField] private GameObject mainElementBaseUI; // Objeto cuadrado
    [SerializeField] private ElementsBaseUI elementsBaseUI;
    [SerializeField] private GameObject[] nubes;
    [SerializeField] private GameObject bloqueo_touching;
    //[SerializeField] private GameObject nubeColorida;
    AudioSource audioSource;
    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip wrong;
    [SerializeField] private GameObject avioneta;

    private int[] fruitnumb;
    private int currentFruitIndex = 1;
    private int currentPoints = 0;
    private int finalPoints;

    [SerializeField] private Sprite interrogante; 

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); //recuperamos el AudioSource de la nube
        GridLayoutGroup elementsGridLayout = elementsBaseUI.GetComponent<GridLayoutGroup>();
        GridLayoutGroup mainElementGridLayout = mainElementBaseUI.GetComponent<GridLayoutGroup>();
    }

    void Start()
    {
        finalPoints = mainFruits.Length;
        Debug.Log("num frutas: " + finalPoints);

        fruitnumb = new int[fruits.Length];
        for (int i = 0; i < fruits.Length; i++)
        {
            fruitnumb[i] = i;
        }
        GenerateMainFruit(0);
        Debug.Log("main generado");
        GenerateInitialFruits(); //Generamos las primeras frutas
        Debug.Log("Vamos a ocultar al fruta");
        StartCoroutine(ChangeSprite());
    }
    public void GenerateMainFruit(int i)
    {
        //PARA GENERAR LA MAINFRUIT
        originalFruit = mainFruits[i].GetComponent<MainFruit2>(); //la obtenemos como fruta a adivinar
        Debug.Log("La fruta obtenida es: " + mainFruits[i].tag);   
        originalFruit = Instantiate(originalFruit, mainElementBaseUI.transform);      
    }
    public void GenerateInitialFruits()
    {
        //elementsBaseUI.GenerateInitialFruits();
        int[] shuffledIndexes = ShuffleArray(fruitnumb);
        for (int i = 0; i < fruits.Length; i++)
        {
            int index = shuffledIndexes[i];
            Debug.Log("index: "+index);
            fruits[index].transform.localScale = new Vector3(1,1,1); //nos aseguramos de que el tamaño de la escala no es afectado por el ChangeSprite()
            Instantiate(fruits[index], elementsBaseUI.transform);
            /*if(fruits[index].tag == originalFruit.tag){
                MainFruit2 thisFruit = fruits[index].GetComponent<MainFruit2>();
                thisFruit.Parpadeo();
            }*/
            Debug.Log("creado "+ fruits[index].tag);  
        }
    }

    public void DestroyAllFruits()
    {
        string[] fruitTags = { "combi1", "combi2", "combi3", "combi4", "combi5", "combi6", "combi7", "combi8", "combi9"}; //tags de las frutas
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
        StartCoroutine(ChangeSprite());
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
        for(int i = 0; i < newArray.Length; i++){
            int tmp = newArray[i];
            Debug.Log(tmp);
        }
        return newArray;
    }

    public void CheckFruit(MainFruit2 fruitselected)
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
                avioneta.GetComponent<avionetaMovement>().GanarDer();
                audioSource.PlayOneShot(win);
                Invoke("VolverEspaña", 3f);
            }
        }
        else{
            Debug.Log("Inténtalo de nuevo");
            audioSource.PlayOneShot(wrong);
            avioneta.GetComponent<avionetaMovement>().Fallo();
        }
    }

    private void SumarPuntos()
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

    private IEnumerator ChangeSprite()
    {
        // Cambia el sprite del originalFruit
        Debug.Log("Empieza la corrutina");
        bloqueo_touching.SetActive(true);
        //originalFruit.Parpadeo();
        yield return new WaitForSeconds(5f);
        Debug.Log("Acaba el tiempo de espera");
        //originalFruit.ChangeSprite(interrogante);
        bloqueo_touching.SetActive(false);
        Debug.Log("Hemos cambiado el sprite");
    }

}
