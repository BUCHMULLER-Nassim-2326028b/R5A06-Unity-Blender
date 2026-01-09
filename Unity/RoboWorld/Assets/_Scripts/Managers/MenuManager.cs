using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject customMenuPanel;
    public GameObject pausePanel;       // Canvas Overlay
    public GameObject gameUI;           // HUD

    [Header("Boutons Retour (Dans Custom Panel)")]
    public GameObject btnRetourMenu;    // Bouton "Retour Accueil"
    public GameObject btnRetourPause;   // Bouton "Retour Pause"

    [Header("Studio & Caméras")]
    public Camera menuCamera;
    public Camera playerCamera;
    public GameObject menuRobotObject;  // Robot du Studio

    [Header("Le Vrai Joueur")]
    public GameObject realPlayer;       
    public Transform spawnPoint;

    // État du jeu
    private bool jeuEnCours = false; 
    private bool enEditionDepuisPause = false; // <--- LA VARIABLE QUI SAUVE TOUT

    void Start()
    {
        ShowMainMenu();
    }

    void Update()
    {
        // Gestion de la touche ECHAP
        if (jeuEnCours && Input.GetKeyDown(KeyCode.Escape))
        {
            // Cas 1 : On est dans le menu custom (venant de la pause) -> On retourne en pause
            if (enEditionDepuisPause)
            {
                BackToPause();
            }
            // Cas 2 : On est en pause -> On reprend le jeu
            else if (pausePanel.activeSelf) 
            {
                ResumeGame();
            }
            // Cas 3 : On joue -> On met pause
            else 
            {
                PauseGame();
            }
        }
    }

    // --- NAVIGATION PRINCIPALE ---

    public void ShowMainMenu()
    {
        jeuEnCours = false;
        enEditionDepuisPause = false;
        
        ResetAllUI();
        mainMenuPanel.SetActive(true);
        
        // Caméras & Studio
        menuCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);
        if(menuRobotObject != null) menuRobotObject.SetActive(false); // Caché sur l'accueil
        
        TogglePlayerControl(false);
        Time.timeScale = 1f; 
    }

    public void ShowCustomMenu() // Depuis l'accueil
    {
        ResetAllUI();
        customMenuPanel.SetActive(true);
        enEditionDepuisPause = false; // On vient de l'accueil

        if(menuRobotObject != null) menuRobotObject.SetActive(true);

        // Gestion des boutons
        if(btnRetourMenu) btnRetourMenu.SetActive(true);
        if(btnRetourPause) btnRetourPause.SetActive(false);
    }

    public void PlayGame()
    {
        enEditionDepuisPause = false;

        // Cache le studio
        menuCamera.gameObject.SetActive(false);
        if(menuRobotObject != null) menuRobotObject.SetActive(false);

        // Affiche le jeu
        ResetAllUI();
        gameUI.SetActive(true);
        playerCamera.gameObject.SetActive(true);
        
        // Reset du jeu (Items, scores...)
        if(GameManager.instance != null) GameManager.instance.ResetGame();

        jeuEnCours = true;
        Time.timeScale = 1f;

        TogglePlayerControl(true);
        ResetPlayerState(); 
    }


    public void PauseGame()
    {
        pausePanel.SetActive(true);
        gameUI.SetActive(false);
        Time.timeScale = 0f; // Stop le temps
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        gameUI.SetActive(true);
        Time.timeScale = 1f;
    }

    // --- LE FLUX PAUSE <-> CUSTOM ---

    public void OpenCustomFromPause()
    {
        enEditionDepuisPause = true; // <--- ON MÉMORISE QU'ON VIENT DE LA PAUSE

        pausePanel.SetActive(false); // Ferme l'overlay pause

        // Switch Caméra vers Studio
        playerCamera.gameObject.SetActive(false);
        menuCamera.gameObject.SetActive(true);

        // Affiche Robot & Menu
        if(menuRobotObject != null) menuRobotObject.SetActive(true);
        customMenuPanel.SetActive(true);

        // Gestion des boutons (Retour Pause activé)
        if(btnRetourMenu) btnRetourMenu.SetActive(false);
        if(btnRetourPause) btnRetourPause.SetActive(true);
    }

    public void BackToPause()
    {
        enEditionDepuisPause = false; // On a fini l'édition

        // Ferme Custom & Robot
        customMenuPanel.SetActive(false);
        if(menuRobotObject != null) menuRobotObject.SetActive(false);

        // Retour Caméra Jeu
        menuCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);

        // Réouvre la Pause
        pausePanel.SetActive(true);
    }

    // --- UTILITAIRES ---

    void TogglePlayerControl(bool state)
    {
        if (realPlayer != null)
        {
            PlayerMovement pm = realPlayer.GetComponent<PlayerMovement>();
            if (pm != null) pm.enabled = state;
        }
    }

    void ResetPlayerState()
    {
        if(realPlayer != null)
        {
            // Spawn au point de spawn défini, sinon on laisse le joueur où il est (ou à 0,0 si besoin, mais l'utilisateur voulait éviter le 0,0 forcé)
            if (spawnPoint != null)
            {
                realPlayer.transform.position = spawnPoint.position;
                realPlayer.transform.rotation = spawnPoint.rotation;
            }

            Rigidbody rb = realPlayer.GetComponent<Rigidbody>();
            if(rb) rb.velocity = Vector3.zero;
            
            PlayerMovement pm = realPlayer.GetComponent<PlayerMovement>();
            if(pm != null) pm.enduranceActuelle = pm.enduranceMax;
        }
        // Note : Plus besoin d'appliquer la couleur ici, c'est automatique avec le Material
    }

    void ResetAllUI()
    {
        mainMenuPanel.SetActive(false);
        customMenuPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameUI.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}