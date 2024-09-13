using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using System.Collections;
using Unity.VisualScripting;
using System.Linq;


public class SceneController2_6 : MonoBehaviour
{
    [SerializeField] private AudioClip winLevel;
    [SerializeField] private AudioClip wrong;
    [SerializeField] public AudioClip instruction;
    [SerializeField] private GameObject avioneta;
    private AudioSource audioSource;

    [System.Serializable]
    public class GameRound6
    {
        public Sprite elementSprites;
        public string elementNames;
        public AudioClip sound;
        public int numb_silb;
        public Vector3 scale;
    }


    [SerializeField] private GameObject[] huevos;
    private int currentPoints = 0;
    public List<GameRound6> rounds; // Configura esto en el Inspector
    private List<GameObject> instantiatedElements = new List<GameObject>();
    [SerializeField] private GameObject elementsBaseUI;
    [SerializeField] private GameObject elementPrefab; // Prefab para instanciar los elementos
    private GameObject currentElementOnBase;
    private int current_num = 0;
    private int num_silb;
    [SerializeField] private Button button;
    [SerializeField] private GameObject[] bases;
    [SerializeField] private GameObject bird;
    [SerializeField] private Sprite[] sprite_bird;
    [SerializeField] private GameObject door;
    private List<Bird_6> birds = new List<Bird_6>();
    private int iteracion = 0;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Invoke("PlayInstruct", 0.2f);
        ShuffleRounds();
        GenerateMainElements();
        GenerateBirds();
    }

    private void ShuffleRounds()
    {
        List<GameRound6> initialRounds = rounds.Take(rounds.Count - 3).ToList();
        List<GameRound6> finalRounds = rounds.Skip(rounds.Count - 3).ToList();

        // Shuffle both lists separately
        initialRounds = initialRounds.OrderBy(x => Random.value).ToList();
        finalRounds = finalRounds.OrderBy(x => Random.value).ToList();

        // Combine the lists back together
        rounds = initialRounds.Concat(finalRounds).ToList();
    }

    public void GenerateMainElements()
    {
        button.GetComponent<Image>().raycastTarget = true;
        GameRound6 currentRound = rounds[currentPoints];
        currentElementOnBase = Instantiate(elementPrefab, elementsBaseUI.transform);
        currentElementOnBase.GetComponent<Element_palabras_6>().SetElement(
            currentRound.elementSprites,
            currentRound.elementNames,
            currentRound.sound,
            currentRound.numb_silb,
            currentRound.scale
        );
        instantiatedElements.Add(currentElementOnBase);
        num_silb = currentRound.numb_silb;
        StartCoroutine(Play_Sounds()); //audio de los objetos por primera vez
    }

    public void Sonidos_Para_Boton()
    { //para el botón del altavoz
        StartCoroutine(Play_Sounds());
    }

    public void Sonidos_Para_Fallo()
    {
        StartCoroutine(Play_Sounds());
    }

    public IEnumerator Play_Sounds()
    {
        if(iteracion == 0)
        {
            yield return new WaitForSeconds(9.5f);
            iteracion ++;
        }
        StartCoroutine(currentElementOnBase.GetComponent<Element_palabras_6>().play_sound());
    }

    public void SumarPuntos()
    {
        huevos[currentPoints].GetComponent<nube>().brillar(); //Llamamos al método brillar que hace la animación de la nube al pasar de nivel
        currentPoints++;
        CheckPoints();
    }

    public void Fallo()
    {
        ReturnPajaritos();
        Debug.Log("Fallaste");
        audioSource.PlayOneShot(wrong);
        avioneta.GetComponent<avionetaMovement>().Fallo();
        Invoke("Sonidos_Para_Fallo", 1f);
    }

    public void AddNumber()
    {
        current_num++;
    }

    public bool CheckObject()
    {
        StartCoroutine(door.GetComponent<Door>().OriginalSprite());
        Debug.Log("current: " + current_num + " ACTUAL: " + num_silb);
        if (current_num == num_silb)
        {
            SumarPuntos();
            current_num = 0;

            return true;
        }
        else
        {
            Fallo();
            current_num = 0;

            return false;
        }
    }

    public void CheckPoints()
    {
        button.GetComponent<Image>().raycastTarget = false;
        if (currentPoints == 8) //num de elementos
        {
            Debug.Log("Ganaste");
            avioneta.GetComponent<avionetaMovement>().GanarDer();
            audioSource.PlayOneShot(winLevel);
            Invoke("VolverItalia", 3f);
            //añadir sonido de los pajaritos volando
        }
        else
        {
            SetRayCastNullBirds(); //para que no se vayan contando pajaritos en la siguiente ronda MEDIDA DE SEGURIDAD
            Invoke("DestroyOptions", 2f);
            Invoke("DestroyBirds", 2f);//destruimos los pájaros
            Invoke("GenerateMainElements", 3f);
            Invoke("GenerateBirds", 3f);
        }
    }

    public void GenerateBirds()
    {
        birds.Clear();
        for (int i = 0; i < bases.Length; i++)
        {
            GameObject birdy = Instantiate(bird, bases[i].transform);
            Bird_6 birdScript = birdy.GetComponent<Bird_6>();
            birdScript.SetElement(sprite_bird[i]);
            birdScript.SetRaycastTarget(i == 0); // Solo el primer pájaro tiene raycast habilitado
            birds.Add(birdScript);
        }
    }

    public void OnBirdClicked(Bird_6 clickedBird)
    {
        int index = birds.IndexOf(clickedBird);
        if (index >= 0 && index < birds.Count - 1)
        {
            birds[index + 1].SetRaycastTarget(true); // Habilita el siguiente pájaro
        }
    }

    public void ReturnPajaritos()
    {
        string tag = "Respawn";
        GameObject[] birdsTag = GameObject.FindGameObjectsWithTag(tag); // Encuentra todos los pájaros con el tag específico
        foreach (GameObject bird in birdsTag)
        {
            if (bird != null)
            {
                bird.GetComponent<Bird_6>().RestoreOriginalState();
                bird.GetComponent<Bird_6>().SetRaycastTarget(false); //devolvemos al estado original a los birds
            }
        }
        birds[0].GetComponent<Bird_6>().SetRaycastTarget(true); //solo habilitamos el raycast del primer bird
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

    public void SetRayCastNullBirds(){
        string tag = "Respawn";
        GameObject[] birdsTag = GameObject.FindGameObjectsWithTag(tag); // Encuentra todos los objetos con el tag específico
        foreach (GameObject bird in birdsTag)
        {
            if (bird != null)
            {
                 bird.GetComponent<Bird_6>().SetRaycastTarget(false); //devolvemos al estado original a los birds
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
