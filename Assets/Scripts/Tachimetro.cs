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
    public float velocitaStampa = 180f;
    
    void Start()
    {
        thisRPM = this;
    }
 
    public void ShowSpeed(float speed, float min, float max)
    {   
        //Debug.Log(speed);
        speed_prop = (speed * velocitaStampa)/max;
        float ang = Mathf.Lerp(minAngle, maxAngle, Mathf.InverseLerp(max, min,  speed));
        thisRPM.transform.eulerAngles = new Vector3(0, 0, ang);
        velocita.GetComponent<UnityEngine.UI.Text>().text = ""+ Mathf.Round(speed_prop) ;
    }

    public void MostraMarcia()
    {
        String testoMarcia = "0";
        
        if (speed_prop > 1f && speed_prop <= velocitaStampa * 0.05f )
            testoMarcia = "1";
        if (speed_prop > velocitaStampa * 0.05f && speed_prop <= velocitaStampa * 0.2f)
            testoMarcia = "2"; 
        if (speed_prop > velocitaStampa * 0.2f && speed_prop <= velocitaStampa * 0.4f)
            testoMarcia = "3";
        
        if (speed_prop >  velocitaStampa * 0.4f && speed_prop <=  velocitaStampa * 0.7f)
            testoMarcia = "4";
        if (speed_prop >  velocitaStampa * 0.7f && speed_prop <=  velocitaStampa * 0.9f)
            testoMarcia = "5";
        if (speed_prop >  velocitaStampa * 0.9f) 
            testoMarcia = "6";
        
        
        marcia.GetComponent<UnityEngine.UI.Text>().text = testoMarcia ;
        
        
    }

}
