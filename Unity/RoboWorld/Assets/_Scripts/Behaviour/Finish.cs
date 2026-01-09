using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Finish : MonoBehaviour
{
    [Header("Cinématique")]
    public float danceDuration = 4f;
    public float rotationSpeed = 30f;

    [Header("Références UI & FX")]
    public GameObject victoryUIPanel;
    public GameObject vanishParticlePrefab;

    private Camera mainCam;
    private CameraMovementOrbital orbitScript;

    void Start()
    {
        // Sécurité : Force tous les colliders en mode Trigger
        foreach(Collider c in GetComponentsInChildren<Collider>())
        {
            c.isTrigger = true;
        }

        mainCam = Camera.main;
        if (mainCam != null) orbitScript = mainCam.GetComponent<CameraMovementOrbital>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. On désactive l'étoile physiquement (plus de collision)
            GetComponent<Collider>().enabled = false;

            // 2. On la cache visuellement (comme si elle était collectée)
            // Si ton étoile a des enfants (modèle 3D), utilise GetComponentInChildren<Renderer>()
            Renderer starRenderer = GetComponent<Renderer>();
            if (starRenderer != null) starRenderer.enabled = false; 
            else GetComponentInChildren<Renderer>().enabled = false;

            StartCoroutine(SequenceVictoireAuSol(other.gameObject));
        }
    }

    IEnumerator SequenceVictoireAuSol(GameObject player)
    {
        Animator anim = player.GetComponentInChildren<Animator>();
        PlayerMovement moveScript = player.GetComponent<PlayerMovement>();
        Rigidbody rb = player.GetComponent<Rigidbody>();

        // --- PHASE 1 : ON COUPE LES COMMANDES (EN L'AIR) ---
        
        // On empêche le joueur de bouger, mais on laisse la gravité agir
        if (moveScript != null) moveScript.enabled = false;
        if (orbitScript != null) orbitScript.enabled = false;

        // On stop le mouvement horizontal (pour pas qu'il glisse), mais on garde la vitesse Y (chute)
        if (rb != null)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            rb.angularVelocity = Vector3.zero;
        }

        // --- PHASE 2 : ON ATTEND L'ATTERRISSAGE ---
        
        // Tant que la vitesse verticale n'est pas proche de 0 (donc qu'il tombe encore), on attend.
        // (0.1f est une petite marge de tolérance)
        if (rb != null)
        {
            yield return new WaitUntil(() => Mathf.Abs(rb.velocity.y) < 0.1f);
        }

        // --- PHASE 3 : LE SHOW (AU SOL) ---

        // Maintenant qu'il est posé, on fige tout pour de bon
        if (rb != null) rb.isKinematic = true;

        // On lance l'interface
        if (victoryUIPanel != null) victoryUIPanel.SetActive(true);

        // On lance la danse
        if (anim != null) anim.SetTrigger("Victoire");

        // --- PHASE 4 : CAMÉRA QUI TOURNE ---
        float timer = 0f;
        Vector3 centerPoint = player.transform.position + Vector3.up * 1.5f;

        while (timer < danceDuration)
        {
            if (mainCam != null)
            {
                mainCam.transform.RotateAround(centerPoint, Vector3.up, rotationSpeed * Time.deltaTime);
                mainCam.transform.LookAt(centerPoint);
            }
            timer += Time.deltaTime;
            yield return null;
        }

        // --- PHASE 5 : DISPARITION & RELOAD ---
        if (vanishParticlePrefab != null)
            Instantiate(vanishParticlePrefab, player.transform.position + Vector3.up, Quaternion.identity);

        player.SetActive(false); // Pouf !

        yield return new WaitForSeconds(2f); // Petite pause dramatique

        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // On recommence
    }
}