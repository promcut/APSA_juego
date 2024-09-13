using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SceneController2_2 : MonoBehaviour
{
    [SerializeField] private GameObject[] huevos;
    private int currentPoints = 0;
    [SerializeField] private GameObject[] animales;
    [SerializeField] private GameObject[] vehiculos;
    [SerializeField] private GameObject[] objetos_casa;

    private GameObject elementDrop;
    [SerializeField] private GameObject elementsBaseUI;
    [SerializeField] private GameObject mainElement;
    private int[] shuffle_indexes;
    int index = 0;
    int index1 = 0;
    int index2 = 0;
    [SerializeField] public AudioClip winLevel;
    [SerializeField] public AudioClip wrong;
    [SerializeField] public AudioClip instruction;
    [SerializeField] public AudioClip escucha;
    [SerializeField] private GameObject avioneta;
    AudioSource audioSource;
    private int current_id;
    private List<GameObject> instantiatedElements = new List<GameObject>();
    [SerializeField] private Sprite altavoz;
    private int iteration = 0;

    public void Start()
    {
        AssignIDs(animales);
        AssignIDs(vehiculos);
        AssignIDs(objetos_casa);
        audioSource = GetComponent<AudioSource>();
        Invoke("PlayEscucha", 0.2f);

        shuffle_indexes = new int[4];
        for (int i = 0; i < 4; i++)
        {
            shuffle_indexes[i] = i;
            Debug.Log(shuffle_indexes[i]);
        }
        shuffle_indexes = ShuffleArray(shuffle_indexes);
        Invoke("GenerateElement", 0.2f);
        Invoke("GenerateOptions", 0.2f);
        Invoke("PlayInstruct", 3f);
        //GenerateElement();
        //GenerateOptions();
    }

    public void GenerateElement()
    {
        //cada 2 puntos cambia de set de iniciación
        if (currentPoints >= 0 && currentPoints < 2)
        {
            // Instantiate animales
            int randomIndex = shuffle_indexes[index];
            index++;
            elementDrop = Instantiate(animales[randomIndex], mainElement.transform);
        }
        else if (currentPoints >= 2 && currentPoints < 4)
        {
            // Instantiate vehiculos
            int randomIndex = shuffle_indexes[index1];
            index1++;
            elementDrop = Instantiate(vehiculos[randomIndex], mainElement.transform);
        }
        else
        {
            // Instantiate objetos_casa
            int randomIndex = shuffle_indexes[index2];
            index2++;
            elementDrop = Instantiate(objetos_casa[randomIndex], mainElement.transform);
        }
        Element element1 = elementDrop.GetComponent<Element>();
        if(iteration == 0)
        {
            element1.Init(1, 1, 1); //el principal
            iteration ++;
        }else{
            element1.Init(1, 1, 0); //el principal
        }
        current_id = elementDrop.GetComponent<Element>().get_id();
        instantiatedElements.Add(elementDrop);
    }

    public void GenerateOptions()
    {

        GameObject[] selectedArray;

        if (currentPoints >= 0 && currentPoints < 2)
        {
            // Select from animales
            selectedArray = animales;
        }
        else if (currentPoints >= 2 && currentPoints < 4)
        {
            // Select from vehiculos
            selectedArray = vehiculos;
        }
        else
        {
            // Select from objetos_casa
            selectedArray = objetos_casa;
        }

        int[] indices = new int[selectedArray.Length];
        for (int i = 0; i < selectedArray.Length; i++)
        {
            indices[i] = i;
        }

        // Barajar los índices
        indices = ShuffleArray(indices);

        // Instanciar los elementos en el orden barajado
        foreach (int index in indices)
        {
            GameObject option = Instantiate(selectedArray[index], elementsBaseUI.transform);
            Element option1 = option.GetComponent<Element>();
            option1.Init(0, 0, 0); //el principal
            instantiatedElements.Add(option);
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
        Invoke("PlayElementSound", 1.0f); // Invocar play_sound después de 1 segundo
    }

    // Método auxiliar para invocar play_sound
    private void PlayElementSound()
    {
        elementDrop.GetComponent<Element>().play_sound();
    }

    public bool CheckObject(int id)
    {
        if (id == current_id)
        {
            SumarPuntos();
            elementDrop.GetComponent<Element>().Revelar();
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
        if (currentPoints == 6) //num de elementos
        {
            Debug.Log("Ganaste");
            avioneta.GetComponent<avionetaMovement>().GanarDer();
            audioSource.PlayOneShot(winLevel);
            Invoke("VolverItalia", 3f);
            //añadir sonido de los pajaritos volando
        }
        else
        {
            Invoke("DestroyElement", 1.5f); //destruimos el objeto actual
            Invoke("DestroyOptions", 1.5f); //destruimos los pájaros
            Invoke("GenerateElement", 2f);
            Invoke("GenerateOptions", 2f);
        }
    }

    public void DestroyElement()
    {
        if (elementDrop != null)
        {
            Destroy(elementDrop);
            instantiatedElements.Remove(elementDrop);
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


    private void AssignIDs(GameObject[] elements)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].GetComponent<Element>().set_id(i);
        }
    }
    public void PlayInstruct()
    {
        audioSource.PlayOneShot(instruction);
    }
    
    public void PlayEscucha()
    {
        audioSource.PlayOneShot(escucha);
    }

    public void VolverItalia()
    {
        // Cargar la nueva escena
        SceneManager.LoadScene(10);
    }
}
