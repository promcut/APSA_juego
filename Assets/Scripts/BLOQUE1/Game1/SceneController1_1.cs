using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController1_1 : MonoBehaviour
{

    private MainFruit2 originalFruit;
    [SerializeField] private GameObject[] fruits;
    [SerializeField] private GameObject mainElementBaseUI; // Objeto cuadrado
    [SerializeField] private GameObject[] elementsBaseUI;
    [SerializeField] private GameObject[] nubes;
    //[SerializeField] private GameObject nubeColorida;
    AudioSource audioSource;
    [SerializeField] public AudioClip win;
    [SerializeField] public AudioClip wrong;
    [SerializeField] public AudioClip instruct;
    [SerializeField] private GameObject avioneta;
    [SerializeField] private Sprite correct_base;
    private Sprite originalSprite_base;
    private int[] fruitnumb;
    private int currentPoints = 0;
    private int finalPoints;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); //recuperamos el AudioSource de la nube
    }

    void Start()
    {
        finalPoints = fruits.Length;
        Debug.Log("num frutas: " + finalPoints);

        fruitnumb = new int[fruits.Length];
        for (int i = 0; i < fruits.Length; i++)
        {
            fruitnumb[i] = i;
        }
        GenerateMainFruit(0);
        Debug.Log("main generado");
        GenerateInitialFruits(); //Generamos las primeras frutas
        Invoke("PlayInstruct", 0.2f);
    }
    public void GenerateMainFruit(int i)
    {
        //PARA GENERAR LA MAINFRUIT
        originalFruit = fruits[i].GetComponent<MainFruit2>(); //la obtenemos como fruta a adivinar
        Debug.Log("La fruta obtenida es: " + fruits[i].tag);

        //scaleFruit = originalFruit.GetComponent<RectTransform>().localScale; //guardamos la escala orginal para luego
        //originalFruit.GetComponent<RectTransform>().localScale = scaleFruit * 1.5f; //aumentamos la escala

        originalFruit = Instantiate(originalFruit, mainElementBaseUI.transform);
        originalFruit.SetRaycastTarget(false);
    }
    public void GenerateInitialFruits()
    {
        //elementsBaseUI.GenerateInitialFruits();
        int[] shuffledIndexes = ShuffleArray(fruitnumb);
        for (int i = 0; i < fruits.Length; i++)
        {
            int index = shuffledIndexes[i];
            Debug.Log("index: "+index);
            Instantiate(fruits[index], elementsBaseUI[i].transform);
            Debug.Log("creado "+ index);
            
        }
    }

    public void DestroyAllFruits()
    {
        string[] fruitTags = { "combi1", "combi2", "combi3", "combi4", "combi5", "combi6", "combi7", "combi8", "combi9"}; //tags de las frutas
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
        public void DesactivarColliderElementos()
    {
        string[] fruitTags = { "combi1", "combi2", "combi3", "combi4", "combi5", "combi6", "combi7", "combi8", "combi9"}; //tags de las frutas
        foreach (string tag in fruitTags)
        {
            GameObject[] fruitsWithTag = GameObject.FindGameObjectsWithTag(tag); // Encuentra todos los objetos con el tag específico
            foreach (GameObject fruit in fruitsWithTag)
            {
                if (fruit != null)
                {
                    fruit.GetComponent<MainFruit2>().SetRaycastTarget(false);
                }
            }
        }
    }
    
    public void ActivarElementos()
    {
        string[] fruitTags = { "combi1", "combi2", "combi3", "combi4", "combi5", "combi6", "combi7", "combi8", "combi9"}; //tags de las frutas
        foreach (string tag in fruitTags)
        {
            GameObject[] fruitsWithTag = GameObject.FindGameObjectsWithTag(tag); // Encuentra todos los objetos con el tag específico
            foreach (GameObject fruit in fruitsWithTag)
            {
                if (fruit != null)
                {
                    fruit.GetComponent<MainFruit2>().SetRaycastTarget(true);
                }
            }
        }
    }

    public void RegenerateFruits()
    {
        GenerateMainFruit(currentPoints);
        GenerateInitialFruits();
    }


    //Barajar números para que aparezcan de manera aleatoria
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
            Debug.Log(tmp);
        }
        return newArray;
    }

    public void CheckFruit(MainFruit2 fruitselected)
    {
        Debug.Log("Vamos a ver si adivinas la fruta");
        if (fruitselected.tag == originalFruit.tag)
        {
            DesactivarColliderElementos();
            SumarPuntos();
            StartCoroutine(AnimationBaseAcierto());
            Debug.Log("Adivinaste");
            if (currentPoints == fruits.Length)
            {
                Debug.Log("TERMINASTE");
                avioneta.GetComponent<avionetaMovement>().GanarDer();
                audioSource.PlayOneShot(win);
                Invoke("VolverEspaña", 3f);
            }
            else
            {
                Invoke("DestroyAllFruits", 1.5f);
                Invoke("ReturnBaseSprite", 1.5f);
                Invoke("RegenerateFruits", 2f);
                Invoke("ActivarColliderElementos", 2f);
            }
        }
        else
        {
            Debug.Log("Inténtalo de nuevo");
            audioSource.PlayOneShot(wrong);
            avioneta.GetComponent<avionetaMovement>().Fallo();
        }
    }

    public void SumarPuntos()
    {
        //nubes[currentPoints].SetActive(false);
        nubes[currentPoints].GetComponent<nube>().brillar(); //Llamamos al método brillar que hace la animación de la nube al pasar de nivel
        //Instantiate(nubeColorida, nubes[currentPoints].transform.position, Quaternion.identity);
        currentPoints++;
    }

    IEnumerator AnimationBaseAcierto()
    {
        // Obtener el Image y RectTransform del mainElementBaseUI
        Image image = mainElementBaseUI.GetComponent<Image>();
        RectTransform rectTransform = mainElementBaseUI.GetComponent<RectTransform>();

        Vector3 originalScale = rectTransform.localScale;
        originalSprite_base = image.sprite;
        yield return rectTransform.DOScale(originalScale * 1.2f, 0.5f).SetEase(Ease.OutQuad).WaitForCompletion();
        image.sprite = correct_base;
        yield return rectTransform.DOScale(originalScale, 0.5f).SetEase(Ease.OutQuad).WaitForCompletion();
    }

    public void ReturnBaseSprite()
    {
        Image image = mainElementBaseUI.GetComponent<Image>();
        image.sprite = originalSprite_base;
    }

    public void PlayInstruct()
    {
        audioSource.PlayOneShot(instruct);
    }


    public void VolverEspaña()
    {
        // Cargar la nueva escena
        SceneManager.LoadScene(2);
    }

}
