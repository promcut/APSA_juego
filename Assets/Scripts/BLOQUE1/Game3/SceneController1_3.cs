using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using System.Collections;
using DG.Tweening;


public class SceneController1_3 : MonoBehaviour
{
    [SerializeField] private AudioClip winLevel;
    [SerializeField] private AudioClip wrong;
    [SerializeField] public AudioClip instruct;
    [SerializeField] private GameObject avioneta;
    private AudioSource audioSource;

    [SerializeField] private GameObject[] huevos;
    private int currentPoints = 0;
    private List<GameObject> instantiatedElements = new List<GameObject>();
    [SerializeField] private GameObject elementsBaseUI;
    [SerializeField] private GameObject mainElementsBaseUI;
    public GameObject[] objects;
    private GameObject[] current_objects_selected;
    private GameObject[] mainObjects;
    private int current_elements = 0;
    int[] shuffledIndexes;

    [SerializeField] private GameObject dropZoneBase;
    [SerializeField] private Sprite correct_base;
    private Sprite originalSprite_base;

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
        GenerateMainElements();
        Invoke("PlayInstruct", 0.2f);
    }

    public void GenerateMainElements()
    {

        int[] shuffled;
        if(currentPoints<=2)
        {
            shuffled = ShuffleArray(shuffledIndexes, 3);
        }else{
            shuffled = ShuffleArray(shuffledIndexes, 4);
        }
        mainObjects = new GameObject[shuffled.Length];
        current_objects_selected = new GameObject[mainObjects.Length];
        for (int i = 0; i < mainObjects.Length; i++)
        {
            int index = shuffled[i];
            mainObjects[i] = Instantiate(objects[index], mainElementsBaseUI.transform);
            mainObjects[i].GetComponent<Element_playa1_2_drag>().SetRaycastTarget(false);
            instantiatedElements.Add(mainObjects[i]);
        }
        
        for (int i = 0; i < objects.Length; i++)
        {
            int index = shuffledIndexes[i];
            GameObject object_inst = Instantiate(objects[index], elementsBaseUI.transform);
            instantiatedElements.Add(object_inst);
        }
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
        foreach (GameObject element in current_objects_selected)
        {
            Element_playa1_2_drag element_script = element.GetComponent<Element_playa1_2_drag>();
            StartCoroutine(element_script.ReturnToPosition());
        }
        AllowRaycast();
    }

public IEnumerator CheckObject(GameObject new_element)
{
    current_objects_selected[current_elements] = new_element;
    current_elements++;
    Debug.Log("current_elements: " + current_elements);
    
    if(current_elements == mainObjects.Length)
    {
        NoRaycast();
        yield return new WaitForSeconds(0.7f);
        bool allCorrect = true;
        // Comparar los objetos seleccionados con los objetos principales en orden
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
            // Si todos son correctos, sumar puntos
            SumarPuntos();
            StartCoroutine(AnimationBaseAcierto());
        }
        else
        {
            // Si no son correctos, indicar fallo
            Fallo();
        }
        // Reiniciar el estado para la siguiente ronda
        current_objects_selected = new GameObject[mainObjects.Length];
        current_elements = 0;
    }
}


    public void CheckPoints()
    {
        if (currentPoints == 6) //num de elementos
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
        Image image = dropZoneBase.GetComponent<Image>();
        RectTransform rectTransform = dropZoneBase.GetComponent<RectTransform>();

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
        Image image = dropZoneBase.GetComponent<Image>();
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
                option.GetComponent<Element_playa1_2_drag>().SetRaycastTarget(true);
            }
        }
    }

    public void NoRaycast()
    {
        foreach (GameObject option in instantiatedElements)
        {
            if (option != null)
            {
                option.GetComponent<Element_playa1_2_drag>().SetRaycastTarget(false);
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
