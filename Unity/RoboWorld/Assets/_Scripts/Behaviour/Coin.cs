using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Réglages")]
    public int valeur = 1;
    public float vitesseRotation = 100f;
    public AudioClip sonCoin;
    public float volume = 0.8f;

    private bool dejaRecolte = false;

    void Start()
    {
        // Enregistrement pour pouvoir respawn
        if (GameManager.instance != null) GameManager.instance.RegisterCollectible(gameObject);

        // Sécurité : Force tous les colliders (parent et enfants) en mode Trigger
        foreach(Collider c in GetComponentsInChildren<Collider>())
        {
            c.isTrigger = true;
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.up * vitesseRotation * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (dejaRecolte) return;

        if (other.CompareTag("Player"))
        {
            dejaRecolte = true;
            // 1. Jouer le son (Crée un objet temporaire pour jouer le son nettement en 2D)
            if(sonCoin != null) 
            {
                // On crée manuellement l'AudioSource pour le mettre en 2D (spatialBlend = 0)
                // Cela évite l'effet Doppler et la distorsion due à la distance ou vitesse
                GameObject audioObj = new GameObject("CoinSoundTemp");
                audioObj.transform.position = transform.position;

                AudioSource source = audioObj.AddComponent<AudioSource>();
                source.clip = sonCoin;
                source.volume = volume;
                source.spatialBlend = 0f; // Le son est joué "partout" (2D), clair et sans déformation
                
                source.Play();
                Destroy(audioObj, sonCoin.length + 0.1f);
            }

            // 2. Ajouter au score
            if (GameManager.instance != null)
            {
                GameManager.instance.AjouterPiece(valeur);
            }
            Debug.Log("Pièce ramassée ! (+ " + valeur + ")");

            // 3. Désactiver l'objet (au lieu de détruire, pour qu'il puisse respawn)
            gameObject.SetActive(false);
        }
    }
}