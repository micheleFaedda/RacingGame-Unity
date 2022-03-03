using System;
using UnityEngine;

public class Tachimetro : MonoBehaviour
{
    
    public GameObject velocita;
    public GameObject marcia;
    static float minAngle = -90f;
    static float maxAngle = 180f;
    static Tachimetro thisRPM;
    private float speed_prop;
    
    void Start()
    {
        thisRPM = this;
    }
 
    public void ShowSpeed(float speed, float min, float max)
    {   
        //Debug.Log(speed);
        speed_prop = (speed * 180)/max;
        float ang = Mathf.Lerp(minAngle, maxAngle, Mathf.InverseLerp(max, min,  speed));
        thisRPM.transform.eulerAngles = new Vector3(0, 0, ang);
        velocita.GetComponent<UnityEngine.UI.Text>().text = ""+ Mathf.Round(speed_prop) ;
    }

    public void MostraMarcia()
    {
        String testoMarcia = "0";
        
        if (speed_prop > 0f && speed_prop <= 20f)
            testoMarcia = "1";
        if (speed_prop > 20f && speed_prop <= 50f)
            testoMarcia = "2"; 
        if (speed_prop > 50f && speed_prop <= 100f)
            testoMarcia = "3";
        
        if (speed_prop > 100f && speed_prop <= 120f)
            testoMarcia = "4";
        if (speed_prop > 120f)
            testoMarcia = "5";
        
        
        marcia.GetComponent<UnityEngine.UI.Text>().text = testoMarcia ;
        
        
    }

}
