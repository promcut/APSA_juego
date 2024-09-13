using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class nube : MonoBehaviour
{
    private Vector3 escalaOriginal;
    private Vector3 escalaBrillo;
    [SerializeField] public Sprite nubeColorida;
    public AudioClip audioClip;
    AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>(); //recuperamos el AudioSource de la nube
    }

    public void brillar()
    {
        escalaOriginal = transform.localScale;
        escalaBrillo = escalaOriginal * 1.2f;

        audioSource.PlayOneShot(audioClip); //sonidito de subir de nivel

        transform.DOScale(escalaBrillo, 0.3f) // Escalamos la nube para hacerla más grande
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                // Cambiamos el sprite del objeto
                GetComponent<SpriteRenderer>().sprite = nubeColorida;

                        // Finalmente, regresamos a la escala original
                        transform.DOScale(escalaOriginal, 0.7f)
                            .SetEase(Ease.OutBounce); // Utilizamos OutBounce para un efecto de rebote más pronunciado
                    
            });
    }

}
