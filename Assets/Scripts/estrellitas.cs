using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class estrellitas : MonoBehaviour
{
    // Start is called before the first frame update
    private Image image;
    private Animator animator;

    void Awake()
    {
        image = GetComponent<Image>();
        animator = GetComponent<Animator>();
        transform.localScale = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Brillo_Estrellita(){
        transform.localScale = new Vector3(1,1,1);
        animator.SetTrigger("Win");
    }

    public void Desaparecer_Estrellita(){
        transform.localScale = new Vector3(0,0,0);
    }
}
