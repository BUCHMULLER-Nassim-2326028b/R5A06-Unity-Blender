using UnityEngine;

public class CameraMovementOrbital : MonoBehaviour
{
    [Header("Cibles")]
    public Transform target; // Glisse ton Player ici
    public float heightOffset = 1.5f; // Pour viser la tête et pas les pieds

    [Header("Réglages")]
    public float distance = 6.0f; // Distance derrière le joueur
    public float sensitivity = 3.0f; // Vitesse de rotation
    public float yMinLimit = -20f; // Angle min (pour pas passer sous le sol)
    public float yMaxLimit = 80f;  // Angle max (pour pas se tordre le cou)

    private float currentX = 0.0f;
    private float currentY = 0.0f;

    void Start()
    {
        // On initialise les angles actuels de la caméra pour éviter qu'elle saute au début
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;
    }

    void LateUpdate()
    {
        // Sécurité : si pas de joueur, on fait rien
        if (target == null) return;

        // --- GESTION DU CLIC DROIT ---
        // 0 = Clic Gauche, 1 = Clic Droit, 2 = Molette
        if (Input.GetMouseButton(1)) 
        {
            // Optionnel : On cache le curseur pour que ce soit plus fluide
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // On ajoute le mouvement de la souris aux angles
            currentX += Input.GetAxis("Mouse X") * sensitivity;
            currentY -= Input.GetAxis("Mouse Y") * sensitivity;

            // On bloque l'angle vertical (Clamp) pour ne pas faire de looping
            currentY = Mathf.Clamp(currentY, yMinLimit, yMaxLimit);
        }
        else
        {
            // On réaffiche le curseur quand on lâche le clic
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        // --- CALCUL DE LA POSITION ---
        // C'est des maths de rotation (Quaternion), fais-moi confiance ça marche
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        
        // On recule la caméra de la "distance" par rapport à la rotation
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        
        // Position finale = Position Joueur + Hauteur + (Rotation * Recul)
        Vector3 position = rotation * negDistance + target.position + new Vector3(0, heightOffset, 0);

        // On applique les changements
        transform.rotation = rotation;
        transform.position = position;
    }
}