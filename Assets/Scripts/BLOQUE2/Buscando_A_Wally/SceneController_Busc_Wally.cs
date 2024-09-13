using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class SceneController_Busc_Wally : MonoBehaviour
{
    private int currentPoints = 0;
    [SerializeField] private GameObject[] elementos;
    private GameObject mainElement;
    [SerializeField] private GameObject elementsBaseUI;
    private int currentId;
    //[SerializeField] private GameObject finalElementsBase;
    private int[] shuffle_indexes;
    [SerializeField] public AudioClip winLevel;
    [SerializeField] public AudioClip wrong;
    [SerializeField] private AudioClip intruct;
    [SerializeField] public AudioClip acierto;
    [SerializeField] private GameObject avioneta;
    AudioSource audioSource;

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        shuffle_indexes = new int[19];
        for (int i = 0; i < shuffle_indexes.Length; i++) {
            shuffle_indexes[i] = i;
        }
        shuffle_indexes = ShuffleArray(shuffle_indexes);
        
        GenerateElement();
        Invoke("PlayInstruct", 0.2f);
    }

    public void GenerateElement()
    {
        Set_ids();
        mainElement = Instantiate(elementos[shuffle_indexes[currentPoints]], elementsBaseUI.transform);
        mainElement.GetComponent<Elem_Busc_Wally>().Set_Raycast_Target(false);
        currentId = mainElement.GetComponent<Elem_Busc_Wally>().getId();
    }

    public void SumarPuntos()
    {
        audioSource.PlayOneShot(acierto);
        currentPoints++;
        CheckPoints();
    }

    public void Fallo()
    {
        Debug.Log("Fallaste");
        audioSource.PlayOneShot(wrong);
        avioneta.GetComponent<avionetaMovement>().Fallo();
    }

    public void CheckObject(GameObject element){
        Elem_Busc_Wally elem_script = element.GetComponent<Elem_Busc_Wally>();
        Debug.Log(currentId + ", " + elem_script.getId());
        if(currentId == elem_script.getId()){
            SumarPuntos();
            elem_script.go_to_finalbase(); //desplazar elemento al final_base
        }else{
            Fallo();
        }
    }

    public void CheckPoints()
    {
        if (currentPoints == 8)
        {
            Debug.Log("Ganaste");
            avioneta.GetComponent<avionetaMovement>().GanarDer();
            audioSource.PlayOneShot(winLevel);
            Invoke("VolverEspaña", 3f);
        }
        else
        {
            Destroy(mainElement);
            Invoke("GenerateElement", 1f);
        }
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
        for(int i = 0; i < newArray.Length; i++){
            int tmp = newArray[i];
            //Debug.Log(tmp);
        }
        return newArray;
    }

    public void Set_ids(){
        for(int i=0; i<elementos.Length; i++){
            elementos[i].GetComponent<Elem_Busc_Wally>().setId(i);
            Debug.Log("elemento" + elementos[i].name + "id: " + i);
        }
    }

    public void PlayInstruct()
    {
        audioSource.PlayOneShot(intruct);
    }
    
    public void VolverEspaña()
    {
        SceneManager.LoadScene(2);
    }
}