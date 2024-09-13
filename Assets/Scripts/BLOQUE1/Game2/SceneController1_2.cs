using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController1_2 : MonoBehaviour
{

    private MainFruit2 originalFruit;
    [SerializeField] private GameObject[] fruits;
    [SerializeField] private GameObject[] black_fruits;
    [SerializeField] private GameObject[] mainElementsBaseUI; // Objeto cuadrado
    [SerializeField] private GameObject[] black_elementsBaseUI;
    [SerializeField] private GameObject[] nubes;
    AudioSource audioSource;
    [SerializeField] public AudioClip win;
    [SerializeField] public AudioClip wrong;
    [SerializeField] public AudioClip instruct;
    [SerializeField] private GameObject avioneta;
    [SerializeField] private Sprite correct_base;
    private Sprite originalSprite_base;
    private int[] fruitnumb;
    private int currentPoints = 0;
    private int finalPoints;
    private int roundPoints = 0;
    private int[] shuffledIndexes = new int[8];

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); //recuperamos el AudioSource de la nube
    }

    void Start()
    {
        finalPoints = fruits.Length;
        Debug.Log("num frutas: " + finalPoints);

        fruitnumb = new int[fruits.Length];
        for (int i = 0; i < fruits.Length; i++)
        {
            fruitnumb[i] = i;
        }
        Debug.Log("main generado");
        shuffledIndexes = ShuffleArray(fruitnumb, 8);
        int[] shuffledIndexes_4 = ShuffleArray(shuffledIndexes, 4);
        GenerateInitialFruits(true, shuffledIndexes_4); //Generamos las primeras frutas
        GenerateInitialFruits(false, shuffledIndexes_4); //Generamos las primeras frutas
        Invoke("PlayInstruct", 0.2f);
    }

    public void GenerateInitialFruits(bool black, int[] shuffled)
    {
        
        //for (int i = 0; i < shuffled.Length; i++) Debug.Log("index: " + shuffled[i]);
        if (black)
        {
            GameObject[] blackObjects = new GameObject[4];
            for (int i = 0; i < blackObjects.Length; i++)
            {
                int index = shuffled[i];
                blackObjects[i] = Instantiate(black_fruits[index], black_elementsBaseUI[i].transform);
                //blackObjects[i].GetComponent<Element_playa1_2>().Init(false);
            }
        }
        else
        {
            GameObject[] objects = new GameObject[4];
            int[] indexesMovidos = ShuffleArray(shuffled, 4);
            for (int i = 0; i < objects.Length; i++)
            {
                int index = indexesMovidos[i];
                objects[i] = Instantiate(fruits[index], mainElementsBaseUI[i].transform);
                //objects[i].GetComponent<Element_playa1_2>().Init(true);
            }
        }
    }

    public void RegenerateFruits()
    {
        shuffledIndexes = ShuffleArray(fruitnumb, 8);
        int[] shuffledIndexes_4 = ShuffleArray(shuffledIndexes, 4);
        GenerateInitialFruits(true, shuffledIndexes_4); //Generamos las primeras frutas
        GenerateInitialFruits(false, shuffledIndexes_4); //Generamos las primeras frutas
    }

    public void SumarPuntos()
    {
        roundPoints++;
        if (roundPoints == 4)
        {
            nubes[currentPoints].GetComponent<nube>().brillar(); //Llamamos al método brillar que hace la animación de la nube al pasar de nivel
        }
        CheckPoints();
    }

    public void Fallo()
    {
        Debug.Log("Inténtalo de nuevo");
        audioSource.PlayOneShot(wrong);
        avioneta.GetComponent<avionetaMovement>().Fallo();
    }

    public bool CheckPoints()
    {
        if (roundPoints == 4)
        {   currentPoints ++;
            if (currentPoints == 2)
            {
                //cambiar escena a la siguiente
                Invoke("CambiarEscena", 1.5f);
            }
            Invoke("DestroyAllFruits", 1.5f);
            Invoke("RegenerateFruits", 2f);
            Invoke("ActivarColliderElementos", 2f);
            roundPoints = 0;   
        }
        return true;
    }

    public void CambiarEscena()
    {
        SceneManager.LoadScene(29);
    }

    public void DestroyAllFruits()
    {
        string[] fruitTags = { "combi1", "combi2", "combi3", "combi4", "combi5", "combi6", "combi7", "combi8", "combi9" }; //tags de las frutas
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

    public void DesactivarColliderElementos()
    {
        string[] fruitTags = { "combi1", "combi2", "combi3", "combi4", "combi5", "combi6", "combi7", "combi8", "combi9" }; //tags de las frutas
        foreach (string tag in fruitTags)
        {
            GameObject[] fruitsWithTag = GameObject.FindGameObjectsWithTag(tag); // Encuentra todos los objetos con el tag específico
            foreach (GameObject fruit in fruitsWithTag)
            {
                if (fruit != null)
                {
                    fruit.GetComponent<MainFruit2>().SetRaycastTarget(false);
                }
            }
        }
    }

    public void ActivarElementos()
    {
        string[] fruitTags = { "combi1", "combi2", "combi3", "combi4", "combi5", "combi6", "combi7", "combi8", "combi9" }; //tags de las frutas
        foreach (string tag in fruitTags)
        {
            GameObject[] fruitsWithTag = GameObject.FindGameObjectsWithTag(tag); // Encuentra todos los objetos con el tag específico
            foreach (GameObject fruit in fruitsWithTag)
            {
                if (fruit != null)
                {
                    fruit.GetComponent<MainFruit2>().SetRaycastTarget(true);
                }
            }
        }
    }

    private int[] ShuffleArray(int[] numbers, int nCombis)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        int[] nCombisArray = new int[nCombis]; //array con los nCombis primeros nums del array
        for (int i = 0; i < nCombis; i++)
        {
            nCombisArray[i] = newArray[i];
            //Debug.Log("array_en_shuffle: "+ nCombisArray[i]);
        }
        return nCombisArray;
    }

    public void PlayInstruct()
    {
        audioSource.PlayOneShot(instruct);
    }

    public void VolverEspaña()
    {
        // Cargar la nueva escena
        SceneManager.LoadScene(2);
    }

}
