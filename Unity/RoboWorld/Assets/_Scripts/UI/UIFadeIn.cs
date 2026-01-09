using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Nécessaire pour CanvasGroup

// Ce script a besoin d'un CanvasGroup pour marcher
[RequireComponent(typeof(CanvasGroup))]
public class UIFadeIn : MonoBehaviour
{
    public float dureeFade = 0.4f;
    private CanvasGroup cg;

    void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }

    // Se lance quand on fait SetActive(true) sur le panel
    void OnEnable()
    {
        cg.alpha = 0f; // On commence invisible
        StopAllCoroutines();
        StartCoroutine(FadeInRoutine());
    }

    IEnumerator FadeInRoutine()
    {
        float timer = 0f;
        while (timer < dureeFade)
        {
            // Ici on utilise deltaTime normal si c'est le menu principal, 
            // ou unscaled si c'est un menu de pause. 
            // Dans le doute, unscaled marche partout pour l'UI.
            timer += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, timer / dureeFade);
            yield return null;
        }
        cg.alpha = 1f; // Sûr d'être opaque à la fin
    }
}