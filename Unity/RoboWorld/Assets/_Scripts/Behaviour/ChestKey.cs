using UnityEngine;

public class ChestKey : MonoBehaviour
{
    [Header("Réglages")]
    public TreasureChest coffreCible; // Le coffre que cette clé ouvre
    public AudioClip sonCle; // Un petit "Ding"

    void Start()
    {
        if (GameManager.instance != null) GameManager.instance.RegisterCollectible(gameObject);
    }

    void Update()
    {
        // Petite rotation pour faire joli
        transform.Rotate(Vector3.up * 100 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (coffreCible != null)
            {
                coffreCible.AvoirLaCle(); // On dit au coffre "C'est bon, j'ai la clé !"
                
                if (GameManager.instance != null)
                {
                    GameManager.instance.RamasserCle();
                }

                if(sonCle != null) AudioSource.PlayClipAtPoint(sonCle, transform.position);
                
                Debug.Log("🗝️ Clé trouvée !");
                gameObject.SetActive(false); // La clé disparaît
            }
        }
    }
}