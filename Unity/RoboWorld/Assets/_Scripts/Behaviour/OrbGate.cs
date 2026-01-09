using UnityEngine;
using TMPro; 

public class OrbGate : MonoBehaviour
{
    [Header("Réglages")]
    public int clesNecessaires = 3;
    private int clesActuelles = 0;

    [Header("Références")]
    public GameObject cageObtacle; 
    public TextMeshProUGUI compteurTexte; 

    void Start()
    {
        UpdateUI();
    }

    public void RamasserCle()
    {
        clesActuelles++;
        Debug.Log("Orbe ramassée ! " + clesActuelles + "/" + clesNecessaires);
        
        UpdateUI();

        if (clesActuelles >= clesNecessaires)
        {
            OuvrirCage();
        }
    }

    void OuvrirCage()
    {
        Debug.Log("CAGE OUVERTE !");
        
        if(cageObtacle != null) 
        {
            cageObtacle.SetActive(false);
            // todo: add un son ici
        }
    }

    void UpdateUI()
    {
        if (compteurTexte != null)
        {
            compteurTexte.text = "Orbes : " + clesActuelles + " / " + clesNecessaires;
        }
    }

    public void ResetState()
    {
        clesActuelles = 0;
        if(cageObtacle != null) cageObtacle.SetActive(true);
        UpdateUI();
    }
}