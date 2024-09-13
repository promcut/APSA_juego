using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController3_7 : MonoBehaviour
{

    [SerializeField] private GameObject[] estrellitas;
    [SerializeField] private GameObject[] gameObjects;
    [SerializeField] private GameObject baseTransform;
    private GameObject mainObject;
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

    public void CheckPoints()
    {
        Debug.Log("points: " + currentPoints);
  
        if (currentPoints == pointsToWin)
        {
            avioneta.GetComponent<avionetaMovement>().GanarBajoDer();
            Invoke("AvionetaAnimation", 3f);
            Debug.Log("¡Has ganado!");
            Invoke("VolverNorte", 7f);
        }
        else
        {
                Invoke("DestroyMain", 1f);
                Invoke("SpawnObject", 3f);   
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
