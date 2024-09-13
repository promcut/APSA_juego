using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController3_2 : MonoBehaviour
{

    [SerializeField] private GameObject[] estrellitas;
    [SerializeField] private GameObject[] gameObjects;
    [SerializeField] private GameObject baseTransform;
    private GameObject mainObject;

    public int pointsToWin = 10;      // Puntos necesarios para ganar
    private int currentPoints = 0;    // Puntos actuales del jugador
    private int currentPointsScene = 0;
    public int numScene;

    [SerializeField] private AudioClip winLevel;
    [SerializeField] private AudioClip wrong;
    [SerializeField] public AudioClip instruction;
    [SerializeField] private GameObject avioneta;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (numScene == 1)
        {
            currentPointsScene = 3;
        }else if (numScene == 0)
        {
            Invoke("PlayInstruct", 0.2f);
        }
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
        estrellitas[currentPointsScene].GetComponent<nube>().brillar(); //Llamamos al método brillar que hace la animación de la nube al pasar de nivel
        currentPointsScene++;
        currentPoints++;
        CheckPoints();
    }

    private void CheckPoints()
    {
        Debug.Log("points: " + currentPointsScene);
        if (currentPointsScene == pointsToWin)
        {
            FindObjectOfType<estrellitas>().Brillo_Estrellita();
            //avioneta.GetComponent<avionetaMovement>().GanarDer();
            Invoke("AvionetaAnimation", 2f);
            Debug.Log("¡Has ganado!");
            Invoke("VolverNorte", 4f);
        }
        else
        {

            if (currentPointsScene == 3)
            {
                //cambiamos de escena a la siguiente
                Invoke("CambioEscenaSiguiente", 6f);
            }
            if (currentPointsScene <= 3)
            {
                Invoke("DestroyMain", 6f);
                Invoke("SpawnObject", 7f);
            }
            else
            {
                Invoke("DestroyMain", 2f);
                Invoke("SpawnObject", 3f);
            }

        }
    }

    private void CambioEscenaSiguiente()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void AvionetaAnimation()
    {
        audioSource.PlayOneShot(winLevel);
    }

    public void DestroyMain()
    {
        Destroy(mainObject);
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
