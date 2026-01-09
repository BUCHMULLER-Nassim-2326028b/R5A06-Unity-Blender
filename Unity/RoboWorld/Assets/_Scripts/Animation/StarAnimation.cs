using UnityEngine;

public class StarAnimation : MonoBehaviour
{
    public float vitesseRotation = 100f;
    public float amplitudeFlottement = 0.5f; // De combien ça monte/descend
    public float vitesseFlottement = 2f;    // A quelle vitesse

    private Vector3 posDepart;

    void Start()
    {
        posDepart = transform.position;
    }

    void Update()
    {
        // 1. Rotation (Comme la toupie)
        transform.Rotate(Vector3.up * vitesseRotation * Time.deltaTime);

        // 2. Flottement (Formule magique Sinus)
        // Mathf.Sin crée une vague entre -1 et 1.
        float nouveauY = posDepart.y + Mathf.Sin(Time.time * vitesseFlottement) * amplitudeFlottement;
        
        transform.position = new Vector3(transform.position.x, nouveauY, transform.position.z);
    }
}