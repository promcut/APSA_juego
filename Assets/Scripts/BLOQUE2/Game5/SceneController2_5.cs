using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using System.Collections;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.Video;

[System.Serializable]
public class GameRound5
{
    public Sprite[] elementSprites = new Sprite[6];
    public string[] elementNames = new string[6];
    public AudioClip[] sounds = new AudioClip[6];
    public Vector3[] scales = new Vector3[6];
}

public class SceneController2_5 : MonoBehaviour
{
    [SerializeField] private AudioClip winLevel;
    [SerializeField] private AudioClip wrong;
    [SerializeField] public AudioClip instruction;
    [SerializeField] private GameObject avioneta;
    private AudioSource audioSource;

    [SerializeField] private GameObject[] huevos;
    private int currentPoints = 0;
    public List<GameRound5> rounds; // Configura esto en el Inspector
    private List<GameObject> instantiatedElements = new List<GameObject>();
    [SerializeField] private GameObject elementsBaseUI;
    [SerializeField] private GameObject elementPrefab; // Prefab para instanciar los elementos
    private GameObject currentElementOnBase;
    private GameObject[] current_objects;
    private GameObject[] current_objects_selected;
    private int current_elements = 0;
    public GameObject nido_central;
    private bool playing_sound = false;
    [SerializeField] private Button button;
    public List<Nido> nidos = new List<Nido>();
    [SerializeField] private GameObject[] nidos_totales;
    private int iteracion = 0;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Invoke("PlayInstruct", 0.2f);
        nido_central.SetActive(false);
        GenerateMainElements();
    }

    public void GenerateMainElements()
    {
        button.GetComponent<Image>().raycastTarget = true;
        GameRound5 currentRound = rounds[currentPoints];
        int elementsToGenerate = currentPoints >= 4 ? 6 : 4;
        current_elements = 0;
        if(currentPoints >= 4){
            nido_central.SetActive(true);
        } 
        GetNidos();
        for (int i = 0; i < elementsToGenerate; i++)
        {
            GameObject element = Instantiate(elementPrefab, elementsBaseUI.transform);
            element.GetComponent<Element_palabras_5>().SetElement(
                currentRound.elementSprites[i],
                currentRound.elementNames[i],
                currentRound.sounds[i],
                currentRound.scales[i]
            );
            instantiatedElements.Add(element);
        }

        int elementsToSelect = currentPoints < 4 ? 2 : 3;
        List<GameObject> temporaryList = new List<GameObject>(instantiatedElements);
        List<GameObject> selectedElements = new List<GameObject>();

        for (int i = 0; i < elementsToSelect; i++)
        {
            int randomIndex = Random.Range(0, temporaryList.Count);
            selectedElements.Add(temporaryList[randomIndex]);
            temporaryList.RemoveAt(randomIndex); // borramos de la copia y no del original para que luego no nos afecte en el Remove
        }

        // Convertir la lista a un array
        current_objects = selectedElements.ToArray();
        current_objects_selected = new GameObject[current_objects.Length];

        StartCoroutine(Play_Sounds()); //audio de los objetos por primera vez
    }

    public void Sonidos_Para_Boton()
    { //para el botón del altavoz
        StartCoroutine(Play_Sounds());
    }

    public void Sonidos_Para_Fallo()
    { //para el botón del altavoz
        StartCoroutine(Play_Sounds());
    }

    public IEnumerator Play_Sounds()
    {
        if(iteracion == 0)
        {
            yield return new WaitForSeconds(5f);
            iteracion ++;
        }
        if(!playing_sound){
            playing_sound = true;
            NoRaycast(); //hasta que no acabe el audio no se pueden arrastrar los objetos
            for (int i = 0; i < current_objects.Length; i++)
            {
                Element_palabras_5 element = current_objects[i].GetComponent<Element_palabras_5>();
                element.play_sound();
                yield return new WaitForSeconds(element.audioClip.length + 0.5f);
            }
            AllowRaycast();
            playing_sound = false;
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
        for (int i=0; i<=current_elements; i++){
            Debug.Log("devuelvo el "+ i + " elemento");
            Element_palabras_5 element = current_objects_selected[i].GetComponent<Element_palabras_5>();
            element.ReturnToBox();
        }
        Invoke("Sonidos_Para_Fallo", 1f);
    }

    public IEnumerator CheckObject(GameObject new_element)
    {
        yield return new WaitForSeconds(0.4f);
        // Agregar el nuevo elemento a la lista de elementos seleccionados
        current_objects_selected[current_elements] = new_element;
        Debug.Log("current_elements: " + current_elements);
        Debug.Log("current_objects_selected: " + current_objects_selected[current_elements].GetComponent<Element_palabras_5>().elementName + " current_object: " + current_objects[current_elements].GetComponent<Element_palabras_5>().elementName);
        if(current_objects_selected[current_elements].GetComponent<Element_palabras_5>().elementName == current_objects[current_elements].GetComponent<Element_palabras_5>().elementName){
            current_elements++;
            if(current_elements == current_objects.Length){
                SumarPuntos();
                GetNidos();
            }
            //no hay que volver a resetear los nidos pq se ha sumado una iteración y ha acertado un nido
        }else{
            Fallo();
            current_objects_selected = new GameObject[current_objects.Length];
            current_elements = 0;
            GetNidos();
        }
        
        
        //Version anterior en la que se checkeaba únicamente cuando estaban todos los elementos en los nidos
        /*if (current_elements == current_objects.Length)
        {
            
            // Obtener los nombres de los elementos originales y seleccionados
            List<string> originalNames = current_objects
                .Select(obj => obj.GetComponent<Element_palabras_5>().elementName)
                .ToList();
            List<string> selectedNames = current_objects_selected
                .Select(obj => obj.GetComponent<Element_palabras_5>().elementName)
                .ToList();

            // Comprobar si ambas listas de nombres son iguales
            if (originalNames.SequenceEqual(selectedNames))
            {
                SumarPuntos();
            }
            else
            {
                Fallo();
                current_objects_selected = new GameObject[current_objects.Length];
                current_elements = 0;
            }
            GetNidos();
        }*/
    }

    public void CheckPoints()
    {
        button.GetComponent<Image>().raycastTarget = false;
        if (currentPoints == 7) //num de elementos
        {
            Debug.Log("Ganaste");
            avioneta.GetComponent<avionetaMovement>().GanarDer();
            audioSource.PlayOneShot(winLevel);
            Invoke("VolverItalia", 3f);
        }
        else
        {
            NoRaycast();
            Invoke("DestroyOptions", 2f);
            Invoke("GenerateMainElements", 3f);
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

    public void GetNidos(){
        nidos.Clear();
        foreach (GameObject nido in nidos_totales)
        {
            if (nido.transform.parent.gameObject.activeSelf)
            {
                nidos.Add(nido.GetComponent<Nido>()); // Añade el objeto a la lista si está habilitado
                nido.GetComponent<Nido>().SetRaycastTarget(false);
            }
        }
        Debug.Log("Nidos actuales: "+ nidos.Count);
        nidos[0].SetRaycastTarget(true); //el primero de los nidos tiene que tener el raycast en true
    }

    public void OnNidoDropped(Nido nido_anterior){
        int index = nidos.IndexOf(nido_anterior);
        if (index >= 0 && index < nidos.Count - 1)
        {
            nidos[index + 1].SetRaycastTarget(true); // Habilita el siguiente pájaro
        }
    }

    public void OcultarElementosIniciales()
    {
        foreach (GameObject element in instantiatedElements)
        {
            element.GetComponent<Element>().OcultarElementoInicialFade();
        }
    }

    public void AllowRaycast(){
        foreach (GameObject option in instantiatedElements)
        {
            if (option != null)
            {
                option.GetComponent<Element_palabras_5>().AllowRaycast();
            }
        }
    }

    public void NoRaycast(){
        foreach (GameObject option in instantiatedElements)
        {
            if (option != null)
            {
                option.GetComponent<Element_palabras_5>().NoRaycast();
            }
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
}
