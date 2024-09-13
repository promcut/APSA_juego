using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController3_6 : MonoBehaviour
{

    [SerializeField] private GameObject[] estrellitas;
    [SerializeField] private GameObject[] gameObjects;
    [SerializeField] private GameObject baseTransform;
    private GameObject mainObject;
    [SerializeField] private GameObject baseFondo;
    public Sprite base_FondoSprite;
    public int pointsToWin = 10;      // Puntos necesarios para ganar
    private int currentPoints = 0;    // Puntos actuales del jugador

    [SerializeField] private AudioClip winLevel;
    [SerializeField] private AudioClip wrong;
    [SerializeField] private GameObject avioneta;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ShuffleGameObjects();
        SpawnObject();
    }

    void SpawnObject()
    {
        mainObject = Instantiate(gameObjects[currentPoints], baseTransform.transform);
    }

    public void Fallo()
    {
        Debug.Log("Fallaste");
        audioSource.PlayOneShot(wrong);
        avioneta.GetComponent<avionetaMovement>().Fallo();
    }

    public void SumarPuntos()
    {
        estrellitas[currentPoints].GetComponent<nube>().brillar(); //Llamamos al método brillar que hace la animación de la nube al pasar de nivel
        currentPoints++;
        CheckPoints();
    }

    public void CheckObjet(GameObject casilla)
    {
        BlockCasillas();
        if(mainObject.GetComponent<Forma_n6>().position == casilla.GetComponent<Casilla>().position)
        {
            SumarPuntos();
            StartCoroutine(casilla.GetComponent<Casilla>().ChangeSpriteCorrect());
            casilla.GetComponent<Casilla>().SetCanTouch(false);
            Invoke("AllowCasillas", 2f);
        }else{
            Fallo();
            StartCoroutine(casilla.GetComponent<Casilla>().ChangeSpriteWrong());
            AllowCasillas();
        }
        
    }

    private void CheckPoints()
    {
        Debug.Log("points: " + currentPoints);
        if (currentPoints == pointsToWin)
        {
            avioneta.GetComponent<avionetaMovement>().GanarDer();
            Invoke("ChangeBaseFondoSprite", 1f);
            Invoke("AvionetaAnimation", 4f);
            Debug.Log("¡Has ganado!");
            Invoke("VolverNorte", 7f);
        }
        else
        {
                Invoke("DestroyMain", 1f);
                Invoke("SpawnObject", 2f);   
        }
    }

    public void AvionetaAnimation()
    {
        audioSource.PlayOneShot(winLevel);
    }

    public void DestroyMain()
    {
        Destroy(mainObject);
    }

    public void BlockCasillas()
    {
        foreach (GameObject casilla in GameObject.FindGameObjectsWithTag("Respawn"))
        {
            casilla.GetComponent<Casilla>().BlockRaycast();
        }
    }

    public void AllowCasillas()
    {
        foreach (GameObject casilla in GameObject.FindGameObjectsWithTag("Respawn"))
        {
            if(casilla.GetComponent<Casilla>().GetCanTouch())
            {
                casilla.GetComponent<Casilla>().RestartRaycast();
            } 
        }
    }

    public void ChangeBaseFondoSprite()
    {
        Image imageComponent = baseFondo.GetComponent<Image>();
        if (imageComponent != null)
        {
            imageComponent.DOFade(0, 0.5f).OnComplete(() =>
            {
                imageComponent.sprite = base_FondoSprite;
                imageComponent.DOFade(1, 0.5f);
            });
        }
    }

    private void ShuffleGameObjects()
    {
        // Crear una lista a partir del array de gameObjects
        List<GameObject> list = gameObjects.ToList();
        // Usar Fisher-Yates shuffle para barajar la lista
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            GameObject temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        // Convertir la lista barajada de nuevo a un array
        gameObjects = list.ToArray();
    }

    public void VolverNorte()
    {
        SceneManager.LoadScene(19);
    }
}
