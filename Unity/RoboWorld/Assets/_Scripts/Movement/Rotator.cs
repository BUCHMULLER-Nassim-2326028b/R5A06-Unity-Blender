using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 vitesseRotation = new Vector3(0, 50, 0); // Tourne sur l'axe Y (hauteur)

    void Update()
    {
        // Fait tourner l'objet un peu à chaque frame
        transform.Rotate(vitesseRotation * Time.deltaTime);
    }
}