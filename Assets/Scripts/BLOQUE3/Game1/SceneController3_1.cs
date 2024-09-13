using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController3_1 : MonoBehaviour
{
    public GameObject flecha_arriba;  // Prefab de la flecha arriba
    public GameObject flecha_abajo;   // Prefab de la flecha abajo
    public GameObject flecha_izquierda; // Prefab de la flecha izquierda
    public GameObject flecha_derecha; // Prefab de la flecha derecha
    public Transform spawnPoint;      // Punto de generación
    public Canvas canvas;             // Referencia al canvas
    [SerializeField] private GameObject[] estrellitas;

    public int pointsToWin = 10;      // Puntos necesarios para ganar
    private int currentPoints = 0;    // Puntos actuales del jugador

    [SerializeField] private AudioClip winLevel;
    [SerializeField] private AudioClip wrong;
    [SerializeField] public AudioClip instruction;
    [SerializeField] private GameObject avioneta;
    private AudioSource audioSource;

    public int num_scene = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Llamar a la función para generar objetos aleatorios cada segundo
        InvokeRepeating("SpawnRandomObject", 1f, 3f);
        if(num_scene == 1)
        {
                Invoke("PlayInstruct", 0.2f);
        }
    }

    void SpawnRandomObject()
    {
        // Determinar el tipo de objeto a instanciar
        int objectType;

        if (num_scene == 1)
        {
            // Solo flecha arriba y flecha abajo
            objectType = Random.Range(0, 2);
        }
        else if (num_scene == 2)
        {
            // Solo flecha izquierda y flecha derecha
            objectType = Random.Range(2, 4);
        }
        else if (num_scene == 3)
        {
            // Incluir los cuatro tipos de flechas
            objectType = Random.Range(0, 4);
        }
        else
        {
            return; // No instanciar nada si num_scene no es 1, 2 o 3
        }

        // Instanciar el objeto correspondiente en el punto de spawn
        GameObject spawnedObject = null;

        switch (objectType)
        {
            case 0:
                spawnedObject = Instantiate(flecha_arriba, spawnPoint.position, Quaternion.identity);
                break;
            case 1:
                spawnedObject = Instantiate(flecha_abajo, spawnPoint.position, Quaternion.identity);
                break;
            case 2:
                spawnedObject = Instantiate(flecha_izquierda, spawnPoint.position, Quaternion.identity);
                break;
            case 3:
                spawnedObject = Instantiate(flecha_derecha, spawnPoint.position, Quaternion.identity);
                break;
        }

        // Asignar el canvas como padre del objeto instanciado
        if (spawnedObject != null)
        {
            spawnedObject.transform.SetParent(spawnPoint.transform, false);
            spawnedObject.transform.position = spawnPoint.position;
        }
    }

    public void CheckObject(GameObject droppedObject, GameObject dropZone)
    {
        if (droppedObject.tag == dropZone.tag)
        {
            Destroy(droppedObject);
            Debug.Log("Destruyendo objeto acertado");
            SumarPuntos();
        }
        else
        {
            Fallo();
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
        estrellitas[currentPoints].GetComponent<nube>().brillar(); //Llamamos al método brillar que hace la animación de la nube al pasar de nivel
        currentPoints++;
        CheckPoints();
    }

    private void CheckPoints()
    {
        if (currentPoints == pointsToWin)
        {
            DesaparecerFlechas();
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            Debug.Log(num_scene);
            if (num_scene < 3)
            {
                SceneManager.LoadScene(currentSceneIndex + 1);
                num_scene++;
            }
            else
            {
                avioneta.GetComponent<avionetaMovement>().GanarDer();
                audioSource.PlayOneShot(winLevel);
                Debug.Log("¡Has ganado!");
                Invoke("VolverNorte", 3f);
            }
        }
    }

    public void DesaparecerFlechas()
    {
        CancelInvoke("SpawnRandomObject");
        foreach (Transform child in spawnPoint)
        {
            Flecha flechaComponent = child.GetComponent<Flecha>();
            if (flechaComponent != null)
            {
                flechaComponent.Desaparecer();
            }
        }
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
