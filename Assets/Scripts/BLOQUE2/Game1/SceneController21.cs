using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class SceneController21 : MonoBehaviour
{
    [SerializeField] private GameObject[] nubes;
    private int currentPoints = 0;
    [SerializeField] private GameObject[] elementos;
    private GameObject elementDrop;
    [SerializeField] private ElementsBaseUI elementsBaseUI;
    private int[] shuffle_indexes;
    [SerializeField] public AudioClip winLevel;
    [SerializeField] public AudioClip wrong;
    [SerializeField] private GameObject avioneta;
    AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        shuffle_indexes = new int[12];
        for (int i = 0; i < shuffle_indexes.Length; i++) {
            shuffle_indexes[i] = i;
        }
        shuffle_indexes = ShuffleArray(shuffle_indexes);
        GenerateElement();
    }

    public void GenerateElement()
    {
        elementDrop = Instantiate(elementos[shuffle_indexes[currentPoints]], elementsBaseUI.transform);
    }

    public void SumarPuntos()
    {
        nubes[currentPoints].GetComponent<nube>().brillar(); //Llamamos al método brillar que hace la animación de la nube al pasar de nivel
        currentPoints++;
        CheckPoints();
    }

    public void Fallo()
    {
        Debug.Log("Fallaste");
        audioSource.PlayOneShot(wrong);
        avioneta.GetComponent<avionetaMovement>().Fallo();
    }

    public void CheckPoints()
    {
        if (currentPoints == 12)
        {
            Debug.Log("Ganaste");
            avioneta.GetComponent<avionetaMovement>().GanarDer();
            audioSource.PlayOneShot(winLevel);
            Invoke("VolverItalia", 3f);
        }
        else
        {
            Invoke("GenerateElement", 1f);
            //GenerateElement();
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
    
    private void VolverItalia()
    {
        // Cargar la nueva escena
        SceneManager.LoadScene(10);
    }
}