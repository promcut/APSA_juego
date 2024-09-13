using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using System.Collections;
using DG.Tweening;


public class SceneController1_4 : MonoBehaviour
{
    [SerializeField] private AudioClip winLevel;
    [SerializeField] private AudioClip wrong;
    [SerializeField] private AudioClip instruct1;
    [SerializeField] private AudioClip instruct2;
    [SerializeField] private GameObject avioneta;
    private AudioSource audioSource;

    [SerializeField] private GameObject[] huevos;
    private int currentPoints = 0;
    private List<GameObject> instantiatedElements = new List<GameObject>();
    [SerializeField] private GameObject[] elementsBaseUI;
    [SerializeField] private GameObject mainElementsBaseUI1;
    [SerializeField] private GameObject mainElementsBaseUI2;
    [SerializeField] private GameObject mainElementsBaseUI3;
    public GameObject[] objects;
    private GameObject[] current_objects_selected;
    private GameObject[] objects_to_guess;
    private GameObject[] mainObjects;
    private int current_elements = 0;
    int[] shuffledIndexes;
    public Sprite interrogante;
    private GameObject mainbase;

    [SerializeField] private Sprite correct_base;
    private Sprite originalSprite_base;

    private Dictionary<int, HashSet<int>> usedIndexesBySeries = new Dictionary<int, HashSet<int>>();


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); //recuperamos el AudioSource de la nube
    }

    public void Start()
    {
        int[] objnumb = new int[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            objnumb[i] = i;
        }
        Debug.Log("main generado");
        shuffledIndexes = ShuffleArray(objnumb, 8);
        for (int i = 0; i < shuffledIndexes.Length; i++)
        {
            Debug.Log(shuffledIndexes[i]);
        }
        // Inicializa el diccionario de índices usados
        foreach (int count in new int[] { 1, 2, 3 })
        {
            usedIndexesBySeries[count] = new HashSet<int>();
        }
        GenerateMainElements();
        Invoke("PlayInstruct1", 0.2f);
    }

    public void GenerateMainElements()
    {
        int numElementsToGenerate;

        if (currentPoints < 4)
        {
            numElementsToGenerate = 1;
            mainbase = mainElementsBaseUI1;
            mainElementsBaseUI2.SetActive(false);
            mainElementsBaseUI3.SetActive(false);
        }
        else if (currentPoints >= 4 && currentPoints < 8)
        {
            numElementsToGenerate = 2;
            mainbase = mainElementsBaseUI2;
            mainElementsBaseUI1.SetActive(false);
            mainElementsBaseUI2.SetActive(true);
            mainElementsBaseUI3.SetActive(false);
        }
        else
        {
            numElementsToGenerate = 3;
            mainbase = mainElementsBaseUI3;
            mainElementsBaseUI1.SetActive(false);
            mainElementsBaseUI2.SetActive(false);
            mainElementsBaseUI3.SetActive(true);
        }

        mainObjects = new GameObject[numElementsToGenerate];
        current_objects_selected = new GameObject[mainObjects.Length];

        // Crear una lista de índices disponibles
        List<int> availableIndexes = new List<int>(shuffledIndexes);
        for (int i = 0; i < availableIndexes.Count; i++)
        {
            if (usedIndexesBySeries[numElementsToGenerate].Contains(availableIndexes[i]))
            {
                availableIndexes.RemoveAt(i);
                i--;
            }
        }
        //hay suficientes indices
        if (availableIndexes.Count < numElementsToGenerate)
        {
            Debug.LogWarning("No hay suficientes índices disponibles para generar la serie.");
            return;
        }

        // aleatorio para esta serie
        for (int i = 0; i < numElementsToGenerate; i++)
        {
            int randomIndex = Random.Range(0, availableIndexes.Count);
            int index = availableIndexes[randomIndex];

            mainObjects[i] = Instantiate(objects[index], mainbase.transform);
            mainObjects[i].GetComponent<MainFruit2>().SetRaycastTarget(false);
            instantiatedElements.Add(mainObjects[i]);
            StartCoroutine(mainObjects[i].GetComponent<MainFruit2>().Parpadeo(interrogante));

            Invoke("PlayInstruct2", 6f); //reproducimos la segunda parte de la instruccion
            
            //marcamos como usado
            usedIndexesBySeries[numElementsToGenerate].Add(index);

            //eliminamos para que no se repita
            availableIndexes.RemoveAt(randomIndex);
        }

        //generamos los demas objetos
        objects_to_guess = new GameObject[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            int index = shuffledIndexes[i];
            objects_to_guess[i] = Instantiate(objects[index], elementsBaseUI[i].transform);
            instantiatedElements.Add(objects_to_guess[i]);
            objects_to_guess[i].GetComponent<MainFruit2>().Desparecer();
        }

        Invoke("ReaparecerObjects", 5f);
    }

    public void SumarPuntos()
    {
        huevos[currentPoints].GetComponent<nube>().brillar(); //Llamamos al método brillar que hace la animación de la nube al pasar de nivel
        currentPoints++;
        CheckPoints();
    }

    public void Fallo()
    {
        Debug.Log("Fallaste");
        audioSource.PlayOneShot(wrong);
        avioneta.GetComponent<avionetaMovement>().Fallo();
        AllowRaycast();
        foreach (GameObject main in mainObjects)
        {
            main.GetComponent<MainFruit2>().StartShowImage();
        }
        foreach (GameObject obj in current_objects_selected)
        {
            obj.GetComponent<MainFruit2>().ReturnColor();
        }
    }

    public void CheckObject(GameObject new_element)
    {
        current_objects_selected[current_elements] = new_element;
        current_elements++;
        Debug.Log("current_elements: " + current_elements);

        if (current_elements == mainObjects.Length)
        {
            Debug.Log("Compruebo si todo bien");
            NoRaycast();
            bool allCorrect = true;
            //comparamos en orden
            for (int i = 0; i < mainObjects.Length; i++)
            {
                if (current_objects_selected[i].name != mainObjects[i].name)
                {
                    allCorrect = false;
                    break;
                }
            }
            if (allCorrect)
            {
                SumarPuntos();
                StartCoroutine(AnimationBaseAcierto());
                ReaparecerMainObjects();
            }
            else
            {
                Fallo();
            }
            current_objects_selected = new GameObject[mainObjects.Length];
            current_elements = 0;
        }
    }


    public void CheckPoints()
    {
        if (currentPoints == 10) //num de elementos
        {
            Debug.Log("Ganaste");
            avioneta.GetComponent<avionetaMovement>().GanarDer();
            audioSource.PlayOneShot(winLevel);
            Invoke("VolverEspaña", 3f);
        }
        else
        {
            NoRaycast();
            Invoke("DestroyOptions", 2f);
            Invoke("GenerateMainElements", 3f);
        }
    }

    IEnumerator AnimationBaseAcierto()
    {
        // Obtener el Image y RectTransform del mainElementBaseUI
        Image image = mainbase.GetComponent<Image>();
        RectTransform rectTransform = mainbase.GetComponent<RectTransform>();

        Vector3 originalScale = rectTransform.localScale;
        originalSprite_base = image.sprite;
        yield return rectTransform.DOScale(originalScale * 1.2f, 0.5f).SetEase(Ease.OutQuad).WaitForCompletion();
        image.sprite = correct_base;
        yield return rectTransform.DOScale(originalScale, 0.5f).SetEase(Ease.OutQuad).WaitForCompletion();
        yield return new WaitForSeconds(1f);
        ReturnBaseSprite();
    }

    public void ReturnBaseSprite()
    {
        Image image = mainbase.GetComponent<Image>();
        image.sprite = originalSprite_base;
    }

    public void DestroyOptions()
    {
        foreach (GameObject option in instantiatedElements)
        {
            if (option != null)
            {
                Destroy(option);
            }
        }
        instantiatedElements.Clear();
    }

    public void AllowRaycast()
    {
        foreach (GameObject option in instantiatedElements)
        {
            if (option != null)
            {
                option.GetComponent<MainFruit2>().SetRaycastTarget(true);
            }
        }
    }

    public void NoRaycast()
    {
        foreach (GameObject option in instantiatedElements)
        {
            if (option != null)
            {
                option.GetComponent<MainFruit2>().SetRaycastTarget(false);
            }
        }
    }

    public void ReaparecerObjects()
    {
        foreach (GameObject ob in objects_to_guess)
        {
            ob.GetComponent<MainFruit2>().Reaparecer();
        }
    }

    public void ReaparecerMainObjects()
    {
        foreach (GameObject ob in mainObjects)
        {
            ob.GetComponent<MainFruit2>().OriginalSprite();
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
    
    public void PlayInstruct1()
    {
        audioSource.PlayOneShot(instruct1);
    }

    public void PlayInstruct2()
    {
        audioSource.PlayOneShot(instruct2);
    }

    public void VolverEspaña()
    {
        // Cargar la nueva escena
        SceneManager.LoadScene(2);
    }
}
