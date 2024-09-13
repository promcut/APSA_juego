using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class GameRound_1_2scene2
{
    public Sprite[] elementSprites = new Sprite[3];
    //public string[] elementNames = new string[3];
    public Sprite[] spritesBlack = new Sprite[3];
    public Vector3[] scales = new Vector3[3];
    public string[] tags = new string[3];
}

public class SceneController1_2_scene2 : MonoBehaviour
{
    private MainFruit2 originalFruit;
    public List<GameRound_1_2scene2> rounds;
    private List<GameRound_1_2scene2> selectedRounds; //rondas seleccionadas
    [SerializeField] private GameObject basePrefab_drop; //contiene el Element_playa_1_2_drop.cs
    [SerializeField] private GameObject basePrefab_drag; //contiene el Element_playa_1_2_drap.cs
    private GameObject mainFruit;
    [SerializeField] private GameObject mainElementBaseUI;
    [SerializeField] private GameObject[] black_elementsBaseUI;
    [SerializeField] private GameObject[] nubes;
    AudioSource audioSource;
    [SerializeField] public AudioClip win;
    [SerializeField] public AudioClip wrong;
    [SerializeField] private GameObject avioneta;
    private Sprite originalSprite_base;
    private int currentPoints = 2;
    private int finalPoints;
    private int currentRound = 0;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        SelectRandomRounds();
        Debug.Log("main generado");
        GenerateInitialFruits(currentRound);
    }

    private void SelectRandomRounds()
    {
        selectedRounds = new List<GameRound_1_2scene2>(); // Inicializamos la lista para almacenar las rondas seleccionadas
        List<int> availableIndices = new List<int> { 0, 1, 2, 3, 4, 5, 6 }; // Índices disponibles

        for (int i = 0; i < 4; i++) // Seleccionamos 4 rondas
        {
            int randomIndex = Random.Range(0, availableIndices.Count); // Selecciona un índice aleatorio
            int selectedIndex = availableIndices[randomIndex];
            selectedRounds.Add(rounds[selectedIndex]); // Agrega la ronda seleccionada
            availableIndices.RemoveAt(randomIndex); // Elimina el índice seleccionado para evitar duplicados
        }
    }

    public void GenerateInitialFruits(int round)
    {
        GameRound_1_2scene2 currentRound = selectedRounds[round];

        mainFruit = Instantiate(basePrefab_drag, mainElementBaseUI.transform);
        Element_playa1_2_drag mainFruitScript = mainFruit.GetComponent<Element_playa1_2_drag>();
        mainFruitScript.SetElement(currentRound.elementSprites[0], currentRound.scales[0], currentRound.tags[0]); //le pasamos el sprite original

        // Crear una lista de índices para las frutas negras y barajarla
        int[] indices = { 0, 1, 2 }; // asumiendo que hay 3 frutas negras
        indices = ShuffleArray(indices);

        // Generar las frutas negras en orden aleatorio
        for (int i = 0; i < black_elementsBaseUI.Length; i++)
        {
            int index = indices[i]; // Obtener un índice aleatorio de la lista barajada

            GameObject blackFruit = Instantiate(basePrefab_drop, black_elementsBaseUI[i].transform);
            Element_playa1_2_drop blackFruitScript = blackFruit.GetComponent<Element_playa1_2_drop>();
            blackFruitScript.SetElement(currentRound.spritesBlack[index], currentRound.scales[index], currentRound.tags[index]); //le pasamos los sprites black
        }
    }


    public void RegenerateFruits()
    {
        currentRound++;
        GenerateInitialFruits(currentRound);
    }

    public void SumarPuntos()
    {
        nubes[currentPoints].GetComponent<nube>().brillar();
        currentPoints++;
        CheckPoints();
    }

    public void Fallo()
    {
        Debug.Log("Inténtalo de nuevo");
        audioSource.PlayOneShot(wrong);
        avioneta.GetComponent<avionetaMovement>().Fallo();
    }

    public void CheckFruit(Element_playa1_2_scene2 elem)
    {
        if (mainFruit.GetComponent<Element_playa1_2_scene2>().GetId() == elem.GetId())
        {
            SumarPuntos();
        }
        else
        {
            Fallo();
            elem.SetRaycastTarget(true);
        }
    }

    public bool CheckPoints()
    {
        if (currentPoints == 6)
        {
            //fin
            Debug.Log("Ganaste");
            avioneta.GetComponent<avionetaMovement>().GanarDer();
            audioSource.PlayOneShot(win);
            Invoke("VolverEspaña", 3f);
        }
        else
        {
            Invoke("DestroyAllFruits", 1.5f);
            Invoke("RegenerateFruits", 2f);
        }
        return true;
    }

    public void DestroyAllFruits()
    {
        string[] fruitTags = { "combi1", "combi2", "combi3", "combi4", "combi5", "combi6", "combi7", "combi8", "combi9" }; //tags de las frutas
        foreach (string tag in fruitTags)
        {
            GameObject[] fruitsWithTag = GameObject.FindGameObjectsWithTag(tag); // Encuentra todos los objetos con el tag específico
            foreach (GameObject fruit in fruitsWithTag)
            {
                if (fruit != null)
                {
                    Destroy(fruit); // Destruye cada objeto de fruta encontrado con el tag específico
                }
            }
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

        return newArray;
    }


    public void VolverEspaña()
    {
        // Cargar la nueva escena
        SceneManager.LoadScene(2);
    }

}
