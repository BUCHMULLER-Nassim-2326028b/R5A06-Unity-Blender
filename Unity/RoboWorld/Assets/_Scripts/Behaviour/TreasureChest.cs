using UnityEngine;
using System.Collections; // Pour les animations manuelles (Coroutines)

public class TreasureChest : MonoBehaviour
{
    [Header("État")]
    public bool estVerrouille = true;
    private bool aLaCle = false;
    private bool estOuvert = false;
    private bool joueurProche = false;

    [Header("Récompense")]
    public GameObject objetRecompensePrefab; // Ce qui va sortir (ex: une méga pièce)
    public Transform pointDApparition; // Un point vide au centre du coffre
    
    [Header("Animations")]
    public Animator animatorCoffre;
    // public string triggerVanish = "Vanish";
    public GameObject vanishEffectPrefab; // Effet de particules (ex: PlayerVanish)
    public AudioClip sonOuverture;
    public AudioClip sonRecompense;

    private Transform playerTransform;
    private Collider myCollider;
    private Renderer[] myRenderers;

    private void Start()
    {
        myCollider = GetComponent<Collider>();
        myRenderers = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        // Si le joueur est devant, qu'il a la clé, qu'il appuie sur E et que c'est pas déjà ouvert
        if (joueurProche && !estOuvert && Input.GetKeyDown(KeyCode.E))
        {
            if (aLaCle || !estVerrouille)
            {
                OuvrirLeCoffre();
            }
            else
            {
                Debug.Log("Il te faut la clé !");
            }
        }
    }

    // Appelée par le script de la clé
    public void AvoirLaCle()
    {
        aLaCle = true;
    }

    void OuvrirLeCoffre()
    {
        estOuvert = true;
        
        if (GameManager.instance != null)
        {
            GameManager.instance.AfficherInteractionCoffre(false);
        }

        Debug.Log("Coffre Ouvert !");

        // 1. Animation du coffre
        if (animatorCoffre != null) animatorCoffre.SetTrigger("Open");
        if (sonOuverture != null) AudioSource.PlayClipAtPoint(sonOuverture, transform.position);

        // 2. Faire pop la récompense après un petit délai (le temps que le couvercle s'ouvre)
        StartCoroutine(SpawnRewardSequence());
    }

    IEnumerator SpawnRewardSequence()
    {
        yield return new WaitForSeconds(0.5f); // Attends que le coffre s'ouvre un peu

        if (objetRecompensePrefab != null && pointDApparition != null)
        {
            // On crée l'objet
            GameObject recompense = Instantiate(objetRecompensePrefab, pointDApparition.position, Quaternion.identity);
            
            // On désactive la collision du coffre pour éviter les bugs physiques
            if (myCollider != null) myCollider.enabled = false;

            // On désactive le visuel du coffre (Disparition)
            if (myRenderers != null)
            {
                foreach(Renderer r in myRenderers) r.enabled = false;
            }

            // On joue les particules de disparition
            if (vanishEffectPrefab != null)
            {
               Instantiate(vanishEffectPrefab, transform.position, Quaternion.identity);
            }

            // Son de victoire
            if (sonRecompense != null) AudioSource.PlayClipAtPoint(sonRecompense, transform.position);

            // --- ANIMATION DE LA RÉCOMPENSE ---
            float timer = 0f;
            Vector3 startPos = recompense.transform.position;
            // On calcule une position finale : PLUS haut et PLUS devant
            // Note: Si l'objet part sur le côté, vérifiez la rotation du coffre ou utilisez transform.right
            Vector3 finalPos = startPos + Vector3.up * 2.5f + transform.forward * 2.5f;

            // Animation : Sortir du coffre vers le devant
            while (timer < 1f)
            {
                recompense.transform.position = Vector3.Lerp(startPos, finalPos, timer);
                recompense.transform.Rotate(Vector3.up * 180 * Time.deltaTime); // Rotation décorative
                timer += Time.deltaTime * 2f;
                yield return null;
            }
            
            // On s'assure qu'elle est bien à la position finale
            recompense.transform.position = finalPos;
            
            // Optionnel : Détruire le coffre après l'animation ?
            // Destroy(gameObject, 1f); // Attention cela arrêterait le script si on le faisait trop tôt
        }
    }

    // Détection du joueur (Zone d'interaction)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            joueurProche = true;
            playerTransform = other.transform;
            
            if (GameManager.instance != null)
            {
                if (estOuvert)
                {
                    // Déjà ouvert, rien à dire
                }
                else if (aLaCle || !estVerrouille)
                {
                    GameManager.instance.AfficherInteractionCoffre(true, "Appuyez sur E pour ouvrir");
                }
                else
                {
                    GameManager.instance.AfficherInteractionCoffre(true, "Verrouillé (nécessite une clé)");
                }
            }
            Debug.Log("Appuie sur E pour ouvrir");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            joueurProche = false;
            
            if (GameManager.instance != null)
            {
                GameManager.instance.AfficherInteractionCoffre(false);
            }
        }
    }
}