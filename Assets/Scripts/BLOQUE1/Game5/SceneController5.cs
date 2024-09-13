using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SceneController5 : MonoBehaviour
{

    private Combis originalFruit;
    [SerializeField] private GameObject[] mainFruits2;
    private Vector2 scaleFruit;
    [SerializeField] private GameObject[] fruits2;
    [SerializeField] private GameObject[] fruits3;
    private GameObject[] temFruits; // lista temporal sobre la que instanciaremos las fruits durante cada ronda
    [SerializeField] private GameObject mainElementBaseUI; // Objeto cuadrado
    [SerializeField] private ElementsBaseUI elementsBaseUI;
    [SerializeField] private GameObject[] nubes;
    //[SerializeField] private GameObject nubeColorida;
    AudioSource audioSource;
    [SerializeField] public AudioClip win;
    [SerializeField] public AudioClip wrong;
    [SerializeField] private GameObject avioneta;

    private int[] fruitnumb;
    private int currentFruitIndex = 0;
    private int currentPoints = 0;
    private int finalPoints;
    private int currentCombis = 2;
    private int currentOptions = 4;

    [SerializeField] private Sprite interrogante;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); //recuperamos el AudioSource de la nube
    }

    void Start()
    {
        finalPoints = 8;
        Debug.Log("num frutas: " + finalPoints);
        temFruits = new GameObject[fruits2.Length]; //ya que no almacenamos el mainFruit
        fruitnumb = new int[fruits2.Length];
        for (int i = 0; i < fruits2.Length; i++)
        {
            fruitnumb[i] = i;
        }
        GenerateMainFruit(0);
        Debug.Log("main generado");
        GenerateInitialFruits(); //Generamos las primeras frutas
        StartCoroutine(ChangeSprite());
    }
    public void GenerateMainFruit(int i)
    {
        //PARA GENERAR LA MAINFRUIT
        if (currentCombis == 2)
        {
            originalFruit = fruits2[i].GetComponent<Combis>(); //la obtenemos como fruta a adivinar
            Debug.Log("La fruta obtenida es: " + fruits2[i].tag);
        }
        else
        {
            originalFruit = fruits3[i].GetComponent<Combis>(); //la obtenemos como fruta a adivinar
            Debug.Log("La fruta obtenida es: " + fruits3[i].tag);
        }
        originalFruit = Instantiate(originalFruit, mainElementBaseUI.transform);
        originalFruit.DesactivarCollider();
        originalFruit.VolverAlCentro(0); //primero ubicamos la main fruit en el centro
        originalFruit.OcultarImagenes(); //en un primer momento no queremos que se vean los elementos de la primera imagen

    }
    public void GenerateInitialFruits()
    {
        //elementsBaseUI.GenerateInitialFruits();
        Debug.Log("Ronda Nueva");
        List<int> shuffledIndexes = new List<int>(ShuffleArray(fruitnumb, currentOptions)); //solo vamos a añadir 8 nums más a parte de la main
        Debug.Log("el indice del main es: " + currentFruitIndex + "la fruta es: " + originalFruit.tag);

        // Asegurarse de que la originalFruit esté presente entre las frutas generadas
        if (!shuffledIndexes.Contains(currentFruitIndex))
        {
            // Insertar la originalFruit en una posición aleatoria
            int randomIndex = Random.Range(0, currentOptions);
            Debug.Log("el indice del main es: " + currentFruitIndex + " y se inserta en la pos: " + randomIndex);
            shuffledIndexes.Insert(randomIndex, currentFruitIndex);
        }
        for (int i = 0; i < currentOptions; i++)
        {
            int index = shuffledIndexes[i];
            Debug.Log("index: " + index);
            if(currentCombis == 2){
                temFruits[i] = Instantiate(fruits2[index], elementsBaseUI.transform);
            }else{
                temFruits[i] = Instantiate(fruits3[index], elementsBaseUI.transform);
            }
            Debug.Log("creado " + index);
            temFruits[i].GetComponent<Combis>().Desparecer(); //hasta que no se complete la animación de la main fruit no aparecerán

        }
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

    public void RegenerateFruits()
    {
        currentFruitIndex++;
        if(currentPoints == 2){
            currentOptions = 6;
        }else if(currentPoints == 4){
            currentOptions = 4;
            currentCombis = 3;
        }else if(currentPoints== 6){
            currentOptions = 6;
        }
        GenerateMainFruit(currentFruitIndex);
        GenerateInitialFruits();
        StartCoroutine(ChangeSprite());
    }


    //Barajar números para que aparezcan de manera aleatoria
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
        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            Debug.Log(tmp);
        }
        int[] nCombisArray = new int[nCombis]; //array con los nCombis primeros nums del array
        for (int i = 0; i < nCombis; i++)
        {
            nCombisArray[i] = newArray[i];
        }

        return nCombisArray;
    }

    public void CheckFruit(Combis fruitselected)
    {
        Debug.Log("Vamos a ver si adivinas la fruta");
        if (fruitselected.tag == originalFruit.tag)
        {
            Debug.Log("Adivinaste");
            if (currentFruitIndex < nubes.Length - 1)
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

    private IEnumerator ChangeSprite()
    {
        yield return StartCoroutine(originalFruit.ChangeSprite(interrogante));
        for (int i = 0; i < currentOptions; i++)
        {
            temFruits[i].GetComponent<Combis>().Reaparecer();
            Debug.Log("He reaparecido");
        }
    }

}
