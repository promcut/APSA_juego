using UnityEngine;
using DG.Tweening;

public class avioneta : MonoBehaviour
{

    private RectTransform _rectTransform;
    private void Awake() {
        _rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        // Movimiento circular durante dos segundos
        DOTween.Sequence()
            .Append(_rectTransform.DOMove(_rectTransform.position + Vector3.up * 2f, 1f).SetEase(Ease.InOutSine)) // Mover hacia arriba
            .Join(_rectTransform.DORotate(new Vector3(0, 0, 360), 2f, RotateMode.FastBeyond360).SetEase(Ease.InOutSine)) // Girar en sentido horario
            .AppendInterval(1f) // Esperar un segundo
            .Append(_rectTransform.DOAnchorPos(new Vector2(127,-114), 1.5f).SetEase(Ease.InOutSine));// Mover 300f a la derecha
            //.Append(transform.DOMoveX(transform.position.x + 500f, 1.5f).SetEase(Ease.InOutSine)) // Mover 300f a la derecha
            //.Join(transform.DOMoveY(transform.position.y - 500f, 1.5f).SetEase(Ease.InOutSine)); // Mover 200f hacia abajo
    }
}

