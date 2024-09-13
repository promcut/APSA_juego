using System.Collections;
using System.Globalization;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class SceneController22 : MonoBehaviour
{
    [SerializeField] private GameObject[] nubes;
    private int currentPoints = 0;
    [SerializeField] private GameObject[] elementos;
    [SerializeField] private GameObject[] bases;
    [SerializeField] private GameObject bird;
    [SerializeField] private GameObject[] birds;
    private GameObject elementDrop;
    [SerializeField] private ElementsBaseUI elementsBaseUI;
    private int[] shuffle_indexes;
    [SerializeField] public AudioClip winLevel;
    [SerializeField] public AudioClip wrong;
    [SerializeField] public AudioClip instruction1;
    [SerializeField] public AudioClip instruction2;
    [SerializeField] public AudioClip instruction3;
    [SerializeField] public AudioClip instruction_general;
    [SerializeField] public AudioClip recordatorio_puerta;
    [SerializeField] private GameObject avioneta;
    [SerializeField] private GameObject door;
    AudioSource audioSource;
    private int sounds = 0;
    private int current_sounds = 0;
    private int iteracion = 0;
    private bool raycastState;

    private float inactivityTimer = 0f; //timer
    private float inactivityThreshold = 10f; // 15s
    private bool stop_timer = false;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        shuffle_indexes = new int[6]; //num de elementos
        for (int i = 0; i < shuffle_indexes.Length; i++)
        {
            shuffle_indexes[i] = i;
        }
        shuffle_indexes = ShuffleArray(shuffle_indexes.Skip(1).ToArray()); //no contamos el primer elemento en el shuffle pq siempre será el mismo
        for (int i = 0; i < shuffle_indexes.Length; i++) Debug.Log( shuffle_indexes[i] + "i: " + elementos[shuffle_indexes[i]].name);
        birds = new GameObject[4];
        Invoke("PlayInstruct1", 0.2f);
        GenerateInitialElement();
        Invoke("PlayElementSound", 4f);
        GenerateBirds();
        Invoke("StartMonitor", 5f); //empezamos a monitorear el tiempo de inactividad de la pantalla
    }

    public void GenerateInitialElement()
    {
        elementDrop = Instantiate(elementos[0], elementsBaseUI.transform);
        elementDrop.GetComponent<Animal>().SetRaycast(false);
        sounds = elementDrop.GetComponent<Animal>().get_num_sound();
    }

    public void GenerateElement()
    {
        Debug.Log("Iteration: " + iteracion);
        //OnUserActivity();
        Debug.Log("Chosen: " + shuffle_indexes[currentPoints - 1]);
        elementDrop = Instantiate(elementos[shuffle_indexes[currentPoints - 1]], elementsBaseUI.transform); //generar los elementos que hacen sonidos
        sounds = elementDrop.GetComponent<Animal>().get_num_sound();
        elementDrop.GetComponent<Animal>().SetRaycast(false);
        //Debug.Log("elementDrop: " + elementDrop.name);
        if (iteracion == 1)
        {
            SetRaycastBirds(false);
            Invoke("PlayInstruct_General", 1f); //intruccion general
            iteracion++;
            Invoke("PlayElementSound", 6f);
            raycastState = true;
            Invoke("InvokeSetRaycastBirds", 6f);
            Invoke("StartMonitor", 10f);
        }
        else
        {
            Invoke("StartMonitor", 5f);
            Invoke("PlayElementSound", 1f); //le damos 1sec de margen pq si no coge elementDrop como null
        }
        SetRaycastBirds(true);
    }

    public void SumarPuntos()
    {
        nubes[currentPoints].GetComponent<nube>().brillar(); //Llamamos al método brillar que hace la animación de la nube al pasar de nivel
        currentPoints++;
        CheckPoints();
        OnUserActivity();
    }

    public void Fallo()
    {
        OnUserActivity();
        //Debug.Log("Fallaste");
        audioSource.PlayOneShot(wrong);
        avioneta.GetComponent<avionetaMovement>().Fallo();
        Invoke("PlayElementSound", 1.0f);
        //StartCoroutine(elementDrop.GetComponent<Animal>().play_sound());
    }

    private void PlayElementSound()
    {
        StartCoroutine(elementDrop.GetComponent<Animal>().play_sound());
    }

    public void AddNumber()
    {
        current_sounds++;
        //Debug.Log("sounds = " + current_sounds);
    }

    public bool CheckSounds()
    {
        StartCoroutine(door.GetComponent<Door>().OriginalSprite());
        if (current_sounds == sounds)
        {
            current_sounds = 0;
            SumarPuntos();
            if (iteracion == 0)
            {
                //Debug.Log("primera iter");
            }
            return true;

        }
        else
        {
            current_sounds = 0;
            RestoreValuesBirds();
            Fallo();
            return false;
        }
    }

    public void CheckPoints()
    {
        Debug.Log("Checking, stop timer");
        //StopCoroutine(MonitorInactivity());
        stop_timer = true; //para el timer
        SetRaycastBirds(false);
        if (currentPoints == elementos.Length) //num de elementos
        {
            Debug.Log("Ganaste");
            FlyBirds();
            DestroyElement();
            avioneta.GetComponent<avionetaMovement>().GanarDer();
            audioSource.PlayOneShot(winLevel);
            Invoke("VolverItalia", 3f);
            //añadir sonido de los pajaritos volando
        }
        else if (iteracion == 0)
        {
            Invoke("PlayInstruct3", 1f);
            Invoke("FlyBirds", 3f);
            Invoke("DestroyElement", 5f);//destruimos el objeto actual
            Invoke("DestroyBirds", 5f);//destruimos los pájaros
            Invoke("GenerateElement", 6f);
            Invoke("GenerateBirds", 6f);
            iteracion++;
        }
        else
        {
            FlyBirds();
            Invoke("DestroyElement", 3f);//destruimos el objeto actual
            Invoke("DestroyBirds", 3f);//destruimos los pájaros
            Invoke("GenerateElement", 4.5f);
            Invoke("GenerateBirds", 4.5f);
        }
    }

    public void GenerateBirds()
    {
        //cuando se ha acabado la ronda volver a generar los pajaritos
        for (int i = 0; i < bases.Length; i++)
        {
            Instantiate(bird, bases[i].transform);
        }

    }

    public void DestroyElement()
    {
        Destroy(elementDrop);
    }

    public void DestroyBirds()
    {
        string tag = "Respawn";
        GameObject[] birdsTag = GameObject.FindGameObjectsWithTag(tag); // Encuentra todos los objetos con el tag específico
        foreach (GameObject bird in birdsTag)
        {
            if (bird != null)
            {
                Destroy(bird); // Destruye cada objeto de fruta encontrado con el tag específico
            }
        }

    }

    public void FlyBirds()
    {
        string tag = "Respawn";
        GameObject[] birdsTag = GameObject.FindGameObjectsWithTag(tag); // Encuentra todos los objetos con el tag específico
        foreach (GameObject bird in birdsTag)
        {
            if (bird != null)
            {
                bird.GetComponent<Bird>().Volar();
            }
        }
    }

    public void RestoreValuesBirds()
    {
        string tag = "Respawn";
        GameObject[] birdsTag = GameObject.FindGameObjectsWithTag(tag); // Encuentra todos los objetos con el tag específico
        foreach (GameObject bird in birdsTag)
        {
            if (bird != null)
            {
                bird.GetComponent<Bird>().RestoreValueBird();
            }
        }
    }

    public void SetRaycastBirds(bool state)
    {
        string tag = "Respawn";
        GameObject[] birdsTag = GameObject.FindGameObjectsWithTag(tag); // Encuentra todos los objetos con el tag específico
        foreach (GameObject bird in birdsTag)
        {
            if (bird != null)
            {
                bird.GetComponent<Bird>().SetRaycast(state);
            }
        }
    }

    private void InvokeSetRaycastBirds()
    {
        SetRaycastBirds(raycastState);
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
            //Debug.Log(tmp);
        }
        return newArray;
    }

    public void PlayInstruct1()
    {
        SetRaycastBirds(false);
        StartCoroutine(PlayInstruct1_2());
    }

    public IEnumerator PlayInstruct1_2()
    {
        audioSource.PlayOneShot(instruction1);
        yield return new WaitForSeconds(instruction1.length + 4.5f); //esto es un poco chapuza pero se supone que es para darle margen al sonido del elemento
        StartCoroutine(PlayInstruct2());
    }

    public IEnumerator PlayInstruct2()
    {
        audioSource.PlayOneShot(instruction2);
        yield return new WaitForSeconds(instruction2.length);
        SetRaycastBirds(true);
    }

    public void PlayInstruct3()
    {
        audioSource.PlayOneShot(instruction3);
    }

    public void PlayInstruct_General()
    {
        audioSource.PlayOneShot(instruction_general);
    }

    private IEnumerator MonitorInactivity()
    {
        inactivityTimer = 0;
        while (!stop_timer)
        {
            inactivityTimer += Time.deltaTime;

            if (inactivityTimer >= inactivityThreshold)
            {
                Debug.Log("timer: "+ inactivityTimer);
                if (iteracion == 0)
                {
                    audioSource.PlayOneShot(instruction2);
                    inactivityTimer = 0f;
                }
                else
                {
                    //Debug.Log("Timer: " + inactivityTimer);
                    audioSource.PlayOneShot(recordatorio_puerta);
                    inactivityTimer = 0f;
                }
            }
            yield return null;
        }
    }

    public void StartMonitor()
    {
        //inactivityTimer = 0f;
        Debug.Log("starting timer: "+ inactivityTimer);
        stop_timer = false;
        StartCoroutine(MonitorInactivity());
    }

    public void OnUserActivity()
    {
        inactivityTimer = 0f;
    }

    public void VolverItalia()
    {
        SceneManager.LoadScene(10);
    }
}