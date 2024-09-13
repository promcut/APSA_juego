using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using Unity.VisualScripting;

[System.Serializable]
public class GameRound
{
    public Sprite element1Sprite;
    public string element1Name;
    public AudioClip sound1;
    public Vector3 scale1;
    public Sprite element2Sprite;
    public string element2Name;
    public AudioClip sound2;
    public Vector3 scale2;
}

public class SceneController2_4 : MonoBehaviour
{
    [SerializeField] public AudioClip winLevel;
    [SerializeField] public AudioClip wrong;
    [SerializeField] public AudioClip instruction;
    [SerializeField] private GameObject avioneta;
    AudioSource audioSource;
    [SerializeField] private GameObject[] huevos;
    private int currentPoints = 0;
    public List<GameRound> rounds; // Configura esto en el Inspector
    private List<GameObject> instantiatedElements = new List<GameObject>();
    [SerializeField] private GameObject elementsBaseUI;
    [SerializeField] private GameObject mainElement;
    public GameObject elementPrefab; // Prefab para instanciar los elementos
    private GameObject currentElementOnBase;
    [SerializeField]private GameObject[] estrellitas;
    private int randomIndex;
    private int iteration = 0;


    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Invoke("PlayInstruct", 0.2f);
        GenerateMainElements();
    }

    public void GenerateMainElements()
    {
        GameRound currentRound = rounds[currentPoints];

        // Instancia los elementos en el grid
        GameObject element1 = Instantiate(elementPrefab, elementsBaseUI.transform);
        Debug.Log(currentRound.element1Name + " " + currentRound.element1Sprite.name);
        element1.GetComponent<Element_palabras>().SetElement(currentRound.element1Sprite, currentRound.element1Name, currentRound.sound1, currentRound.scale1);
        element1.GetComponent<Element_palabras>().Init(0,0);
        instantiatedElements.Add(element1);

        GameObject element2 = Instantiate(elementPrefab, elementsBaseUI.transform);
        element2.GetComponent<Element_palabras>().SetElement(currentRound.element2Sprite, currentRound.element2Name, currentRound.sound2, currentRound.scale2);
        element2.GetComponent<Element_palabras>().Init(0,0);
        instantiatedElements.Add(element2);

        // Selecciona aleatoriamente uno de los elementos
        randomIndex = Random.Range(0, 2);
        GameObject selectedElement = randomIndex == 0 ? element1 : element2;

        // Instancia un nuevo objeto basado en el seleccionado y colócalo en la base
        GameObject mainObject = Instantiate(elementPrefab, mainElement.transform);
        if (randomIndex == 0)
        {
            mainObject.GetComponent<Element_palabras>().SetElement(currentRound.element1Sprite, currentRound.element1Name,  currentRound.sound1,currentRound.scale1);
        }
        else
        {
            mainObject.GetComponent<Element_palabras>().SetElement(currentRound.element2Sprite, currentRound.element2Name, currentRound.sound2, currentRound.scale2);
        }
        mainObject.GetComponent<Element_palabras>().Init(1,1);
        currentElementOnBase = mainObject;
        instantiatedElements.Add(mainObject);

        if(iteration == 0)
        {
            Invoke("PlayMainElementSound", 3f); //esperamos a que suene la instruccion
            iteration ++;
        }else
        {
            StartCoroutine(currentElementOnBase.GetComponent<Element_palabras>().play_sound());
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
    }

    public bool CheckObject(GameObject selected_Element)
    {            
        Debug.Log("name1: "+ selected_Element.GetComponent<Element_palabras>().elementName + "name2: " + currentElementOnBase.GetComponent<Element_palabras>().elementName);

        if (selected_Element.GetComponent<Element_palabras>().elementName == currentElementOnBase.GetComponent<Element_palabras>().elementName)
        {
            SumarPuntos();
            //estrellita de triunfo
            estrellitas[randomIndex].GetComponent<estrellitas>().Brillo_Estrellita();
            selected_Element.GetComponent<Element_palabras>().AnimateElement();
            return true;
        }
        else
        {
            Fallo();
            return false;
        }
    }

    public void CheckPoints()
    {
        if (currentPoints == 7) //num de elementos
        {
            Debug.Log("Ganaste");
            avioneta.GetComponent<avionetaMovement>().GanarDer();
            audioSource.PlayOneShot(winLevel);
            Invoke("VolverItalia", 3f);
            //añadir sonido de los pajaritos volando
        }
        else
        {
            Invoke("DestroyOptions", 1.5f); 
            Invoke("Desaparecer_Estrellitas", 1.5f);
            Invoke("GenerateMainElements", 2f);
        }
    }

    public void Desaparecer_Estrellitas(){
        foreach (GameObject estrella in estrellitas){
            estrella.GetComponent<estrellitas>().Desaparecer_Estrellita();
        }
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
        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            Debug.Log("num: " + tmp);
        }
        return newArray;
    }

    public void PlayInstruct()
    {
        audioSource.PlayOneShot(instruction);
    }

    public void PlayMainElementSound()
    {
        StartCoroutine(currentElementOnBase.GetComponent<Element_palabras>().play_sound());
    }

    public void VolverItalia()
    {
        // Cargar la nueva escena
        SceneManager.LoadScene(10);
    }

    public void OcultarElementosIniciales()
    {
        foreach (GameObject element in instantiatedElements)
        {
            element.GetComponent<Element>().OcultarElementoInicialFade();
        }
    }
}
