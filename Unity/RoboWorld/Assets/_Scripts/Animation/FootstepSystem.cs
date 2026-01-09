using UnityEngine;

public class FootstepSystem : MonoBehaviour
{
    [Header("Réglages")]
    public AudioSource sourceAudio; // Glisse ton AudioSource ici
    public AudioClip[] sonsPas; // Mets tes 3-4 sons de pas ici pour varier
    
    [Range(0.8f, 1.2f)]
    public float pitchMin = 0.9f; // Pour varier le grave/aigu
    [Range(0.8f, 1.2f)]
    public float pitchMax = 1.1f;

    // Cette fonction sera appelée par l'Animation
    public void Pas()
    {
        // Sécurité : Si pas de sons ou pas de source, on annule
        if (sonsPas.Length == 0 || sourceAudio == null) return;

        // 1. On choisit un son au hasard (pour pas avoir l'effet mitraillette)
        AudioClip clip = sonsPas[Random.Range(0, sonsPas.Length)];

        // 2. On change un peu le pitch (la hauteur) pour que chaque pas soit unique
        sourceAudio.pitch = Random.Range(pitchMin, pitchMax);
        
        // 3. On change un peu le volume aussi
        sourceAudio.volume = Random.Range(0.8f, 1.0f);

        // 4. On joue le son
        sourceAudio.PlayOneShot(clip);
    }
}