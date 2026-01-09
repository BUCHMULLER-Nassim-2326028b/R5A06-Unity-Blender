using UnityEngine;
using UnityEngine.EventSystems; // Indispensable pour détecter la souris sur l'UI

public class UIHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Réglages")]
    public float distanceDeplacement = 30f; // De combien de pixels il bouge vers la droite
    public float vitesse = 15f; // Vitesse de l'animation

    private Vector3 posDepart;
    private Vector3 posCible;
    private RectTransform rectTransform;
    private bool isHovered = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        posDepart = rectTransform.anchoredPosition; // On mémorise la place initiale
    }

    // Quand la souris ENTRE
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        StopAllCoroutines(); // On coupe l'anim de retour si elle était en cours
        StartCoroutine(AnimerRebond(true));
    }

    // Quand la souris SORT
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        StopAllCoroutines();
        StartCoroutine(AnimerRebond(false));
    }

    // La petite magie du rebond (Coroutine)
    System.Collections.IEnumerator AnimerRebond(bool entrer)
    {
        float timer = 0f;
        
        // Si on entre, on vise la droite. Si on sort, on vise le départ.
        Vector3 target = entrer ? posDepart + (Vector3.right * distanceDeplacement) : posDepart;
        
        // --- ETAPE 1 : L'OVERSHOOT (Aller un peu trop loin) ---
        // On ne le fait que si on entre (pour l'effet rebond)
        if (entrer)
        {
            Vector3 targetOvershoot = posDepart + (Vector3.right * (distanceDeplacement * 1.2f)); // 20% plus loin
            
            while (Vector3.Distance(rectTransform.anchoredPosition, targetOvershoot) > 0.5f)
            {
                rectTransform.anchoredPosition = Vector3.Lerp(rectTransform.anchoredPosition, targetOvershoot, Time.unscaledDeltaTime * vitesse);
                yield return null;
            }
        }

        // --- ETAPE 2 : STABILISATION (Aller à la position finale) ---
        while (Vector3.Distance(rectTransform.anchoredPosition, target) > 0.5f)
        {
            rectTransform.anchoredPosition = Vector3.Lerp(rectTransform.anchoredPosition, target, Time.unscaledDeltaTime * vitesse);
            yield return null;
        }

        // Pour être sûr qu'on est pile au bon endroit à la fin
        rectTransform.anchoredPosition = target;
    }
    
    // Sécurité : Si on désactive le bouton pendant qu'il est décalé, on le remet en place
    void OnDisable()
    {
        if(rectTransform != null) rectTransform.anchoredPosition = posDepart;
    }
}