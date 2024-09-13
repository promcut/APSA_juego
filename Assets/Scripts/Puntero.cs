using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Puntero : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Parpadear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Parpadear()
    {
        transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 1, 0, 1F).SetLoops(-1);
    }
}
