using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class GameRound3_4
    {
        public Sprite elementSprites;
        public Vector3 scale;
    }
    
public class SceneController3_4 : MonoBehaviour
{
    private GameObject mainElement;
    [SerializeField] private GameObject elementPrefab; // Prefab para los elementos
    public Transform mainBase; // Punto de generación a la derecha
    public Transform contenedor_elements;

    public Canvas canvas; // Referencia al canvas
    [SerializeField] private GameObject[] estrellitas;

    public int pointsToWin = 4; // Puntos necesarios para ganar
    private int currentPoints = 0; // Puntos actuales del jugador

    [SerializeField] private AudioClip winLevel;
    [SerializeField] private AudioClip wrong;
    [SerializeField] private GameObject avioneta;
    private AudioSource audioSource;
    public List<GameRound3_4> rounds;
    private int currentRoundIndex = 0;
    private int num_mains = 0;
    private int currentMains = 0;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartNextRound();
    }

    void StartNextRound()
    {
        GenerateMainElement();
        GenerateElements();
    }

    void GenerateElements()
    {
        currentMains = 0;
        num_mains = 0;
        int[] rotations = { 0, 90, 180 };
        int mainRotation = mainElement.GetComponent<Flip_Element>().direct;
        int sameRotationIndex = Random.Range(0, 6);

        for (int i = 0; i < 6; i++)
        {
            GameObject element = Instantiate(elementPrefab, contenedor_elements);
            GameRound3_4 currentRound = rounds[currentRoundIndex];
            Sprite mainSprite = currentRound.elementSprites;
            Vector3 mainScale = currentRound.scale;
            element.GetComponent<Flip_Element>().SetElement(mainSprite, mainScale);

            int rotation = (i == sameRotationIndex) ? mainRotation : rotations[Random.Range(0, rotations.Length)];
            element.GetComponent<Flip_Element>().Init(false, rotation);
            element.GetComponent<Flip_Element>().SetDir(rotation);
            if (rotation == mainRotation)
            {   
                num_mains++;
            }
        }
    }

    public void GenerateMainElement()
    {
        int[] rotations = { 0, 90, 180 };
        int rotation = rotations[Random.Range(0, rotations.Length)];

        // Selecciona el sprite y la escala de la ronda actual
        GameRound3_4 currentRound = rounds[currentRoundIndex];
        Sprite mainSprite = currentRound.elementSprites;
        Vector3 mainScale = currentRound.scale;

        // Instanciar el mainElement usando el prefab
        mainElement = Instantiate(elementPrefab, mainBase);
        Flip_Element flipElement = mainElement.GetComponent<Flip_Element>();
        flipElement.SetElement(mainSprite, mainScale);
        flipElement.Init(true, rotation);
        flipElement.SetDir(rotation);
    }

    public void CheckObject(GameObject droppedObject)
    {
        if (droppedObject.GetComponent<Flip_Element>().direct == mainElement.GetComponent<Flip_Element>().direct)
        {
            currentMains ++;
            if(num_mains == currentMains){
              SumarPuntos();  
            }
        }
        else
        {
            Fallo();
            droppedObject.GetComponent<Flip_Element>().SetOriginalColor();
        }
    }

    public void Fallo()
    {
        Debug.Log("Fallaste");
        audioSource.PlayOneShot(wrong);
        avioneta.GetComponent<avionetaMovement>().Fallo();
    }

    public void SumarPuntos()
    {
        estrellitas[currentPoints].GetComponent<nube>().brillar(); // Llamamos al método brillar que hace la animación de la nube al pasar de nivel
        currentPoints++;
        CheckPoints();
    }

    private void CheckPoints()
    {
        if (currentPoints == pointsToWin)
        {
            avioneta.GetComponent<avionetaMovement>().GanarDer();
            audioSource.PlayOneShot(winLevel);
            Debug.Log("¡Has ganado!");
            Invoke("VolverNorte", 3f);
        }
        else
        {
            currentRoundIndex++;
            Invoke("DestroyMain", 1f);
            Invoke("DestroyAllElements", 1f);
            Invoke("StartNextRound", 2f);
        }
    }

    public void DestroyAllElements()
    {
        foreach (Transform child in contenedor_elements)
        {
            Destroy(child.gameObject);
        }
    }

    public void DestroyMain()
    {
        Destroy(mainElement);
    }

    public void VolverNorte()
    {
        SceneManager.LoadScene(19);
    }
}
