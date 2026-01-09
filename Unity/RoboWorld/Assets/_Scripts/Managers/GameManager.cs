using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Stats")]
    public int energie = 3;
    public int energieMax = 3;
    public int pieces = 0;
    public int orbesArgentees = 0;
    public bool aLaCleDuCoffre = false;

    [Header("Death Settings")]
    public GameObject playerVanish; 
    public Transform spawnPoint; 
    public float respawnDelay = 2.0f; 

    [Header("UI References")]
    public TextMeshProUGUI texteEnergie; 
    public TextMeshProUGUI textePieces; 
    public TextMeshProUGUI texteOrbes; 
    public GameObject iconeCle; 
    public GameObject panneauCoffre; 
    public TextMeshProUGUI texteCoffre; 

    private System.Collections.Generic.List<GameObject> registeredCollectibles = new System.Collections.Generic.List<GameObject>();

    public void RegisterCollectible(GameObject obj)
    {
        if (!registeredCollectibles.Contains(obj))
        {
            registeredCollectibles.Add(obj);
        }
    }

    public void ResetGame()
    {
        // Reset Stats
        pieces = 0;
        orbesArgentees = 0;
        aLaCleDuCoffre = false;
        energie = energieMax;
        
        // Reset Collectibles
        foreach (GameObject obj in registeredCollectibles)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }

        // Reset OrbGate
        OrbGate gate = FindObjectOfType<OrbGate>();
        if (gate != null) gate.ResetState();

        UpdateUI();
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
        if(panneauCoffre != null) panneauCoffre.SetActive(false);
        if(iconeCle != null) iconeCle.SetActive(false);
    }

    public void ModifierEnergie(int montant)
    {
        energie += montant;
        if (energie > energieMax) energie = energieMax;
        if (energie <= 0)
        {
            energie = 0;
            GameOver();
        }
        UpdateUI();
    }

    void GameOver()
    {
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Disappear effect
            if (playerVanish != null)
            {
                Instantiate(playerVanish, player.transform.position, Quaternion.identity);
            }

            // Hide player
            player.SetActive(false);

            yield return new WaitForSeconds(respawnDelay);

            // Respawn
            if (spawnPoint != null)
            {
                player.transform.position = spawnPoint.position;
                player.transform.rotation = spawnPoint.rotation;
            }

            player.SetActive(true);

            // Reset physics
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        ResetGame();
    }

    public void AjouterPiece(int montant)
    {
        pieces += montant;
        UpdateUI();
    }

    public void AjouterOrbe()
    {
        orbesArgentees++;
        UpdateUI();
    }

    public void RamasserCle()
    {
        aLaCleDuCoffre = true;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (texteEnergie != null) texteEnergie.text = energie.ToString();
        if (textePieces != null) textePieces.text = "x " + pieces;
        if (texteOrbes != null) texteOrbes.text = "x " + orbesArgentees;
        if (iconeCle != null) iconeCle.SetActive(aLaCleDuCoffre);
    }

    public void AfficherInteractionCoffre(bool visible, string message = "")
    {
        if (panneauCoffre != null)
        {
            panneauCoffre.SetActive(visible);
            if (visible && texteCoffre != null)
            {
                texteCoffre.text = message;
            }
        }
    }
}
