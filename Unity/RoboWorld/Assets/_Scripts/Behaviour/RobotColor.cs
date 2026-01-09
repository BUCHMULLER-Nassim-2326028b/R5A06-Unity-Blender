using UnityEngine;

public class RobotColor : MonoBehaviour
{
    public Renderer[] renderersRobot; // Le robot du menu
    
    public static Color couleurSelectionnee = Color.white; 

    public void SetColorRed() { UpdateColor(Color.red); }
    public void SetColorBlue() { UpdateColor(Color.blue); }
    public void SetColorGreen() { UpdateColor(Color.green); }
    public void SetColorGold() { UpdateColor(new Color(1f, 0.84f, 0f)); }

    void UpdateColor(Color c)
    {
        couleurSelectionnee = c;
        foreach (var r in renderersRobot)
        {
            r.material.SetColor("_BaseColor", c);
        }
    }

    // Fonction appelée par le MenuManager quand on lance le jeu
    public static void ApplyToPlayer()
    {
        // 1. On cherche le joueur par son TAG
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if (player == null)
        {
            Debug.LogError("ERREUR COULEUR : Aucun objet avec le tag 'Player' trouvé !");
            return;
        }

        // 2. On récupère tous les renderers du vrai joueur
        Renderer[] playerRenderers = player.GetComponentsInChildren<Renderer>();
        
        if (playerRenderers.Length == 0)
        {
            Debug.LogError("ERREUR COULEUR : Le joueur a été trouvé mais n'a pas de Renderer !");
            return;
        }

        // 3. On applique
        foreach (var r in playerRenderers) 
        {
            // Debug.Log("Couleur appliquée sur : " + r.name);
            r.material.SetColor("_BaseColor", couleurSelectionnee);
        }
    }
}