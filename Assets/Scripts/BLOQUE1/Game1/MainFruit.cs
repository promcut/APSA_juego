using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFruit : MonoBehaviour
{

    [SerializeField] private SceneController controller;

    void Start()
    {
        Debug.Log("Nombre del objeto: " + gameObject.name);
        controller = GameObject.FindObjectOfType<SceneController>(); // Buscar y asignar dinámicamente el SceneController en la escena
        if (controller != null && controller.GetType() == typeof(SceneController))
        {
            Debug.Log("El controller es del tipo SceneController");
        }
        else
        {
            Debug.LogError("El controller no es del tipo SceneController o es nulo.");
        }
    }

    void Update()
    {
        // Verificar si se ha tocado la pantalla
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Debug.Log("Has tocado la pantalla");
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                if (controller != null)
                {
                    //Debug.Log("Has tocado una fruta");
                    controller.CheckFruit(this); // Llamar al método CheckFruit del SceneController y pasar esta fruta como parámetro
                    //Debug.Log("Volvi");
                }
                else
                {
                    Debug.LogError("No se pudo acceder al SceneController.");
                }
            }
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Has tocado la pantalla");
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                if (controller != null)
                {
                    Debug.Log("Has tocado una fruta");
                    controller.CheckFruit(this); // Llamar al método CheckFruit del SceneController y pasar esta fruta como parámetro
                }
                else
                {
                    Debug.LogError("No se pudo acceder al SceneController.");
                }
            }
        }
#endif
    }

    public static implicit operator MainFruit(GameObject v)
    {
        throw new NotImplementedException();
    }

    public void SetActive(bool v)
    {
        Debug.Log("Desactivando objeto");
        gameObject.SetActive(v);
    }
}
