using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public Transform SpawnPoint; // Là où on va réapparaître

    private void OnTriggerEnter(Collider other)
    {
        // Si l'objet qui entre est le Joueur
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero; // (Utilise rb.velocity si Unity < 2023)
            }

            // 2. Téléportation (On doit désactiver le CharacterController si tu en utilisais un, mais là on est en Rigidbody donc c'est simple)
            other.transform.position = SpawnPoint.position;
        }
    }
}  