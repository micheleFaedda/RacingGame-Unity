using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsScreen : MonoBehaviour
{
    public Toggle fullscreen, vsync;
    public List<ResItem> resultions = new List<ResItem>();
    private int selectedResolution;
    public TMP_Text resolutionLabel;
    
    // Start is called before the first frame update
    void Start()
    {
        fullscreen.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0)
        {
            vsync.isOn = false;
        }
        else
        {
            vsync.isOn = true;
        }

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

    // Update is called once per frame
    void Update()
    {

    }

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
            + resultions[selectedResolution].horizontal.ToString();
    }

    public void ApplyChangesGraph()
    {
        
        if (vsync.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;

        }
        Screen.SetResolution(resultions[selectedResolution].horizontal, resultions[selectedResolution].vertical
            , fullscreen.isOn);
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;

}
