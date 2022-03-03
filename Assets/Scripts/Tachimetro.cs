using UnityEngine;

public class Tachimetro : MonoBehaviour
{
    
    public GameObject velocita;
    static float minAngle = -90f;
    static float maxAngle = 180f;
    static Tachimetro thisRPM;
    
    void Start()
    {
        thisRPM = this;
    }
 
    public void ShowSpeed(float speed, float min, float max)
    {   
        //Debug.Log(speed);
        float speed_prop = (speed * 180)/max;
        float ang = Mathf.Lerp(minAngle, maxAngle, Mathf.InverseLerp(max, min,  speed));
        thisRPM.transform.eulerAngles = new Vector3(0, 0, ang);
        velocita.GetComponent<UnityEngine.UI.Text>().text = ""+ Mathf.Round(speed_prop) ;
    }

}
