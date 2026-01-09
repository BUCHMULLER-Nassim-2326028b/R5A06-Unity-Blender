using UnityEngine;
using System.Collections; // Nécessaire pour les Coroutines

public class UIPopInAnim : MonoBehaviour
{
    [Header("Réglages")]
    public float hauteurDepart = 300f; // De combien de pixels il part d'en haut
    public float duree = 0.6f; // Durée en secondes
    // La courbe magique pour l'effet fluide (rebond ou freinage)
    public AnimationCurve courbeFluidite = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1)); 

    private RectTransform rectTransform;
    private Vector2 positionFinale;

    void Awake()
    {
        // On mémorise où le texte doit finir (sa position actuelle dans l'éditeur)
        rectTransform = GetComponent<RectTransform>();
        positionFinale = rectTransform.anchoredPosition;
    }

    // Cette fonction se lance AUTOMATIQUEMENT à chaque fois que le GameObject est activé (SetActive true)
    void OnEnable()
    {
        // 1. On le téléporte en haut avant de commencer
        rectTransform.anchoredPosition = positionFinale + new Vector2(0, hauteurDepart);
        
        // 2. On lance l'animation
        StopAllCoroutines(); // Sécurité
        StartCoroutine(AnimerEntree());
    }

    IEnumerator AnimerEntree()
    {
        float timer = 0f;
        Vector2 positionDepart = rectTransform.anchoredPosition;

        while (timer < duree)
        {
            // IMPORTANT : unscaledDeltaTime car le jeu est en pause !
            timer += Time.unscaledDeltaTime; 
            
            // On calcule le pourcentage d'avancement (entre 0 et 1)
            float pourcentage = timer / duree;
            // On applique la courbe pour que ce ne soit pas linéaire (effet "smooth")
            float pourcentageCourbe = courbeFluidite.Evaluate(pourcentage);

            // Lerp fait le déplacement progressif
            rectTransform.anchoredPosition = Vector2.Lerp(positionDepart, positionFinale, pourcentageCourbe);

            yield return null; // On attend la frame suivante
        }

        // Pour être sûr qu'à la fin il soit pile au bon endroit
        rectTransform.anchoredPosition = positionFinale;
    }
}