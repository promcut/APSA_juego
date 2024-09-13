using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class GameRound3_4_scenne1
{
    public Sprite elementSprites;
    public Vector3 scale;
}

public class SceneController3_4_scene1 : MonoBehaviour
{
    private GameObject mainElement;
    [SerializeField] private GameObject elementPrefab; // Prefab para los elementos
    public Transform mainBase; // Punto de generación a la derecha
    public Transform contenedor_elements;

    public Canvas canvas; // Referencia al canvas
    [SerializeField] private GameObject[] estrellitas;

    public int pointsToChangeScene = 4; // Puntos necesarios para ganar
    private int currentPoints = 0; // Puntos actuales del jugador

    [SerializeField] private AudioClip winLevel;
    [SerializeField] private AudioClip wrong;
    [SerializeField] public AudioClip instruction;
    [SerializeField] private GameObject avioneta;
    private AudioSource audioSource;
    public List<GameRound3_4> rounds;
    private int currentRoundIndex = 0;
    private int num_mains = 0;
    private int currentMains = 0;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ShuffleRounds();
        StartNextRound();
        Invoke("PlayInstruct", 0.2f);
    }

    void StartNextRound()
    {
        GenerateMainElement();
        GenerateElements();
    }

    void ShuffleRounds()
    {
        for (int i = 0; i < rounds.Count; i++)
        {
            int randomIndex = Random.Range(i, rounds.Count);
            GameRound3_4 temp = rounds[i];
            rounds[i] = rounds[randomIndex];
            rounds[randomIndex] = temp;
        }
    }

    void GenerateElements()
    {
        currentMains = 0;
        num_mains = 0;
        int[] rotations = { 0, 90, 180 };
        int mainRotation = mainElement.GetComponent<Flip_Element>().direct;

        int sameRotationIndex = Random.Range(0, 4); //aleatorio qn tendrá la rot del main

        for (int i = 0; i < 4; i++)
        {
            GameObject element = Instantiate(elementPrefab, contenedor_elements);
            GameRound3_4 currentRound = rounds[currentRoundIndex];
            Sprite mainSprite = currentRound.elementSprites;
            Vector3 mainScale = currentRound.scale;
            element.GetComponent<Flip_Element>().SetElement(mainSprite, mainScale);
            int rotation;
            if (i == sameRotationIndex)
            {
                rotation = mainRotation;
            }
            else
            {
                int randomRotationIndex;
                do
                {
                    randomRotationIndex = Random.Range(0, rotations.Length);
                } while (rotations[randomRotationIndex] == mainRotation); //si es la del main cambia, para que solo uno tenga la rot del main

                rotation = rotations[randomRotationIndex];
            }

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
            currentMains++;
            if (num_mains == currentMains)
            {
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
        if (currentPoints == pointsToChangeScene)
        {
            Invoke("ChangeScene", 2f);
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

    public void ChangeScene()
    {
        SceneManager.LoadScene(27);
    }

    public void PlayInstruct()
    {
        audioSource.PlayOneShot(instruction);
    }

    public void VolverNorte()
    {
        SceneManager.LoadScene(19);
    }
}
