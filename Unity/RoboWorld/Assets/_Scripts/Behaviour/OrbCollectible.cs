using UnityEngine;

public class OrbCollectible : MonoBehaviour
{
    [Header("Réglages")]
    public float vitesseRotation = 100f;
    public OrbGate scriptCage; // Le lien vers la cage à ouvrir

    void Start()
    {
        if (GameManager.instance != null) GameManager.instance.RegisterCollectible(gameObject);

        // AUTO-DETECTION : Si la cage n'est pas assignée manuellement (cas des prefabs), on la cherche
        if (scriptCage == null)
        {
            scriptCage = FindObjectOfType<OrbGate>();
        }

        // Sécurité : On remet les colliders en mode Trigger pour éviter la physique compliquée
        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            c.isTrigger = true;
        }
    }

    void Update()
    {
        // Ça tourne pour attirer l'oeil
        transform.Rotate(Vector3.up * vitesseRotation * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (scriptCage != null)
            {
                scriptCage.RamasserCle();
            }
            
            if (GameManager.instance != null)
            {
                GameManager.instance.AjouterOrbe();
            }

            if (scriptCage == null)
            {
                Debug.Log("Clé récupérée, mais aucune Cage trouvée !");
            }

            gameObject.SetActive(false);
        }
    }
}