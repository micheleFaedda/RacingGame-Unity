using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tachimetro : MonoBehaviour
{
    
    public GameObject velocita;

    static float minAngle = -90f;
    static float maxAngle = 180f;
    static Tachimetro thisRPM;
 
    // Use this for initialization
    void Start()
    {
        thisRPM = this;
 
    }
 
    public void ShowSpeed(float speed, float min, float max)
    {
        float speed_prop = (speed * 180)/max;

         Debug.Log(speed);
        float ang = Mathf.Lerp(minAngle, maxAngle, Mathf.InverseLerp(max, min,  speed));
        thisRPM.transform.eulerAngles = new Vector3(0, 0, ang);

        velocita.GetComponent<UnityEngine.UI.Text>().text = ""+ Mathf.Round(speed_prop) ;
    }

}
