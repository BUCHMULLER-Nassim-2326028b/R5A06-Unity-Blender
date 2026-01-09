using UnityEngine;

public class GlobalMaterialManager : MonoBehaviour
{
    [Header("Le Fichier Material")]
    public Material materialDuRobot;

    [Header("Réglages")]
    public string nomPropriete = "_BaseColor"; 
    
    // Ta couleur Gold par défaut
    private Color couleurDefaut = new Color(1f, 0.84f, 0f); 

    [Header("Debug")]
    public bool resetSauvegardeAuLancement = false; // COCHE ÇA POUR RÉPARER

    void Start()
    {
        // Si tu coches la case dans l'inspector, ça oublie tout et remet Gold
        if (resetSauvegardeAuLancement)
        {
            PlayerPrefs.DeleteKey("ColorSaved");
            PlayerPrefs.DeleteKey("RobotColor_R");
            PlayerPrefs.DeleteKey("RobotColor_G");
            PlayerPrefs.DeleteKey("RobotColor_B");
            Debug.Log("⚠️ MÉMOIRE COULEUR EFFACÉE !");
        }

        ChargerCouleur();
    }

    public void SetRed() { ChangerLaSource(Color.red); }
    public void SetBlue() { ChangerLaSource(Color.blue); }
    public void SetGreen() { ChangerLaSource(Color.green); }
    public void SetGold() { ChangerLaSource(couleurDefaut); }

    public void ChangerLaSource(Color nouvelleCouleur)
    {
        if (materialDuRobot != null)
        {
            materialDuRobot.SetColor(nomPropriete, nouvelleCouleur);
            SauvegarderCouleur(nouvelleCouleur);
        }
    }

    void SauvegarderCouleur(Color c)
    {
        PlayerPrefs.SetFloat("RobotColor_R", c.r);
        PlayerPrefs.SetFloat("RobotColor_G", c.g);
        PlayerPrefs.SetFloat("RobotColor_B", c.b);
        PlayerPrefs.SetInt("ColorSaved", 1);
        PlayerPrefs.Save();
    }

    void ChargerCouleur()
    {
        // Si on a une sauvegarde valide, on la charge (C'est ça le "garde-le")
        if (PlayerPrefs.HasKey("ColorSaved"))
        {
            float r = PlayerPrefs.GetFloat("RobotColor_R");
            float g = PlayerPrefs.GetFloat("RobotColor_G");
            float b = PlayerPrefs.GetFloat("RobotColor_B");
            Color c = new Color(r, g, b, 1f);
            
            if (materialDuRobot != null) materialDuRobot.SetColor(nomPropriete, c);
        }
        else
        {
            // SINON (Première fois ou après Reset), on force le GOLD
            Debug.Log("Aucune sauvegarde trouvée : Application du GOLD par défaut.");
            if (materialDuRobot != null) materialDuRobot.SetColor(nomPropriete, couleurDefaut);
        }
    }
}