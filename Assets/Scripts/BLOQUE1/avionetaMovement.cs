using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class avionetaMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Ganar(){
                // Movimiento circular durante dos segundos
        DOTween.Sequence()
            .Join(transform.DORotate(new Vector3(0, 180, 360), 1f, RotateMode.FastBeyond360).SetEase(Ease.InOutSine)) // Girar en sentido horario
            .AppendInterval(0.5f) // Esperar un segundo
            .Append(transform.DOMoveX(transform.position.x - 30f, 2f).SetEase(Ease.InOutSine)) // Mover 300f a la derecha
            .Join(transform.DOMoveY(transform.position.y - 30f, 2f).SetEase(Ease.InOutSine)); // Mover 200f hacia abajo
    }
    public void GanarDer(){
        DOTween.Sequence()
            .Join(transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360).SetEase(Ease.InOutSine)) // Girar en sentido horario
            .AppendInterval(0.5f) // Esperar un segundo
            .Append(transform.DOMoveX(transform.position.x + 30f, 2f).SetEase(Ease.InOutSine)) // Mover 300f a la derecha
            .Join(transform.DOMoveY(transform.position.y  - 30f, 2f).SetEase(Ease.InOutSine)); // Mover 200f hacia abajo
    }

    public void GanarBajoDer()
    {
        // Movimiento circular durante dos segundos
        DOTween.Sequence()
            .Join(transform.DORotate(new Vector3(0, 0, -30), 1f, RotateMode.FastBeyond360).SetEase(Ease.InOutSine)) // Girar en sentido horario
            .AppendInterval(0.5f) // Esperar un segundo
            .Append(transform.DOMoveX(transform.position.x - 40f, 2f).SetEase(Ease.InOutSine)) // Mover 30f a la izquierda
            .Join(transform.DOMoveY(transform.position.y + 30f, 2f).SetEase(Ease.InOutSine)); // Mover 30f hacia abajo
    }
    public void Fallo(){
            Debug.Log("poniendo fallo a true");
            animator.SetBool("Fallo",true);
            StartCoroutine(BackIdle());
    }
    IEnumerator BackIdle(){
        yield return new WaitForSeconds(1);
        Debug.Log("poniendo fallo a false");
        animator.SetBool("Fallo",false);
    }
}

