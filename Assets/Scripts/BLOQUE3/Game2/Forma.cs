using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Forma : MonoBehaviour
{
    public int n_trazos;
    public Sprite correctSprite;
    public Sprite animacionSprite;
    public Sprite complementoSprite;
    private Image image;
    private int temp_trazos = 0;
    private Animator animator;
    private SceneController3_2 sceneController3_2;
    private SceneController3_5 sceneController3_5;
    public int typeForma = 0;
    public Vector3 animationScale;
    public bool isAnimScale = false;

    void Awake()
    {
        image = GetComponent<Image>();
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
        }

        sceneController3_2 = FindObjectOfType<SceneController3_2>();
        sceneController3_5 = FindObjectOfType<SceneController3_5>();
    }

    public void ChangeSprite()
    {
        image.DOFade(0f, 0.5f).OnComplete(() =>
        {
            image.sprite = correctSprite; // Cambiar el sprite
            image.DOFade(1f, 0.5f);
        });
    }

    private void ClearTrazos()
    {
        /*foreach (TrazoUI trazo in GetComponentsInChildren<TrazoUI>())
        {
            trazo.GetComponent<TrailRenderer>().Clear();
        }*/
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void ClearJustTrazos()
    {
        foreach (TrazoUI trazo in GetComponentsInChildren<TrazoUI>())
        {
            trazo.GetComponent<TrailRenderer>().Clear();
        }
    }

    public bool CheckForma()
    {
        temp_trazos++;
        if (temp_trazos == n_trazos)
        {
            if (typeForma == 0)
            { 
                ClearTrazos(); // Limpiar trazos
                ChangeSprite();
                if (animator != null)
                {
                    StartCoroutine(PlayAnimation());
                }
                else
                {
                    StartCoroutine(PlaySpriteAnim());
                }
            }
            else if(typeForma == 1)
            {
                //reproducimos estrellitas
                ClearJustTrazos();
                FindObjectOfType<estrellitas>().Brillo_Estrellita();
            }else if(typeForma == 2)
            {
                StartCoroutine(ChangeSpriteAnimation());
            }
            if(sceneController3_2 != null){
                sceneController3_2.SumarPuntos();
            }else{
                sceneController3_5.SumarPuntos();
            }
            
            return true;
        }
        return false;
    }

    public IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(2f);
        image.DOFade(0f, 0.5f).OnComplete(() =>
        {
            // Cambiar el sprite
            if(isAnimScale)
            {
                transform.localScale = animationScale;
            }
            image.sprite = animacionSprite;
            image.DOFade(1f, 0.5f).OnComplete(() =>
            {
                animator.enabled = true;
            });
        });
        Debug.Log("play animation");
    }

    public IEnumerator ChangeSpriteAnimation()
    {
        yield return new WaitForSeconds(1.5f);
        ClearTrazos();
        image.DOFade(0f, 0.5f).OnComplete(() =>
        {
            // Cambiar el sprite
            if(isAnimScale)
            {
                transform.localScale = animationScale;
            }
            image.sprite = animacionSprite;
            image.DOFade(1f, 0.5f);
        });
        Debug.Log("play animation");
    }

    public IEnumerator PlaySpriteAnim()
    {
        yield return new WaitForSeconds(2f);

        // Fade out the current sprite
        image.DOFade(0f, 0.5f).OnComplete(() =>
        {
            // Change the sprite to animacionSprite
            image.sprite = animacionSprite;
            image.DOFade(1f, 0.5f).OnComplete(() =>
            {
                // Wait for 2 seconds before instantiating the complemento
                StartCoroutine(InstantiateComplementoAfterDelay(0.5f));
            });
        });
    }

    private IEnumerator InstantiateComplementoAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Create a new GameObject
        GameObject complemento = new GameObject("Complemento");

        // Add a RectTransform component
        RectTransform rectTransform = complemento.AddComponent<RectTransform>();
        rectTransform.SetParent(transform, false); // Make it a child of the current object

        // Set the size and position of the RectTransform
        rectTransform.sizeDelta = new Vector2(600, 500);
        rectTransform.anchorMin = new Vector2(0.5f, 1f); // Anchor at the center top
        rectTransform.anchorMax = new Vector2(0.5f, 1f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = new Vector2(0, 0); // Position it just above the parent object

        // Add an Image component and assign the sprite
        Image complementoImage = complemento.AddComponent<Image>();
        complementoImage.sprite = complementoSprite;
        complementoImage.color = new Color(1, 1, 1, 0); // Start with transparent

        complementoImage.DOFade(1f, 1f);
        complemento.transform.localScale = Vector3.one * 0.8f; // Start slightly smaller
        complemento.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce);
    }

}
