using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class nube_frente : MonoBehaviour
{
    public float distancia = 1200f; // Distancia a la que se moverá la nube
    public float duracion = 2f; // Duración del movimiento
    private RectTransform _rectTransform;
    
    private void Awake() {
        _rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        /*DOTween.Sequence()
            .AppendInterval(2f)//Espera dos segundos antes de moverse
            .AppendCallback(() =>
            {
                // Mover el objeto hacia la izquierda
                transform.DOMoveY(transform.position.y - distancia, duracion)
                    .SetEase(Ease.Linear); // Utilizar Ease.Linear para un movimiento constante
            });*/
        
        DOTween.Sequence()
            .AppendInterval(2f)//Espera dos segundos antes de moverse
            .AppendCallback(() =>
            {
                // Mover el objeto hacia la izquierda
                _rectTransform.DOAnchorPos(new Vector2(192,-403), duracion)
                    .SetEase(Ease.Linear); // Utilizar Ease.Linear para un movimiento constante
            });
    }
}
