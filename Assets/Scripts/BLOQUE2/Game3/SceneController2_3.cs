using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SceneController2_3 : MonoBehaviour
{
    [SerializeField] private GameObject[] huevos;
    private int currentPoints = 0;
    [SerializeField] private GameObject[] animales;
    [SerializeField] private GameObject[] objetos_casa;
    [SerializeField] private GameObject[] main_objects;
    [SerializeField] private GameObject[] altavoces_lisst;
    private GameObject currentSelectedAltavoz;
    private GameObject currentSelectedItem;
    private GameObject currentSelectedImage;
    [SerializeField] private GameObject elementos;
    [SerializeField] private GameObject altavoces;
    [SerializeField] public AudioClip winLevel;
    [SerializeField] public AudioClip wrong;
    [SerializeField] public AudioClip instruction;
    [SerializeField] private GameObject avioneta;
    AudioSource audioSource;
    private List<GameObject> instantiatedElements = new List<GameObject>();
    [SerializeField] private Sprite altavoz;
    [SerializeField] private Sprite altavoz_correcto;
    private int id_selected;
    private List<Transform> elementosChildren = new List<Transform>();
    private List<Transform> altavocesChildren = new List<Transform>();

    public void Start()
    {
        AssignIDs(animales);
        AssignIDs(objetos_casa);
        audioSource = GetComponent<AudioSource>();
        Invoke("PlayInstruct", 0.2f);
        main_objects = new GameObject[5];
        altavoces_lisst = new GameObject[5];
        // Almacenar referencias a los hijos de elementos y altavocesll
        foreach (Transform child in elementos.transform)
        {
            elementosChildren.Add(child);
        }
        foreach (Transform child in altavoces.transform)
        {
            altavocesChildren.Add(child);
        }
        GenerateMainElements();
        GenerateOptions();
    }

    public void GenerateMainElements()
    {
        GameObject[] selectedArray;
        if (currentPoints >= 0 && currentPoints < 5)
        {
            selectedArray = animales;
        }
        else
        {
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
        for (int i = 0; i < indices.Length; i++)
        {
            altavoces_lisst[indices[i]] = Instantiate(selectedArray[indices[i]], altavocesChildren[i]);
            Element option1 = altavoces_lisst[indices[i]].GetComponent<Element>();
            option1.Init(1, 0, 0); //el principal
            instantiatedElements.Add(altavoces_lisst[indices[i]]);
        }
    }

    public void GenerateOptions()
    {
        GameObject[] selectedArray;

        if (currentPoints >= 0 && currentPoints < 5)
        {
            // Select from animales
            selectedArray = animales;
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
        for (int i = 0; i < indices.Length; i++)
        {
            main_objects[indices[i]] = Instantiate(selectedArray[indices[i]], elementosChildren[i]);
            Element option1 = main_objects[indices[i]].GetComponent<Element>();
            option1.Init(0, 0, 0); //el principal
            instantiatedElements.Add(main_objects[indices[i]]);
        }
        Invoke("OcultarElementosIniciales", 3f);
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

    public bool CheckObject(int id)
    {
        Debug.Log("id: " + id + " id_selected: " + id_selected);
        Debug.Log("selected_altav: " + currentSelectedAltavoz + " selected_image: " + currentSelectedImage + " selected_item: " + currentSelectedItem);
        if (id == id_selected)
        {
            SumarPuntos();
            currentSelectedAltavoz = null; // Reset the current selected altavoz
            currentSelectedItem = null;
            currentSelectedImage = null;
            return true;
        }
        else
        {
            Fallo();
            //cambiar sprite del altavoz al original
            if (id_selected != -1 && currentSelectedAltavoz != null)
            {
                Element elementScript = currentSelectedAltavoz.GetComponent<Element>();
                elementScript.SetAltavozOriginal();

            }
            else if (id_selected != -1 && currentSelectedImage != null)
            {
                Element el = currentSelectedImage.GetComponent<Element>();
                el.OcultarElementoFade();
            }
            id_selected = -1;
            currentSelectedAltavoz = null; // Reset the current selected altavoz
            currentSelectedItem = null;
            currentSelectedImage = null;
            return false;
        }
    }

    public void CheckPoints()
    {
        if (currentPoints == 10) //num de elementos
        {
            Debug.Log("Ganaste");
            avioneta.GetComponent<avionetaMovement>().GanarDer();
            audioSource.PlayOneShot(winLevel);
            Invoke("VolverItalia", 3f);
            //añadir sonido de los pajaritos volando
        }
        else if (currentPoints == 5)
        {
            Invoke("DestroyOptions", 1.5f); //destruimos los pájaros
            Invoke("GenerateMainElements", 2f);
            Invoke("GenerateOptions", 2f);
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

    public void setSelected_altavoz(GameObject altavoz)
    {
        // Restablecer el sprite del altavoz previamente seleccionado
        if (currentSelectedAltavoz != null)
        {
            Element elementScript = currentSelectedAltavoz.GetComponent<Element>();
            elementScript.SetAltavozOriginal();
        }

        // Establecer el nuevo altavoz seleccionado
        currentSelectedAltavoz = altavoz;
        currentSelectedItem = currentSelectedAltavoz; //elemento seleccionado actual es un altavoz
        Element elementScript_new = currentSelectedAltavoz.GetComponent<Element>();
        id_selected = elementScript_new.get_id();
        getSelected_Item();
    }

    public void setSelected_Image(GameObject image)
    {
        if (currentSelectedImage != null)
        {
            Element elementScript = currentSelectedImage.GetComponent<Element>();
            elementScript.OcultarElementoFade();
        }
        // Establecer el nuevo altavoz seleccionado
        currentSelectedImage = image;
        currentSelectedItem = image;
        Element elementScript_new = currentSelectedItem.GetComponent<Element>();
        id_selected = elementScript_new.get_id();
        getSelected_Item();
    }

    public String getSelected_Item()
    {
        string type;
        if (currentSelectedItem != null)
        {
            if (currentSelectedAltavoz != null)
            {
                type = "altavoz";
            }
            else
            {
                type = "image";
            }
        }
        else
        {
            type = "null";
        }
        Debug.Log("seletected item " + type);
        return type;
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
