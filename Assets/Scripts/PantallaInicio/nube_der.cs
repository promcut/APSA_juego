using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class nube_der : MonoBehaviour
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
                transform.DOMoveX(transform.position.x + distancia, duracion)
                    .SetEase(Ease.Linear); // Utilizar Ease.Linear para un movimiento constante
            });*/

            DOTween.Sequence()
            .AppendInterval(2f)//Espera dos segundos antes de moverse
            .AppendCallback(() =>
            {
                // Mover el objeto hacia la izquierda
                _rectTransform.DOAnchorPos(new Vector2(812,-119), duracion)
                    .SetEase(Ease.Linear); // Utilizar Ease.Linear para un movimiento constante
            });
    }
}
