using UnityEngine;
using UnityEngine.UI;

public class Tachimetro : MonoBehaviour
{
    public Rigidbody target;

    public float maxSpeed = 0.0f; // The maximum speed of the target ** IN KM/H **

    public float minSpeedArrowAngle;
    public float maxSpeedArrowAngle;//Angolo della freccia quando la velocita della macchina è massima
    
    public Text speedLabel; // The label that displays the speed;
    public RectTransform arrow; // The arrow in the speedometer

    private float speed = 0.0f;
    private void Update()
    {
        // 3.6f to convert in kilometers
        // ** The speed must be clamped by the car controller **
        speed = target.velocity.magnitude * 3.6f;

        if (speedLabel != null)
            speedLabel.text = ((int)speed) + "";
        if (arrow != null)
            arrow.localEulerAngles =
                new Vector3(0, 0, Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle, speed / maxSpeed));

        /*
            String testoMarcia = "0";
        if (speed_prop > 1f && speed_prop <= maxSpeed * 0.05f )
            testoMarcia = "1";
        if (speed_prop > maxSpeed * 0.05f && speed_prop <= maxSpeed * 0.2f)
            testoMarcia = "2"; 
        if (speed_prop > maxSpeed * 0.2f && speed_prop <= maxSpeed * 0.4f)
            testoMarcia = "3";
        
        if (speed_prop >  maxSpeed * 0.4f && speed_prop <=  maxSpeed * 0.7f)
            testoMarcia = "4";
        if (speed_prop >  maxSpeed * 0.7f && speed_prop <=  maxSpeed * 0.9f)
            testoMarcia = "5";
        if (speed_prop >  maxSpeed * 0.9f) 
            testoMarcia = "6";
        
        
        marcia.GetComponent<UnityEngine.UI.Text>().text = testoMarcia ;*/
    }
}
