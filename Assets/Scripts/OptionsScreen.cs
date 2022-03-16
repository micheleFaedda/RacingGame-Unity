using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsScreen : MonoBehaviour
{
    public Toggle fullscreen;
    public List<ResItem> resultions = new List<ResItem>();
    private int selectedResolution;
    public TMP_Text resolutionLabel;
    
    
    void Start()
    {

        fullscreen.isOn = Screen.fullScreen;

        // se faccio partire il gioco con una risoluzione tra quelle non di default, la aggiungo alla lista
        bool foundRes = false;

        for(int i = 0; i < resultions.Count; i++)
        {
            if(Screen.width == resultions[i].horizontal && Screen.height == resultions[i].vertical)
            {
                foundRes = true;
                selectedResolution = i;
                updateResLabel();
            }
        }

        if (!foundRes)
        {
            ResItem newRes = new ResItem();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;

            resultions.Add(newRes);
            selectedResolution = resultions.Count - 1;
            updateResLabel();
        }
    }

    //scorro verso destra o sinistra la lista di risoluzioni, cambiando quindi il label corrispondente
    public void ResLeft()
    {
        selectedResolution--;
        if (selectedResolution < 0)
        {
            selectedResolution = 0;
        }
        updateResLabel();
    }

    public void ResRight()
    {
        selectedResolution++;
        if (selectedResolution > resultions.Count - 1)
        {
            selectedResolution = resultions.Count - 1;
        }

        updateResLabel();


    }

    public void updateResLabel()
    {
        resolutionLabel.text = resultions[selectedResolution].horizontal.ToString() + " x "
            + resultions[selectedResolution].vertical.ToString();
    }

    //applico i cambiamenti grafici (risoluzione e/o fullscreen)
    public void ApplyChangesGraph()
    {
        
       
        Screen.SetResolution(resultions[selectedResolution].horizontal, resultions[selectedResolution].vertical
            , fullscreen.isOn);
    }
}

//classe per definire la risoluzione
[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;

}
